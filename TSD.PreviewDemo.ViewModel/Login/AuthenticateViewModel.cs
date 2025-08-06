using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Common.Platform;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.ViewModel.Login
{
    public class AuthenticateViewModel : BaseViewModel
    {
        public ReactiveCommand<string, LoginData> Login { get; }
        public ReactiveCommand<string, Unit> ChangeStorage { get; }
        public ReactiveCommand<Unit, Unit> BackNavigate { get; }

        public AuthenticateViewModel(IUserService userService, IDeviceIdentityProvider deviceIdentityProvider, IStateNavigationService stateNavigationService)
        {
            userService.ThrowIfNull(nameof(userService));
            deviceIdentityProvider.ThrowIfNull(nameof(deviceIdentityProvider));

            this.WhenAnyValue(
                vm => vm.BarCode,
                vm => vm.BarCode, (s, _) => !string.IsNullOrWhiteSpace(BarCode));



            BackNavigate = ReactiveCommand.CreateFromTask(async () =>
            {
                await stateNavigationService.SetState(User, "Cancel");
            },
                Observable.Return(true),
                RxApp.MainThreadScheduler);

            Login = ReactiveCommand.CreateFromTask<string, LoginData>(async(barcode) =>
                {
                    var user = await userService.Init(deviceIdentityProvider.DeviceId);
                    user = await userService.CompleteInit(user);
                    user.BarCode = barcode;
                    var ld = await userService.Login(user);
                    User = ld;
                    return ld;
                },
                Observable.Return(true),
                RxApp.MainThreadScheduler);

            ChangeStorage = ReactiveCommand.CreateFromTask<string>(async storage =>
                {
                   await userService.ChangeStorage(storage, User);
                },
                Observable.Return(true),
                RxApp.MainThreadScheduler);
        }

        public string BarCode
        {
            get => _barCode;
            set => this.RaiseAndSetIfChanged(ref _barCode, value);
        }

        public LocationChangeListData LocationChangeListData
        {
            get => _locationChangeListData;
            set => this.RaiseAndSetIfChanged(ref _locationChangeListData, value);
        }

        private string _barCode;
        private LocationChangeListData _locationChangeListData;
    }
}
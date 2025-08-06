using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.ViewModel.Menu
{
    public class MenuViewModel : BaseViewModel
    {
        private string _navigateTo;

        public string NavigateTo
        {
            get => _navigateTo;
            set => this.RaiseAndSetIfChanged(ref _navigateTo, value);
        }

        public ReactiveCommand<Unit, IEntity> Navigate { get; }

        public new User User { get; set; }

        public ReactiveCommand<Unit, User> LogOut { get; }

        public MenuViewModel(IStateNavigationService stateNavigationService, IUserService userService)
        {
            stateNavigationService.ThrowIfNull(nameof(stateNavigationService));
            userService.ThrowIfNull(nameof(userService));

            Navigate = ReactiveCommand.CreateFromTask(async () =>
                {
                    var serviceResponse = await stateNavigationService.SetState(User, NavigateTo);
                    return serviceResponse;
                }, Observable.Return(true),
                RxApp.MainThreadScheduler);

            LogOut = ReactiveCommand.CreateFromTask(async () =>
                {
                    var userResult = await userService.Logout(User);
                    User = userResult;
                    return userResult;
                },
                Observable.Return(true),
                RxApp.MainThreadScheduler);
        }
    }
}
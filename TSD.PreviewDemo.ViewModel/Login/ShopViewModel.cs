using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using TSD.PreviewDemo.Common.Extensions;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Shops;
using TSD.PreviewDemo.Core.Users;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace TSD.PreviewDemo.ViewModel.Login
{
    public class ShopViewModel : BaseViewModel
    {
        public ShopViewModel(IShopService shopService, IUserService userService)
        {
            shopService.ThrowIfNull(nameof(shopService));
            
            SetShopAndStorage = ReactiveCommand.CreateFromTask(async () =>
                {
                    User =  await shopService.SetShop(User, ShopId, StorageId);
                    User = await shopService.CompleteSetShop(User);
                    return User;
                },
                Observable.Return(true),
                RxApp.MainThreadScheduler);

            SelectShop = ReactiveCommand.CreateFromTask(async () =>
                {
                    return User.Shops.First(s => s.Id == ShopId);
                },
                Observable.Return(true),
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

        public ReactiveCommand<Unit, User> LogOut { get; }
        public ReactiveCommand<Unit, User> SetShopAndStorage { get; }
        public ReactiveCommand<Unit, Shop> SelectShop { get; }

        public string ShopId
        {
            get => _shopId;
            set => this.RaiseAndSetIfChanged(ref _shopId, value);
        }

        public string StorageId
        {
            get => _storageId;
            set => this.RaiseAndSetIfChanged(ref _storageId, value);
        }

        private string _shopId;
        private string _storageId;
    }
}

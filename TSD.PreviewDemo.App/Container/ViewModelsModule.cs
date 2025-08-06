using Autofac;
using System;
using TSD.PreviewDemo.App.ViewData;
using TSD.PreviewDemo.Common.Platform;
using TSD.PreviewDemo.Core;
using TSD.PreviewDemo.Core.Interfaces.Services;
using TSD.PreviewDemo.Core.Interfaces.Services.Utility;
using TSD.PreviewDemo.Core.StoreAcceptance;
using TSD.PreviewDemo.Core.Users;
using TSD.PreviewDemo.ViewModel.Login;
using TSD.PreviewDemo.ViewModel.Menu;
using TSD.PreviewDemo.ViewModel.StoreAcceptance;
using TSD.PreviewDemo.ViewModel.Update;

namespace TSD.PreviewDemo.App.Container
{
    internal class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticateViewModel>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IUserService),
                    (_, ctx) => ctx.Resolve<IUserService>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IDeviceIdentityProvider),
                    (_, ctx) => ctx.Resolve<IDeviceIdentityProvider>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IStateNavigationService),
                    (_, ctx) => ctx.Resolve<IStateNavigationService>())
                .OnActivating(args =>
                {
                    args.Instance.User ??= args.Context.Resolve<IActivitySharedDataManager>().Get<LoginData>();
                    args.Instance.LocationChangeListData ??= args.Context.Resolve<IActivitySharedDataManager>().Get<LocationChangeListData>();
                })
                .SingleInstance();

            builder.RegisterType<MenuViewModel>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IUserService),
                    (_, ctx) => ctx.Resolve<IUserService>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IDeviceIdentityProvider),
                    (_, ctx) => ctx.Resolve<IDeviceIdentityProvider>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IStateNavigationService),
                    (_, ctx) => ctx.Resolve<IStateNavigationService>())
                .OnActivating(args =>
                {
                    args.Instance.User ??= args.Context.Resolve<IActivitySharedDataManager>().Get<LoginData>();
                })
                .SingleInstance();

            builder.RegisterType<StoreAcceptanceViewModel>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IUserService),
                    (_, ctx) => ctx.Resolve<IUserService>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IDeviceIdentityProvider),
                    (_, ctx) => ctx.Resolve<IDeviceIdentityProvider>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IStateNavigationService),
                    (_, ctx) => ctx.Resolve<IStateNavigationService>())
                .OnActivating(args =>
                {
                    args.Instance.User ??= args.Context.Resolve<IActivitySharedDataManager>().Get<LoginData>();
                    args.Instance.ProductScanData ??= args.Context.Resolve<IActivitySharedDataManager>().Get<ProductScanData>();
                    args.Instance.JobList ??= args.Context.Resolve<IActivitySharedDataManager>().Get<JobList>();
                })
                .SingleInstance();

            builder.RegisterType<ShopViewModel>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IShopService),
                    (_, ctx) => ctx.Resolve<IShopService>())
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IUserService),
                    (_, ctx) => ctx.Resolve<IUserService>())
                .OnActivating(args =>
                {
                    args.Instance.User ??= args.Context.Resolve<IActivitySharedDataManager>().Get<LoginData>();
                })
                .SingleInstance();

            builder.RegisterType<UpdateViewModel>()
                .WithParameter(
                    (info, _) => info.ParameterType == typeof(IUpdateApplicationService),
                    (_, ctx) => ctx.Resolve<IUpdateApplicationService>())
                .SingleInstance();

            builder.Register<Action<AuthenticateViewModel, IContainer>>(_ =>
            {
                return (model, container) =>
                {
                    model.User = container.Resolve<IActivitySharedDataManager>().Get<LoginData>();
                    model.LocationChangeListData = container.Resolve<IActivitySharedDataManager>().Get<LocationChangeListData>();
                };
            });

            builder.Register<Action<ShopViewModel, IContainer>>(_ =>
            {
                return (model, container) => model.User = container.Resolve<IActivitySharedDataManager>().Get<LoginData>();
            });

            builder.Register<Action<MenuViewModel, IContainer>>(_ =>
            {
                return (model, container) => model.User = container.Resolve<IActivitySharedDataManager>().Get<LoginData>();
            });

            builder.Register<Action<StoreAcceptanceViewModel, IContainer>>(_ =>
            {
                return (model, container) => model.User = container.Resolve<IActivitySharedDataManager>().Get<LoginData>();
            });
            base.Load(builder);
        }
    }
}
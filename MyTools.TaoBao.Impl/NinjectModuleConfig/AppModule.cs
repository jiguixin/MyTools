/*
 *名称：AppModule
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-10 09:58:21
 *修改时间：
 *备注：
 */

using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Logging.TraceSource;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Impl.Authorization;
using MyTools.TaoBao.Interface;
using MyTools.TaoBao.Interface.Authorization;
using Ninject.Modules;
using Top.Api;
using Top.Api.Util;

namespace MyTools.TaoBao.Impl.NinjectModuleConfig
{
    /// <summary>
    /// 通过Ninject注册模型
    /// </summary>
    public class AppModule : NinjectModule
    { 
        public override void Load()
        {
            this.Bind<IShopApi>().To<ShopApi>().InSingletonScope();
            this.Bind<IItemCats>().To<ItemCats>().InSingletonScope();
            this.Bind<IAuthorization>().To<DefaultAuthorization>().InSingletonScope();

            this.Bind<IGoodsApi>().To<GoodsApi>().InSingletonScope();

            this.Bind<ITopClient>().To<DefaultTopClient>().InSingletonScope().WithConstructorArgument("serverUrl", Resource.SysConfig_RealTaobaoServerUrl).WithConstructorArgument("appKey", SysConst.AppKey).WithConstructorArgument("appSecret", SysConst.AppSecret);

            this.Bind<ILoggerFactory>().ToMethod(x => new TraceSourceLogFactory()).InSingletonScope();

            this.Bind<IBanggoMgt>().To<BanggoMgt>().InSingletonScope();
              
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyTools.TaoBao.DomainModule {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MyTools.TaoBao.DomainModule.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 没有获取到认证Code 的本地化字符串。
        /// </summary>
        public static string Exception_NotFoundAuthorizedCode {
            get {
                return ResourceManager.GetString("Exception_NotFoundAuthorizedCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 产入的Url无效 的本地化字符串。
        /// </summary>
        public static string Exception_UrlInvalid {
            get {
                return ResourceManager.GetString("Exception_UrlInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 通过该XPATH没有获取到正确的数据,{0} 的本地化字符串。
        /// </summary>
        public static string Exception_XPathGetDataError {
            get {
                return ResourceManager.GetString("Exception_XPathGetDataError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 在 [{0}] 方法中参数不能为空 的本地化字符串。
        /// </summary>
        public static string ExceptionTemplate_MethedParameterIsNullorEmpty {
            get {
                return ResourceManager.GetString("ExceptionTemplate_MethedParameterIsNullorEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 删除SKU失败，numId：{0}; properties:{1};GoodsSn:{2} 的本地化字符串。
        /// </summary>
        public static string Log_DeleteGoodsSkuFailure {
            get {
                return ResourceManager.GetString("Log_DeleteGoodsSkuFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在执行删除SKU..，numId：{0}; properties:{1};GoodsSn:{2} 的本地化字符串。
        /// </summary>
        public static string Log_DeleteGoodsSkuing {
            get {
                return ResourceManager.GetString("Log_DeleteGoodsSkuing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 删除SKU成功，numId：{0}; properties:{1};GoodsSn:{2} 的本地化字符串。
        /// </summary>
        public static string Log_DeleteGoodsSkuSuccess {
            get {
                return ResourceManager.GetString("Log_DeleteGoodsSkuSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 numId:{0},imgId:{1};GoodsSn:{2}删除销售图片出错! 的本地化字符串。
        /// </summary>
        public static string Log_DeleteItemPropimgFailure {
            get {
                return ResourceManager.GetString("Log_DeleteItemPropimgFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 numId:{0},imgId:{1};GoodsSn:{2},删除销售图片成功 的本地化字符串。
        /// </summary>
        public static string Log_DeleteItemPropimgSuccess {
            get {
                return ResourceManager.GetString("Log_DeleteItemPropimgSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 numId:{0},imgId:{1};GoodsSn:{2},正在删除销售图片... 的本地化字符串。
        /// </summary>
        public static string Log_DeleteItemPropingimg {
            get {
                return ResourceManager.GetString("Log_DeleteItemPropingimg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 GoodsSn:{0}-&gt;导Banggo库存数据与淘宝对手销售数据出错! 的本地化字符串。
        /// </summary>
        public static string Log_ExportBanggoAndTaobaoGoodsInfoBySearchFailure {
            get {
                return ResourceManager.GetString("Log_ExportBanggoAndTaobaoGoodsInfoBySearchFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 GoodsSn:{0}-&gt;正在导出Banggo库存数据.... 的本地化字符串。
        /// </summary>
        public static string Log_ExportProductColorForExceling {
            get {
                return ResourceManager.GetString("Log_ExportProductColorForExceling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 GoodsSn:{0}-&gt;导出Banggo库存数据成功! 的本地化字符串。
        /// </summary>
        public static string Log_ExportProductColorForExcelSuccess {
            get {
                return ResourceManager.GetString("Log_ExportProductColorForExcelSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 query:{0}-&gt;正在导出该产品所有对手的销售数据.... 的本地化字符串。
        /// </summary>
        public static string Log_ExportRivalGoodsInfoing {
            get {
                return ResourceManager.GetString("Log_ExportRivalGoodsInfoing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 query:{0}-&gt;导出该产品所有对手的销售数据成功! 的本地化字符串。
        /// </summary>
        public static string Log_ExportRivalGoodsInfoSuccess {
            get {
                return ResourceManager.GetString("Log_ExportRivalGoodsInfoSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 成功导出：[{0}] 有销售数！ 的本地化字符串。
        /// </summary>
        public static string Log_ExportSingleRivalGoodsInfoSuccess {
            get {
                return ResourceManager.GetString("Log_ExportSingleRivalGoodsInfoSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 得到产品列表失败 的本地化字符串。
        /// </summary>
        public static string Log_GetGoodsListFailure {
            get {
                return ResourceManager.GetString("Log_GetGoodsListFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 得到卖家仓库中的商品出错 的本地化字符串。
        /// </summary>
        public static string Log_GetInventoryGoodsFailure {
            get {
                return ResourceManager.GetString("Log_GetInventoryGoodsFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 得到在售商品出错 的本地化字符串。
        /// </summary>
        public static string Log_GetOnSaleGoodsFailure {
            get {
                return ResourceManager.GetString("Log_GetOnSaleGoodsFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Query:{0}-&gt;正在获取在售产品... 的本地化字符串。
        /// </summary>
        public static string Log_GetOnSaleGoodsing {
            get {
                return ResourceManager.GetString("Log_GetOnSaleGoodsing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Query:{0}-&gt;获取在售产品成功 的本地化字符串。
        /// </summary>
        public static string Log_GetOnSaleGoodsSuccess {
            get {
                return ResourceManager.GetString("Log_GetOnSaleGoodsSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在获取：[{0}] 的销售数据 的本地化字符串。
        /// </summary>
        public static string Log_GetRivalDetailsing {
            get {
                return ResourceManager.GetString("Log_GetRivalDetailsing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 获取：[{0}] 的销售数据完成 的本地化字符串。
        /// </summary>
        public static string Log_GetRivalDetailsSuccess {
            get {
                return ResourceManager.GetString("Log_GetRivalDetailsSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 获取SKU出错：numIds:{0} 的本地化字符串。
        /// </summary>
        public static string Log_GetSkusFailure {
            get {
                return ResourceManager.GetString("Log_GetSkusFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在获取SKU...：numIds:{0} 的本地化字符串。
        /// </summary>
        public static string Log_GetSkusing {
            get {
                return ResourceManager.GetString("Log_GetSkusing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 获取SKU成功：numIds:{0} 的本地化字符串。
        /// </summary>
        public static string Log_GetSkusSuccess {
            get {
                return ResourceManager.GetString("Log_GetSkusSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 GoodsSn:{0}-&gt;已经存在! 的本地化字符串。
        /// </summary>
        public static string Log_GoodsAlreadyExist {
            get {
                return ResourceManager.GetString("Log_GoodsAlreadyExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 NumlId:{0}-&gt;正在下架... 的本地化字符串。
        /// </summary>
        public static string Log_GoodsDelisting {
            get {
                return ResourceManager.GetString("Log_GoodsDelisting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 NumId:{0} -&gt;产品下架失败 的本地化字符串。
        /// </summary>
        public static string Log_GoodsDelistingFailure {
            get {
                return ResourceManager.GetString("Log_GoodsDelistingFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 NumlId:{0}-&gt;下架成功 的本地化字符串。
        /// </summary>
        public static string Log_GoodsDelistingSuccess {
            get {
                return ResourceManager.GetString("Log_GoodsDelistingSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 插入对手销售数据出错!-&gt;对手名：{0} 的本地化字符串。
        /// </summary>
        public static string Log_InsertRivalGoodsInfoFailure {
            get {
                return ResourceManager.GetString("Log_InsertRivalGoodsInfoFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0}:积分兑换失败! 的本地化字符串。
        /// </summary>
        public static string Log_JfExchangeFailure {
            get {
                return ResourceManager.GetString("Log_JfExchangeFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在兑换积分.... 的本地化字符串。
        /// </summary>
        public static string Log_JfExchangeing {
            get {
                return ResourceManager.GetString("Log_JfExchangeing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0}:积分兑换成功,{1} ! 的本地化字符串。
        /// </summary>
        public static string Log_JfExchangeSuccess {
            get {
                return ResourceManager.GetString("Log_JfExchangeSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：{0} 登录失败 的本地化字符串。
        /// </summary>
        public static string Log_LoginFailure {
            get {
                return ResourceManager.GetString("Log_LoginFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：{0} 正在进行登录.... 的本地化字符串。
        /// </summary>
        public static string Log_Logining {
            get {
                return ResourceManager.GetString("Log_Logining", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户：{0} 登录成功 的本地化字符串。
        /// </summary>
        public static string Log_LoginSuccess {
            get {
                return ResourceManager.GetString("Log_LoginSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 修改产品CSS出错 的本地化字符串。
        /// </summary>
        public static string Log_ModifyGoodsDetailTempFailure {
            get {
                return ResourceManager.GetString("Log_ModifyGoodsDetailTempFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发布产品：{0} 发布失败 的本地化字符串。
        /// </summary>
        public static string Log_PublishGoodsFailure {
            get {
                return ResourceManager.GetString("Log_PublishGoodsFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {0}-&gt;正在发布... 的本地化字符串。
        /// </summary>
        public static string Log_PublishGoodsing {
            get {
                return ResourceManager.GetString("Log_PublishGoodsing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发布产品：{0} 已发布成功。numId:{1} 的本地化字符串。
        /// </summary>
        public static string Log_PublishGoodsSuccess {
            get {
                return ResourceManager.GetString("Log_PublishGoodsSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 上传销售图片：失败。 的本地化字符串。
        /// </summary>
        public static string Log_PublishSaleImgFailure {
            get {
                return ResourceManager.GetString("Log_PublishSaleImgFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 NumlId:{0}-&gt;正在上传销售图片... 的本地化字符串。
        /// </summary>
        public static string Log_PublishSaleImging {
            get {
                return ResourceManager.GetString("Log_PublishSaleImging", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 上传销售图片：成功。numId:{0},图片地址:{1} 的本地化字符串。
        /// </summary>
        public static string Log_PublishSaleImgSuccess {
            get {
                return ResourceManager.GetString("Log_PublishSaleImgSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 签到失败.... 的本地化字符串。
        /// </summary>
        public static string Log_SingInFailure {
            get {
                return ResourceManager.GetString("Log_SingInFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在进行签到... 的本地化字符串。
        /// </summary>
        public static string Log_SingIning {
            get {
                return ResourceManager.GetString("Log_SingIning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 签到成功:{0} ! 的本地化字符串。
        /// </summary>
        public static string Log_SingInSuccess {
            get {
                return ResourceManager.GetString("Log_SingInSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 库存相同，不进行更新;-&gt;NumId:{0};GoodsSn:{1}; 的本地化字符串。
        /// </summary>
        public static string Log_StockEqualNotUpdate {
            get {
                return ResourceManager.GetString("Log_StockEqualNotUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 GoodsSn:{0}-&gt;正在将banggo的产品信息填充到taobao实体类中... 的本地化字符串。
        /// </summary>
        public static string Log_StuffProductInfoing {
            get {
                return ResourceManager.GetString("Log_StuffProductInfoing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 GoodsSn:{0}-&gt;将banggo的产品信息填充到taobao实体类中完成! 的本地化字符串。
        /// </summary>
        public static string Log_StuffProductInfoSuccess {
            get {
                return ResourceManager.GetString("Log_StuffProductInfoSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 对手名：{0} -&gt; 该用户为商城卖家不能获取其销量数据! 的本地化字符串。
        /// </summary>
        public static string Log_UnableGetMallSaleData {
            get {
                return ResourceManager.GetString("Log_UnableGetMallSaleData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新产品：更新失败。numId:{0};GoodsSn:{1} 的本地化字符串。
        /// </summary>
        public static string Log_UpdateGoodsFailure {
            get {
                return ResourceManager.GetString("Log_UpdateGoodsFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 NumIid:{0};GoodsSn:{1}-&gt;正在更新... 的本地化字符串。
        /// </summary>
        public static string Log_UpdateGoodsing {
            get {
                return ResourceManager.GetString("Log_UpdateGoodsing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新产品：{0} 已更新成功。numId:{1} 的本地化字符串。
        /// </summary>
        public static string Log_UpdateGoodsSuccess {
            get {
                return ResourceManager.GetString("Log_UpdateGoodsSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新SKU信息出错-&gt;NumId:{0};Properties:{1};GoodsSn:{1}; 的本地化字符串。
        /// </summary>
        public static string Log_UpdateSkuFailure {
            get {
                return ResourceManager.GetString("Log_UpdateSkuFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 正在更新SKU信息...-&gt;NumId:{0};Properties:{1};GoodsSn:{1}; 的本地化字符串。
        /// </summary>
        public static string Log_UpdateSkuing {
            get {
                return ResourceManager.GetString("Log_UpdateSkuing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 成功更新SKU信息-&gt;NumId:{0};Properties:{1};GoodsSn:{1}; 的本地化字符串。
        /// </summary>
        public static string Log_UpdateSkuSuccess {
            get {
                return ResourceManager.GetString("Log_UpdateSkuSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 /html/body/div[@class=&apos;content&apos;]/div[2]/div[@class=&apos;copy-code&apos;]/input 的本地化字符串。
        /// </summary>
        public static string SysConfig_AuthorizedCodeXPath {
            get {
                return ResourceManager.GetString("SysConfig_AuthorizedCodeXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 http://open.taobao.com/authorize/?appkey={0} 的本地化字符串。
        /// </summary>
        public static string SysConfig_AuthorizeUrl {
            get {
                return ResourceManager.GetString("SysConfig_AuthorizeUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 read_colorlist 的本地化字符串。
        /// </summary>
        public static string SysConfig_ColorListId {
            get {
                return ResourceManager.GetString("SysConfig_ColorListId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 2013运费模板 的本地化字符串。
        /// </summary>
        public static string SysConfig_DeliveryTemplateName {
            get {
                return ResourceManager.GetString("SysConfig_DeliveryTemplateName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 25 的本地化字符串。
        /// </summary>
        public static string SysConfig_EmsFee {
            get {
                return ResourceManager.GetString("SysConfig_EmsFee", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 12 的本地化字符串。
        /// </summary>
        public static string SysConfig_ExpressFee {
            get {
                return ResourceManager.GetString("SysConfig_ExpressFee", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 /html/body/div/div[@class=&apos;details_dir&apos;]/a[2] 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetBanggoProductBrandCodeXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetBanggoProductBrandCodeXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 /html/body/div/div[@class=&apos;details_dir&apos;]/a[5] 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetBanggoProductCatalogXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetBanggoProductCatalogXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 /html/body/div/div[@class=&apos;details_dir&apos;]/a[3] 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetBanggoProductCategoryXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetBanggoProductCategoryXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 /html/body/div/div[@class=&apos;details_dir&apos;]/a[4] 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetBanggoProductParentCatalogXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetBanggoProductParentCatalogXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Api 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetDataByApi {
            get {
                return ResourceManager.GetString("SysConfig_GetDataByApi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Local 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetDataByLocal {
            get {
                return ResourceManager.GetString("SysConfig_GetDataByLocal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Api 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetDataWay {
            get {
                return ResourceManager.GetString("SysConfig_GetDataWay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 //*[@id=&apos;goods_model&apos;]//img[@src=&apos;http://s.mb-go.com/pub7/style/images/grey.gif&apos;] 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetGoodsModeImgGreyXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetGoodsModeImgGreyXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 http://act.banggo.com/Price/getGoodsPrice?r={0}&amp;callback=&amp;goods_sn={1} 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetGoodsPriceByBanggoUrl {
            get {
                return ResourceManager.GetString("SysConfig_GetGoodsPriceByBanggoUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 \d{6} 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetGoodsSnByUrlRegex {
            get {
                return ResourceManager.GetString("SysConfig_GetGoodsSnByUrlRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 market_price 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetMarketPriceId {
            get {
                return ResourceManager.GetString("SysConfig_GetMarketPriceId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 div[@class=&apos;goods_price&apos;]/del 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetMarketPriceXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetMarketPriceXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 http://act.banggo.com/Ajax/cartAjax?time={0}&amp;ajaxtype=color_size&amp;type=color&amp;code={1}&amp;r_code=&amp;goods_sn={2} 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetProductByBanggoAvailableColorUrl {
            get {
                return ResourceManager.GetString("SysConfig_GetProductByBanggoAvailableColorUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 http://act.banggo.com/Ajax/cartAjax?time={0}&amp;ajaxtype=color_size&amp;type=size&amp;code={1}&amp;r_code={2}&amp;goods_sn={3} 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetProductByBanggoAvailableSizeUrl {
            get {
                return ResourceManager.GetString("SysConfig_GetProductByBanggoAvailableSizeUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 //*[@id=&apos;productinfo_div&apos;]//img[@src=&apos;http://s.mb-go.com/pub7/style/images/grey.gif&apos;] 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetProductInfoImgGreyXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetProductInfoImgGreyXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 sale_price 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetSalePriceId {
            get {
                return ResourceManager.GetString("SysConfig_GetSalePriceId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 div[@class=&apos;sales&apos;]/p/strong[@class=&apos;red&apos;]/a 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetSalesVolumeXPath {
            get {
                return ResourceManager.GetString("SysConfig_GetSalesVolumeXPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 svip_price 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetSvipPriceId {
            get {
                return ResourceManager.GetString("SysConfig_GetSvipPriceId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 vip_price 的本地化字符串。
        /// </summary>
        public static string SysConfig_GetVipPriceId {
            get {
                return ResourceManager.GetString("SysConfig_GetVipPriceId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 goods_model 的本地化字符串。
        /// </summary>
        public static string SysConfig_GoodsDescId {
            get {
                return ResourceManager.GetString("SysConfig_GoodsDescId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 0.2 的本地化字符串。
        /// </summary>
        public static string SysConfig_ItemWeight {
            get {
                return ResourceManager.GetString("SysConfig_ItemWeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 2000:4146697; 的本地化字符串。
        /// </summary>
        public static string SysConfig_ME_CITY_BrandProp {
            get {
                return ResourceManager.GetString("SysConfig_ME-CITY_BrandProp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 2000:29504; 的本地化字符串。
        /// </summary>
        public static string SysConfig_METERSBONWE_BrandProp {
            get {
                return ResourceManager.GetString("SysConfig_METERSBONWE_BrandProp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 12 的本地化字符串。
        /// </summary>
        public static string SysConfig_PostFee {
            get {
                return ResourceManager.GetString("SysConfig_PostFee", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 1632501 的本地化字符串。
        /// </summary>
        public static string SysConfig_ProductCodeProp {
            get {
                return ResourceManager.GetString("SysConfig_ProductCodeProp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 productinfo_div 的本地化字符串。
        /// </summary>
        public static string SysConfig_ProductInfoId {
            get {
                return ResourceManager.GetString("SysConfig_ProductInfoId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 http://gw.api.taobao.com/router/rest 的本地化字符串。
        /// </summary>
        public static string SysConfig_RealTaobaoServerUrl {
            get {
                return ResourceManager.GetString("SysConfig_RealTaobaoServerUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 read_sizelist 的本地化字符串。
        /// </summary>
        public static string SysConfig_SizeListId {
            get {
                return ResourceManager.GetString("SysConfig_SizeListId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Sku 的本地化字符串。
        /// </summary>
        public static string SysConfig_Sku {
            get {
                return ResourceManager.GetString("SysConfig_Sku", resourceCulture);
            }
        }
    }
}

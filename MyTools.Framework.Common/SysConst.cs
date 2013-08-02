/*
 *名称：SysConst
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-10 10:54:48
 *修改时间：
 *备注：
 */

using System.Collections.Generic;
using System.Configuration;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Utility.CommomHelper;

namespace MyTools.Framework.Common
{
    public class SysConst
    {
        #region Config

        public static readonly string AppLoginUser = ConfigurationManager.AppSettings["AppLoginUser"];
        public static readonly string AppKey = ConfigurationManager.AppSettings["appKey"];
        public static readonly string AppSecret = ConfigurationManager.AppSettings["appSecret"];
        public static readonly string PostageId = ConfigurationManager.AppSettings["PostageId"]; 
        public static readonly double DiscountRatio = ConfigurationManager.AppSettings["DiscountRatio"].ToDouble();
        public static readonly string PrefixTitle = ConfigurationManager.AppSettings["PrefixTitle"];
        /// <summary>
        /// 要替换的产品的原标题部份名字->在更新产品时使用
        /// </summary>
        public static readonly string OriginalTitle = ConfigurationManager.AppSettings["OriginalTitle"];
        /// <summary>
        /// 替换后的新标题->在更新产品时使用
        /// </summary>
        public static readonly string NewTitle = ConfigurationManager.AppSettings["NewTitle"];

        public static readonly string LocationState = ConfigurationManager.AppSettings["LocationState"];
        public static readonly string LocationCity = ConfigurationManager.AppSettings["LocationCity"];

        public static readonly string PrefixGoodsDesc = ConfigurationManager.AppSettings["PrefixGoodsDesc"];

        public static readonly string TaoBaoSearchUrl = ConfigurationManager.AppSettings["TaoBaoSearchUrl"];
        /// <summary>
        /// 买该产品的成本折扣是多少，如打折卷是5折的
        /// </summary>
        public static readonly double CostRatio = ConfigurationManager.AppSettings["CostRatio"].ToDouble(); //成果折扣
        /// <summary>
        /// 额外的成果价，如，购买打折卷的钱，以及 电话、车费、请人吃饭等。
        /// </summary>
        public static readonly double CostExtraPrice = ConfigurationManager.AppSettings["CostExtraPrice"].ToDouble();
         /// <summary>
        /// 强制更新商品，目前主要针对价格和库存
        /// </summary>
        public static readonly bool IsEnforceUpdate = ConfigurationManager.AppSettings["IsEnforceUpdate"].ToBoolean();

        /// <summary>
        /// 邦购积分兑换相应产品，如多少红包，多少邦购币
        /// </summary>
        public static readonly string BanggoJfGoods = ConfigurationManager.AppSettings["BanggoJfGoods"];

        /// <summary>
        /// 读取用于修改淘宝上产品详情的样式
        /// </summary>
        public static readonly string GoodsDetailTemplate = FileHelper.ReadFileContent("GoodsDetailTemplate.css");
         
        #endregion

        public static readonly Dictionary<string, string> CategoryBanggoToTaobaoMap = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> ChildCatalogBanggoToTaobaoCid = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> ManCatalogBanggoToTaobaoCid = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> WomenCatalogBanggoToTaobaoCid = new Dictionary<string, string>();

        static SysConst()
        {
            CategoryBanggoToTaobaoMap.Add("男童", "童装");
            CategoryBanggoToTaobaoMap.Add("女童", "童装");
            CategoryBanggoToTaobaoMap.Add("ME-CITY", "ME&CITY");

            ChildCatalogBanggoToTaobaoCid.Add("休闲裤", "50006217"); //对应的是其它
            ChildCatalogBanggoToTaobaoCid.Add("牛仔裤", "50006217"); //对应的是其它
            ChildCatalogBanggoToTaobaoCid.Add("针织裤", "50006217"); //对应的是其它
            ChildCatalogBanggoToTaobaoCid.Add("裤类", "50006217"); //对应的是其它 
            ChildCatalogBanggoToTaobaoCid.Add("茄克", "50010519"); //夹克\/皮衣 
            ChildCatalogBanggoToTaobaoCid.Add("大衣", "50010520"); //呢大衣
            ChildCatalogBanggoToTaobaoCid.Add("开衫", "50010539"); //毛衣\/针织衫 
            ChildCatalogBanggoToTaobaoCid.Add("裙类", "50013693"); //裙子
            ChildCatalogBanggoToTaobaoCid.Add("配件", "50014503");//挎包\/拎包\/休闲包


            WomenCatalogBanggoToTaobaoCid.Add("裙类", "50010850"); //连衣裙
            WomenCatalogBanggoToTaobaoCid.Add("裤类", "162205"); //牛仔裤
            WomenCatalogBanggoToTaobaoCid.Add("配件", "162404");//挎包\/拎包\/休闲包
            WomenCatalogBanggoToTaobaoCid.Add("鞋", "162404");//挎包\/拎包\/休闲包
            WomenCatalogBanggoToTaobaoCid.Add("鞋类", "162404");//挎包\/拎包\/休闲包
            WomenCatalogBanggoToTaobaoCid.Add("外套", "162404");//挎包\/拎包\/休闲包


            ManCatalogBanggoToTaobaoCid.Add("裤类", "50010167");//牛仔裤
            ManCatalogBanggoToTaobaoCid.Add("西装", "50011130");//西服套装
            ManCatalogBanggoToTaobaoCid.Add("茄克", "50010158");//夹克
            ManCatalogBanggoToTaobaoCid.Add("配件", "50005867");//工装制服
            ManCatalogBanggoToTaobaoCid.Add("鞋", "50005867");//工装制服
            ManCatalogBanggoToTaobaoCid.Add("鞋类", "50005867");//工装制服
            ManCatalogBanggoToTaobaoCid.Add("外套", "50005867");//工装制服





        }

    }
}
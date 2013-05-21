using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// DistributorArchive Data Structure.
    /// </summary>
    [Serializable]
    public class DistributorArchive : TopObject
    {
        /// <summary>
        /// 供应商授权分销商的产品的下载率。  率的值都是*10000后的，取值后直接除以100后加上%即可。比如12.33%，返回值是1233。
        /// </summary>
        [XmlElement("down_load_ratio")]
        public string DownLoadRatio { get; set; }

        /// <summary>
        /// 供应商在分销商店铺中的成交（已付款）订单笔数占比。  率的值都是*10000后的，取值后直接除以100后加上%即可。比如12.33%，返回值是1233。
        /// </summary>
        [XmlElement("order_shop_ratio")]
        public string OrderShopRatio { get; set; }

        /// <summary>
        /// 供应商授权分销商的产品的上架率。  率的值都是*10000后的，取值后直接除以100后加上%即可。比如12.33%，返回值是1233。
        /// </summary>
        [XmlElement("up_self_ratio")]
        public string UpSelfRatio { get; set; }

        /// <summary>
        /// 供应商在分销商店铺中的上架商品占比。  率的值都是*10000后的，取值后直接除以100后加上%即可。比如12.33%，返回值是1233。
        /// </summary>
        [XmlElement("up_shop_ratio")]
        public string UpShopRatio { get; set; }

        /// <summary>
        /// 供应商在分销商店铺中铺货商品UV占店铺商品总UV的比。  率的值都是*10000后的，取值后直接除以100后加上%即可。比如12.33%，返回值是1233。
        /// </summary>
        [XmlElement("uv_shop_ratio")]
        public string UvShopRatio { get; set; }
    }
}

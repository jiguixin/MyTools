using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// TravelItem Data Structure.
    /// </summary>
    [Serializable]
    public class TravelItem : TopObject
    {
        /// <summary>
        /// 商品发布后的状态。onsale出售中，instock库中。
        /// </summary>
        [XmlElement("approve_status")]
        public string ApproveStatus { get; set; }

        /// <summary>
        /// 商城返点比例，为5的倍数，最低0.5%。
        /// </summary>
        [XmlElement("auction_point")]
        public long AuctionPoint { get; set; }

        /// <summary>
        /// 商品所属类目ID。发布旅游线路商品有两个值：1(国内线路类目ID)，2(国际线路类目ID)。
        /// </summary>
        [XmlElement("cid")]
        public long Cid { get; set; }

        /// <summary>
        /// 商品发布时间（格式：yyyy-MM-dd HH:mm:ss）。
        /// </summary>
        [XmlElement("created")]
        public string Created { get; set; }

        /// <summary>
        /// 下架时间（格式：yyyy-MM-dd HH:mm:ss）。
        /// </summary>
        [XmlElement("delist_time")]
        public string DelistTime { get; set; }

        /// <summary>
        /// 商品描述，字数要大于5个字符，小于50000个字符。
        /// </summary>
        [XmlElement("desc")]
        public string Desc { get; set; }

        /// <summary>
        /// 商品详情url
        /// </summary>
        [XmlElement("detail_url")]
        public string DetailUrl { get; set; }

        /// <summary>
        /// 最晚成团提前天数，最小0天，最大为180天。
        /// </summary>
        [XmlElement("duration")]
        public long Duration { get; set; }

        /// <summary>
        /// 费用包含，不超过1500个字符。
        /// </summary>
        [XmlElement("fee_include")]
        public string FeeInclude { get; set; }

        /// <summary>
        /// 费用不包，不超过1500个字符。
        /// </summary>
        [XmlElement("fee_not_include")]
        public string FeeNotInclude { get; set; }

        /// <summary>
        /// 运费承担方式，seller（卖家承担），buyer（买家承担）。
        /// </summary>
        [XmlElement("freight_payer")]
        public string FreightPayer { get; set; }

        /// <summary>
        /// 会员打折（是：true，否：false）。
        /// </summary>
        [XmlElement("has_discount")]
        public bool HasDiscount { get; set; }

        /// <summary>
        /// 是否有发票（是：true，否：false）。
        /// </summary>
        [XmlElement("has_invoice")]
        public bool HasInvoice { get; set; }

        /// <summary>
        /// 是否有橱窗（是：true，否：false）。
        /// </summary>
        [XmlElement("has_showcase")]
        public bool HasShowcase { get; set; }

        /// <summary>
        /// 非分销商品：0，代销：1，经销：2。
        /// </summary>
        [XmlElement("is_fenxiao")]
        public long IsFenxiao { get; set; }

        /// <summary>
        /// 是否为铁定出游商品的参数设置为true，那么该商品为铁定出游商品；设置为false，那么该商品为非铁定出游商品。默认为false
        /// </summary>
        [XmlElement("is_tdcy")]
        public bool IsTdcy { get; set; }

        /// <summary>
        /// 是否定时上架商品。
        /// </summary>
        [XmlElement("is_timing")]
        public bool IsTiming { get; set; }

        /// <summary>
        /// 商品数字ID
        /// </summary>
        [XmlElement("item_id")]
        public long ItemId { get; set; }

        /// <summary>
        /// 商品图片列表(包括主图)。最多5个
        /// </summary>
        [XmlArray("item_imgs")]
        [XmlArrayItem("travel_item_img")]
        public List<TravelItemImg> ItemImgs { get; set; }

        /// <summary>
        /// 上架时间（格式：yyyy-MM-dd HH:mm:ss）。
        /// </summary>
        [XmlElement("list_time")]
        public string ListTime { get; set; }

        /// <summary>
        /// 宝贝所在地，格式为（省份:城市）。
        /// </summary>
        [XmlElement("location")]
        public string Location { get; set; }

        /// <summary>
        /// 允许最多橱窗数。
        /// </summary>
        [XmlElement("max_showcase")]
        public long MaxShowcase { get; set; }

        /// <summary>
        /// 商品修改时间（格式：yyyy-MM-dd HH:mm:ss）。
        /// </summary>
        [XmlElement("modified")]
        public string Modified { get; set; }

        /// <summary>
        /// 卖家昵称。
        /// </summary>
        [XmlElement("nick")]
        public string Nick { get; set; }

        /// <summary>
        /// 商品数量。
        /// </summary>
        [XmlElement("num")]
        public long Num { get; set; }

        /// <summary>
        /// 预定须知，不超过1500个字符。
        /// </summary>
        [XmlElement("order_info")]
        public string OrderInfo { get; set; }

        /// <summary>
        /// 商家编码。
        /// </summary>
        [XmlElement("outer_id")]
        public string OuterId { get; set; }

        /// <summary>
        /// 商品主图片地址。
        /// </summary>
        [XmlElement("pic_url")]
        public string PicUrl { get; set; }

        /// <summary>
        /// 商品价格，格式：500；单位：分；精确到：分。表示：5.00元
        /// </summary>
        [XmlElement("price")]
        public long Price { get; set; }

        /// <summary>
        /// Json串，价格日历信息（针对旅游度假线路的销售属性），定义了2012年6月8号成人价，儿童价，单房差均为10元，库存为100的日历价格。price_calendar属性中一个{}中表示1天的价格日历信息，可以最多输入90天的价格日历，最小和最大日期不能跨度超过90天。其中，"man_num"：成人名额；  "man_price"：成人价格，以分为单位；"child_num"：儿童名额；"child_price"：儿童价格，以分为单位；"diff_price"：单人房差，以分为单位。
        /// </summary>
        [XmlElement("price_calendar")]
        public string PriceCalendar { get; set; }

        /// <summary>
        /// 商品属性列表。格式为：pid:vid;pid:vid。例如发布度假线路商品，那么这里就需要填写：出发地属性id:城市id;目的地市属性id:目的地市id;……等等。（注意：不会包含SKU属性）。
        /// </summary>
        [XmlElement("props")]
        public string Props { get; set; }

        /// <summary>
        /// 商品属性名称。标识着props内容里面的pid和vid所对应的名称。格式为：pid1:vid1:pid_name1:vid_name1;pid2:vid2:pid_name2:vid_name2……(注：属性名称中的冒号":"被转换为："#cln#"; 分号";"被转换为："#scln#" )。
        /// </summary>
        [XmlElement("props_name")]
        public string PropsName { get; set; }

        /// <summary>
        /// 退改规定，不超过1500个字符。
        /// </summary>
        [XmlElement("refund_regulation")]
        public string RefundRegulation { get; set; }

        /// <summary>
        /// 秒杀商品类型。打上秒杀标记的商品，用户只能下架并不能再上架，其他任何编辑或删除操作都不能进行。如果用户想取消秒杀标记，需要联系小二进行操作。如果秒杀结束需要自由编辑请联系活动负责人（小二）去掉秒杀标记。可选类型 web_only(只能通过web网络秒杀) wap_only(只能通过wap网络秒杀) web_and_wap(既能通过web秒杀也能通过wap秒杀)。
        /// </summary>
        [XmlElement("second_kill")]
        public string SecondKill { get; set; }

        /// <summary>
        /// 商品所属的卖家店铺分类的Id，一件商品可以由卖家放在店铺内部的多个商品类目下，多个以“,”分割。
        /// </summary>
        [XmlElement("seller_cids")]
        public string SellerCids { get; set; }

        /// <summary>
        /// 标准商品Sku列表。
        /// </summary>
        [XmlArray("skus")]
        [XmlArrayItem("travel_item_sku")]
        public List<TravelItemSku> Skus { get; set; }

        /// <summary>
        /// 商品开始出售时间（格式：yyyy-MM-dd HH:mm:ss）。
        /// </summary>
        [XmlElement("start")]
        public string Start { get; set; }

        /// <summary>
        /// 商品是否支持拍下减库存:1支持;2取消支持(付款减库存);0(默认)不更改，集市卖家默认拍下减库存;商城卖家默认付款减库存。
        /// </summary>
        [XmlElement("sub_stock")]
        public long SubStock { get; set; }

        /// <summary>
        /// 商品标题。
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 商品关联玩法结构。
        /// </summary>
        [XmlElement("travel_play")]
        public TravelPlay TravelPlay { get; set; }

        /// <summary>
        /// 商品类型(fixed:一口价;auction:拍卖)注：取消团购。
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; }

        /// <summary>
        /// 已被使用的橱窗数。
        /// </summary>
        [XmlElement("used_showcase")]
        public long UsedShowcase { get; set; }

        /// <summary>
        /// 商品是否违规，违规：true，不违规：false。
        /// </summary>
        [XmlElement("violation")]
        public bool Violation { get; set; }

        /// <summary>
        /// 不带html标签的desc文本信息，该字段只在taobao.item.get接口中返回。
        /// </summary>
        [XmlElement("wap_desc")]
        public string WapDesc { get; set; }

        /// <summary>
        /// 适合wap应用的商品详情url。
        /// </summary>
        [XmlElement("wap_detail_url")]
        public string WapDetailUrl { get; set; }
    }
}

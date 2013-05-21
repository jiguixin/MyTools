using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.travel.item.add
    /// </summary>
    public class TravelItemAddRequest : ITopUploadRequest<TravelItemAddResponse>
    {
        /// <summary>
        /// 商品上传后的状态。可选值:onsale(出售中),instock(仓库中);默认值:onsale。
        /// </summary>
        public string ApproveStatus { get; set; }

        /// <summary>
        /// 商品的积分返点比例。如:5,表示:返点比例0.5%. 注意：返点比例必须是>0的整数.B商家在发布非虚拟商品时，这个字段必须设置，返点必须是 5的倍数，即0.5%的倍数。注意该字段值最高值不超过500，即50%。
        /// </summary>
        public Nullable<long> AuctionPoint { get; set; }

        /// <summary>
        /// 商品所属类目ID。发布旅游线路商品有两个值：1(国内线路类目ID)，2(国际线路类目ID)。
        /// </summary>
        public Nullable<long> Cid { get; set; }

        /// <summary>
        /// 宝贝所在地城市。如杭州 。可以通过http://dl.open.taobao.com/sdk/商品城市列表.rar查询 ，该字段为必填字段
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 商品描述，不超过50000个字符。
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 最晚成团提前天数，最小0天，最大为180天。
        /// </summary>
        public Nullable<long> Duration { get; set; }

        /// <summary>
        /// 费用包含，不超过1500个字符。
        /// </summary>
        public string FeeInclude { get; set; }

        /// <summary>
        /// 费用不包，不超过1500个字符。
        /// </summary>
        public string FeeNotInclude { get; set; }

        /// <summary>
        /// 支持会员打折。可选值:true,false;默认值:false(不打折)。
        /// </summary>
        public Nullable<bool> HasDiscount { get; set; }

        /// <summary>
        /// 是否有发票。可选值:true,false (商城卖家此字段必须为true);默认值:false(无发票)。
        /// </summary>
        public Nullable<bool> HasInvoice { get; set; }

        /// <summary>
        /// 橱窗推荐。可选值:true,false;默认值:false(不推荐)，B商家不用设置该字段，均为true。
        /// </summary>
        public Nullable<bool> HasShowcase { get; set; }

        /// <summary>
        /// 商品主图。类型:JPG,GIF;最大长度:500k，支持的文件类型：gif,jpg,jpeg,png。
        /// </summary>
        public FileItem Image { get; set; }

        /// <summary>
        /// 是否为铁定出游商品的参数  设置为true，那么该商品为铁定出游商品；  设置为false，那么该商品为非铁定出游商品。  默认为false
        /// </summary>
        public Nullable<bool> IsTdcy { get; set; }

        /// <summary>
        /// 定时上架时间。(时间格式：yyyy-MM-dd HH:mm:ss)。
        /// </summary>
        public Nullable<DateTime> ListTime { get; set; }

        /// <summary>
        /// 商品库存。如果发布旅游度假线路宝贝，该字段可以忽略。
        /// </summary>
        public Nullable<long> Num { get; set; }

        /// <summary>
        /// 预定须知，不超过1500个字符。
        /// </summary>
        public string OrderInfo { get; set; }

        /// <summary>
        /// 商家的外部编码，最大为512字节。
        /// </summary>
        public string OuterId { get; set; }

        /// <summary>
        /// 商品主图需要关联的图片空间的相对url。这个url所对应的图片必须要属于当前用户。pic_path和image只需要传入一个,如果两个都传，默认选择pic_path。
        /// </summary>
        public string PicPath { get; set; }

        /// <summary>
        /// 玩法描述，已经废弃，不用填写。
        /// </summary>
        public string PlayDesc { get; set; }

        /// <summary>
        /// 线路玩法id，已经废弃，不用填写。
        /// </summary>
        public Nullable<long> PlayId { get; set; }

        /// <summary>
        /// 商品一口价,以“分”为单位。如果发布旅游度假线路的宝贝，该字段可以忽略。
        /// </summary>
        public Nullable<long> Price { get; set; }

        /// <summary>
        /// Json串，价格日历信息（针对旅游度假线路的销售属性），定义了2012年6月8号成人价，儿童价，单房差均为10元，库存为100的日历价格。price_calendar属性中一个{}中表示1天的价格日历信息，可以最多输入90天的价格日历，最小和最大日期不能跨度超过90天。其中，"man_num"：成人名额； "man_price"：成人价格，以分为单位；"child_num"：儿童名额；"child_price"：儿童价格，以分为单位；"diff_price"：单人房差，以分为单位。
        /// </summary>
        public string PriceCalendar { get; set; }

        /// <summary>
        /// 商品属性列表。格式为：pid:vid;pid:vid。例如发布度假线路商品，那么这里就需要填写：出发地属性id:城市id;目的地市属性id:目的地市id;……等等。
        /// </summary>
        public string Props { get; set; }

        /// <summary>
        /// 宝贝所在地省份。如浙江，具体可以下载http://dl.open.taobao.com/sdk/商品城市列表.rar 取到，该字段为必填字段
        /// </summary>
        public string Prov { get; set; }

        /// <summary>
        /// 退改规定，不超过1500个字符。
        /// </summary>
        public string RefundRegulation { get; set; }

        /// <summary>
        /// 商品秒杀三个值：可选类型web_only(只能通过web网络秒杀)，wap_only(只能通过wap网络秒杀)，web_and_wap(既能通过web秒杀也能通过wap秒杀)。
        /// </summary>
        public string SecondKill { get; set; }

        /// <summary>
        /// 关联商品与店铺类目，结构:",cid1,cid2,...,"，如果店铺类目存在二级类目，必须传入子类目cids。
        /// </summary>
        public string SellerCids { get; set; }

        /// <summary>
        /// sku销售属性串对应的价格，精确到分，每一个属性串都会对应一个价格，单位为分。sku_prices的数组大小应该和sku_properties的数组大小一致。如果发布线路的商品，该字段可以忽略。
        /// </summary>
        public string SkuPrices { get; set; }

        /// <summary>
        /// sku销售属性串，调用taobao.travel.itemprops.get接口获取商品销售属性code，然后拼接成pid:vid;pid:vid格式。如果发布线路的商品，该字段可以忽略。
        /// </summary>
        public string SkuProperties { get; set; }

        /// <summary>
        /// sku销售属性串对应的库存，每一个属性串都会对应一个库存，显然sku_quantities的数组大小应该和sku_properties的数组大小一致。如果发布线路的商品，该字段可以忽略。
        /// </summary>
        public string SkuQuantities { get; set; }

        /// <summary>
        /// 商品是否支持拍下减库存:1支持;2取消支持(付款减库存);0(默认)不更改，集市卖家默认拍下减库存;商城卖家默认付款减库存。
        /// </summary>
        public Nullable<long> SubStock { get; set; }

        /// <summary>
        /// 商品标题。不能超过60个字节或者30个汉字
        /// </summary>
        public string Title { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.travel.item.add";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("approve_status", this.ApproveStatus);
            parameters.Add("auction_point", this.AuctionPoint);
            parameters.Add("cid", this.Cid);
            parameters.Add("city", this.City);
            parameters.Add("desc", this.Desc);
            parameters.Add("duration", this.Duration);
            parameters.Add("fee_include", this.FeeInclude);
            parameters.Add("fee_not_include", this.FeeNotInclude);
            parameters.Add("has_discount", this.HasDiscount);
            parameters.Add("has_invoice", this.HasInvoice);
            parameters.Add("has_showcase", this.HasShowcase);
            parameters.Add("is_tdcy", this.IsTdcy);
            parameters.Add("list_time", this.ListTime);
            parameters.Add("num", this.Num);
            parameters.Add("order_info", this.OrderInfo);
            parameters.Add("outer_id", this.OuterId);
            parameters.Add("pic_path", this.PicPath);
            parameters.Add("play_desc", this.PlayDesc);
            parameters.Add("play_id", this.PlayId);
            parameters.Add("price", this.Price);
            parameters.Add("price_calendar", this.PriceCalendar);
            parameters.Add("props", this.Props);
            parameters.Add("prov", this.Prov);
            parameters.Add("refund_regulation", this.RefundRegulation);
            parameters.Add("second_kill", this.SecondKill);
            parameters.Add("seller_cids", this.SellerCids);
            parameters.Add("sku_prices", this.SkuPrices);
            parameters.Add("sku_properties", this.SkuProperties);
            parameters.Add("sku_quantities", this.SkuQuantities);
            parameters.Add("sub_stock", this.SubStock);
            parameters.Add("title", this.Title);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateMaxValue("auction_point", this.AuctionPoint, 500);
            RequestValidator.ValidateMinValue("auction_point", this.AuctionPoint, 0);
            RequestValidator.ValidateRequired("cid", this.Cid);
            RequestValidator.ValidateRequired("desc", this.Desc);
            RequestValidator.ValidateMaxLength("desc", this.Desc, 50000);
            RequestValidator.ValidateRequired("duration", this.Duration);
            RequestValidator.ValidateMaxValue("duration", this.Duration, 180);
            RequestValidator.ValidateMinValue("duration", this.Duration, 0);
            RequestValidator.ValidateRequired("fee_include", this.FeeInclude);
            RequestValidator.ValidateMaxLength("fee_include", this.FeeInclude, 1500);
            RequestValidator.ValidateRequired("fee_not_include", this.FeeNotInclude);
            RequestValidator.ValidateMaxLength("fee_not_include", this.FeeNotInclude, 1500);
            RequestValidator.ValidateRequired("order_info", this.OrderInfo);
            RequestValidator.ValidateMaxLength("order_info", this.OrderInfo, 1500);
            RequestValidator.ValidateMaxLength("outer_id", this.OuterId, 512);
            RequestValidator.ValidateMaxLength("play_desc", this.PlayDesc, 1500);
            RequestValidator.ValidateRequired("price_calendar", this.PriceCalendar);
            RequestValidator.ValidateRequired("props", this.Props);
            RequestValidator.ValidateRequired("refund_regulation", this.RefundRegulation);
            RequestValidator.ValidateMaxLength("refund_regulation", this.RefundRegulation, 1500);
            RequestValidator.ValidateMaxListSize("seller_cids", this.SellerCids, 20);
            RequestValidator.ValidateMaxLength("seller_cids", this.SellerCids, 256);
            RequestValidator.ValidateMaxListSize("sku_prices", this.SkuPrices, 380);
            RequestValidator.ValidateMaxListSize("sku_properties", this.SkuProperties, 380);
            RequestValidator.ValidateMaxListSize("sku_quantities", this.SkuQuantities, 380);
            RequestValidator.ValidateMaxValue("sub_stock", this.SubStock, 2);
            RequestValidator.ValidateMinValue("sub_stock", this.SubStock, 0);
            RequestValidator.ValidateRequired("title", this.Title);
            RequestValidator.ValidateMaxLength("title", this.Title, 60);
        }

        #endregion

        #region ITopUploadRequest Members

        public IDictionary<string, FileItem> GetFileParameters()
        {
            IDictionary<string, FileItem> parameters = new Dictionary<string, FileItem>();
            parameters.Add("image", this.Image);
            return parameters;
        }

        #endregion

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new TopDictionary();
            }
            this.otherParameters.Add(key, value);
        }
    }
}

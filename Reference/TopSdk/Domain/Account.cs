using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// Account Data Structure.
    /// </summary>
    [Serializable]
    public class Account : TopObject
    {
        /// <summary>
        /// 费用科目编码
        /// </summary>
        [XmlElement("account_code")]
        public string AccountCode { get; set; }

        /// <summary>
        /// 费用科目编号
        /// </summary>
        [XmlElement("account_id")]
        public long AccountId { get; set; }

        /// <summary>
        /// 费用科目名称
        /// </summary>
        [XmlElement("account_name")]
        public string AccountName { get; set; }

        /// <summary>
        /// 费用科目类型:1-虚拟账户 2-交易 3-交易佣金 4-服务费  5-天猫积分 6-其他
        /// </summary>
        [XmlElement("account_type")]
        public long AccountType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [XmlElement("gmt_create")]
        public string GmtCreate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [XmlElement("gmt_modified")]
        public string GmtModified { get; set; }

        /// <summary>
        /// 是否订单相关:0-订单无关 1-订单相关
        /// </summary>
        [XmlElement("related_order")]
        public long RelatedOrder { get; set; }

        /// <summary>
        /// 状态:0-不可用 1-可用
        /// </summary>
        [XmlElement("status")]
        public long Status { get; set; }
    }
}

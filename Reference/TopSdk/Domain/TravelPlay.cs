using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// TravelPlay Data Structure.
    /// </summary>
    [Serializable]
    public class TravelPlay : TopObject
    {
        /// <summary>
        /// 商品所属类目ID。发布旅游线路商品有两个值：1(国内线路类目ID)，2(国际线路类目ID)
        /// </summary>
        [XmlElement("cid")]
        public long Cid { get; set; }

        /// <summary>
        /// 玩法描述
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// 是否通过审核，true通过，false未通过
        /// </summary>
        [XmlElement("is_auth")]
        public bool IsAuth { get; set; }

        /// <summary>
        /// 线路玩法ID
        /// </summary>
        [XmlElement("play_id")]
        public long PlayId { get; set; }

        /// <summary>
        /// 线路玩法名称
        /// </summary>
        [XmlElement("play_name")]
        public string PlayName { get; set; }

        /// <summary>
        /// 玩法类型，1跟团游，2自由行
        /// </summary>
        [XmlElement("play_type")]
        public long PlayType { get; set; }
    }
}

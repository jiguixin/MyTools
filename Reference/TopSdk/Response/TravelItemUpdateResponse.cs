using System;
using System.Xml.Serialization;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// TravelItemUpdateResponse.
    /// </summary>
    public class TravelItemUpdateResponse : TopResponse
    {
        /// <summary>
        /// 更新成功后的数据结构
        /// </summary>
        [XmlElement("travel_item")]
        public TravelItem TravelItem { get; set; }
    }
}

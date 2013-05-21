using System;
using System.Xml.Serialization;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// TravelItemGetResponse.
    /// </summary>
    public class TravelItemGetResponse : TopResponse
    {
        /// <summary>
        /// 旅游商品结构
        /// </summary>
        [XmlElement("travel_item")]
        public TravelItem TravelItem { get; set; }
    }
}

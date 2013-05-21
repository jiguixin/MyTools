using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// TravelItemareaGetResponse.
    /// </summary>
    public class TravelItemareaGetResponse : TopResponse
    {
        /// <summary>
        /// 旅游度假商品地区结构列表。
        /// </summary>
        [XmlArray("travel_area_nodes")]
        [XmlArrayItem("travel_area_node")]
        public List<TravelAreaNode> TravelAreaNodes { get; set; }
    }
}

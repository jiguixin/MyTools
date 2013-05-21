using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// TravelItempropsGetResponse.
    /// </summary>
    public class TravelItempropsGetResponse : TopResponse
    {
        /// <summary>
        /// 旅游商品类目属性结构列表。
        /// </summary>
        [XmlArray("travel_item_props")]
        [XmlArrayItem("travel_item_prop")]
        public List<TravelItemProp> TravelItemProps { get; set; }
    }
}

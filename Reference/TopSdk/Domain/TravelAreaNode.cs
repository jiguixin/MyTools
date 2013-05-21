using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// TravelAreaNode Data Structure.
    /// </summary>
    [Serializable]
    public class TravelAreaNode : TopObject
    {
        /// <summary>
        /// 地区属性值。
        /// </summary>
        [XmlElement("travel_prop_value")]
        public TravelPropValue TravelPropValue { get; set; }

        /// <summary>
        /// 该地区下所有下级地区集合(目前地区只有两级，国内：省-市；国际：大洲-国家)。
        /// </summary>
        [XmlArray("travel_prop_values")]
        [XmlArrayItem("travel_prop_value")]
        public List<TravelPropValue> TravelPropValues { get; set; }
    }
}

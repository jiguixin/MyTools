using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Domain;

namespace Top.Api.Response
{
    /// <summary>
    /// TravelItemplayGetResponse.
    /// </summary>
    public class TravelItemplayGetResponse : TopResponse
    {
        /// <summary>
        /// 线路玩法列表
        /// </summary>
        [XmlArray("plays")]
        [XmlArrayItem("travel_play")]
        public List<TravelPlay> Plays { get; set; }
    }
}

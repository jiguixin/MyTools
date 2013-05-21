using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// TravelItemProp Data Structure.
    /// </summary>
    [Serializable]
    public class TravelItemProp : TopObject
    {
        /// <summary>
        /// 商品所属类目ID。发布旅游线路商品有两个值：1(国内线路类目ID)，2(国际线路类目ID)。
        /// </summary>
        [XmlElement("cid")]
        public long Cid { get; set; }

        /// <summary>
        /// 是否销售属性。可选值:true(是),false(否)。
        /// </summary>
        [XmlElement("is_sale_prop")]
        public bool IsSaleProp { get; set; }

        /// <summary>
        /// 发布商品时是否可以多选。可选值: true (是) , false(否)。
        /// </summary>
        [XmlElement("multi")]
        public bool Multi { get; set; }

        /// <summary>
        /// 发布商品时是否必选。可选值: true (是) , false(否)。
        /// </summary>
        [XmlElement("must")]
        public bool Must { get; set; }

        /// <summary>
        /// 属性名称。
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 旅游商品类目属性ID。
        /// </summary>
        [XmlElement("pid")]
        public long Pid { get; set; }

        /// <summary>
        /// 排列序号，表示同级类目的展现次序，如数值相等则按名称次序排列。取值范围:大于零的整数。
        /// </summary>
        [XmlElement("sort_order")]
        public long SortOrder { get; set; }

        /// <summary>
        /// 状态。可选值:normal(正常),deleted(删除)。
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        /// 类目属性值集合。
        /// </summary>
        [XmlArray("travel_prop_values")]
        [XmlArrayItem("travel_prop_value")]
        public List<TravelPropValue> TravelPropValues { get; set; }
    }
}

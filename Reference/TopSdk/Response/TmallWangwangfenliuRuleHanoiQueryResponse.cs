using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuRuleHanoiQueryResponse.
    /// </summary>
    public class TmallWangwangfenliuRuleHanoiQueryResponse : TopResponse
    {
        /// <summary>
        /// JSON格式汉诺塔分组关系信息
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

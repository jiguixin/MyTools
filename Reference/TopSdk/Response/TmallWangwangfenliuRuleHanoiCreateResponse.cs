using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuRuleHanoiCreateResponse.
    /// </summary>
    public class TmallWangwangfenliuRuleHanoiCreateResponse : TopResponse
    {
        /// <summary>
        /// 创建情况
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

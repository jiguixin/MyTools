using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuRuleHanoiUpdateResponse.
    /// </summary>
    public class TmallWangwangfenliuRuleHanoiUpdateResponse : TopResponse
    {
        /// <summary>
        /// 更新情况
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

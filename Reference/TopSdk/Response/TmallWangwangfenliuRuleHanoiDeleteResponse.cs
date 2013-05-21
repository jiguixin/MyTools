using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuRuleHanoiDeleteResponse.
    /// </summary>
    public class TmallWangwangfenliuRuleHanoiDeleteResponse : TopResponse
    {
        /// <summary>
        /// 删除汉诺塔分组关系情况
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

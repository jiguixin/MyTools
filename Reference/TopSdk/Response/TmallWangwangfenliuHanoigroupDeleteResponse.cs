using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuHanoigroupDeleteResponse.
    /// </summary>
    public class TmallWangwangfenliuHanoigroupDeleteResponse : TopResponse
    {
        /// <summary>
        /// 返回删除情况信息
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

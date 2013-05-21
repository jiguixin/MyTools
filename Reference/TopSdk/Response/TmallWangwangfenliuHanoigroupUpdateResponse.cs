using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuHanoigroupUpdateResponse.
    /// </summary>
    public class TmallWangwangfenliuHanoigroupUpdateResponse : TopResponse
    {
        /// <summary>
        /// 返回更新是否成功
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

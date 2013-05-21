using System;
using System.Xml.Serialization;

namespace Top.Api.Response
{
    /// <summary>
    /// TmallWangwangfenliuHanoigroupCreateResponse.
    /// </summary>
    public class TmallWangwangfenliuHanoigroupCreateResponse : TopResponse
    {
        /// <summary>
        /// 返回创建情况信息
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; }
    }
}

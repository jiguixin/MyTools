using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: tmall.wangwangfenliu.hanoigroup.delete
    /// </summary>
    public class TmallWangwangfenliuHanoigroupDeleteRequest : ITopRequest<TmallWangwangfenliuHanoigroupDeleteResponse>
    {
        /// <summary>
        /// 汉诺塔的分组ID
        /// </summary>
        public Nullable<long> HanoiGroupId { get; set; }

        /// <summary>
        /// 汉诺塔分组的标签ID
        /// </summary>
        public Nullable<long> HanoiLabelId { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "tmall.wangwangfenliu.hanoigroup.delete";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("hanoi_group_id", this.HanoiGroupId);
            parameters.Add("hanoi_label_id", this.HanoiLabelId);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("hanoi_group_id", this.HanoiGroupId);
            RequestValidator.ValidateRequired("hanoi_label_id", this.HanoiLabelId);
        }

        #endregion

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new TopDictionary();
            }
            this.otherParameters.Add(key, value);
        }
    }
}

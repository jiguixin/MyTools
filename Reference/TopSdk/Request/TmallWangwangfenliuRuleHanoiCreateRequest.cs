using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: tmall.wangwangfenliu.rule.hanoi.create
    /// </summary>
    public class TmallWangwangfenliuRuleHanoiCreateRequest : ITopRequest<TmallWangwangfenliuRuleHanoiCreateResponse>
    {
        /// <summary>
        /// 汉诺塔的分组ID
        /// </summary>
        public Nullable<long> HanoiGroupId { get; set; }

        /// <summary>
        /// 汉诺塔分组的标签ID
        /// </summary>
        public Nullable<long> HanoiLabelId { get; set; }

        /// <summary>
        /// 优先级为小数格式的字符串
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 服务分组名称
        /// </summary>
        public string ServiceGroupName { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "tmall.wangwangfenliu.rule.hanoi.create";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("hanoi_group_id", this.HanoiGroupId);
            parameters.Add("hanoi_label_id", this.HanoiLabelId);
            parameters.Add("priority", this.Priority);
            parameters.Add("service_group_name", this.ServiceGroupName);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("hanoi_group_id", this.HanoiGroupId);
            RequestValidator.ValidateRequired("hanoi_label_id", this.HanoiLabelId);
            RequestValidator.ValidateRequired("priority", this.Priority);
            RequestValidator.ValidateMaxLength("priority", this.Priority, 6);
            RequestValidator.ValidateRequired("service_group_name", this.ServiceGroupName);
            RequestValidator.ValidateMaxLength("service_group_name", this.ServiceGroupName, 64);
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

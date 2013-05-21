using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.travel.itemplay.get
    /// </summary>
    public class TravelItemplayGetRequest : ITopRequest<TravelItemplayGetResponse>
    {
        /// <summary>
        /// 商品所属类目ID。旅游线路商品有两个值：1(国内线路类目ID)，2(国际线路类目ID)
        /// </summary>
        public Nullable<long> Cid { get; set; }

        /// <summary>
        /// 目的地code列表，多个目的地code以“,”分隔
        /// </summary>
        public string DestCodes { get; set; }

        /// <summary>
        /// 玩法类型，1跟团游、2自由行
        /// </summary>
        public Nullable<long> PlayType { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "taobao.travel.itemplay.get";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("cid", this.Cid);
            parameters.Add("dest_codes", this.DestCodes);
            parameters.Add("play_type", this.PlayType);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("cid", this.Cid);
            RequestValidator.ValidateRequired("dest_codes", this.DestCodes);
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

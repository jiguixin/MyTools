using System;
using System.Collections.Generic;
using Top.Api.Response;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: tmall.product.spec.add
    /// </summary>
    public class TmallProductSpecAddRequest : ITopUploadRequest<TmallProductSpecAddResponse>
    {
        /// <summary>
        /// 产品二维码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 存放产品规格认证类型-认证图片url映射信息，格式为k:v;k:v;，其中key为认证类型数字id，value为调用tmall.product.spec.pic.upload返回的认证图片url文本
        /// </summary>
        public string CertifiedPicStr { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public FileItem Image { get; set; }

        /// <summary>
        /// 产品上市时间
        /// </summary>
        public Nullable<DateTime> MarketTime { get; set; }

        /// <summary>
        /// 产品货号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public Nullable<long> ProductId { get; set; }

        /// <summary>
        /// 产品的规格属性
        /// </summary>
        public string SpecProps { get; set; }

        /// <summary>
        /// 规格属性别名,只允许传颜色别名
        /// </summary>
        public string SpecPropsAlias { get; set; }

        private IDictionary<string, string> otherParameters;

        #region ITopRequest Members

        public string GetApiName()
        {
            return "tmall.product.spec.add";
        }

        public IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("barcode", this.Barcode);
            parameters.Add("certified_pic_str", this.CertifiedPicStr);
            parameters.Add("market_time", this.MarketTime);
            parameters.Add("product_code", this.ProductCode);
            parameters.Add("product_id", this.ProductId);
            parameters.Add("spec_props", this.SpecProps);
            parameters.Add("spec_props_alias", this.SpecPropsAlias);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
            RequestValidator.ValidateRequired("barcode", this.Barcode);
            RequestValidator.ValidateRequired("image", this.Image);
            RequestValidator.ValidateRequired("product_id", this.ProductId);
            RequestValidator.ValidateMaxLength("spec_props_alias", this.SpecPropsAlias, 60);
        }

        #endregion

        #region ITopUploadRequest Members

        public IDictionary<string, FileItem> GetFileParameters()
        {
            IDictionary<string, FileItem> parameters = new Dictionary<string, FileItem>();
            parameters.Add("image", this.Image);
            return parameters;
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

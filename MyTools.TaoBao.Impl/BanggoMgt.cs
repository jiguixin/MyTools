/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using System;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyTools.TaoBao.Impl
{
    public class BanggoMgt:IBanggoMgt
    {
        /// <summary>
        /// 得到单个产品信息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public BanggoProduct GetGoodsInfo(BanggoRequestModel requestModel)
        {
            #region 得到banggo数据

            if (requestModel == null)
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                  new System.Diagnostics.StackTrace().ToString()));

            string url = string.Format(
                "http://act.banggo.com/Ajax/cartAjax?time={0}&ajaxtype=color_size&type=size&code={1}&r_code={2}&goods_sn={3}",
                DateTime.Now.Ticks, requestModel.SizeCode, requestModel.ColorCode, requestModel.GoodsSn);
            var restClient = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Host", "act.banggo.com");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Referer", requestModel.Referer);
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");


            var response = restClient.Execute(request);


            if (response.ErrorException != null)
                throw response.ErrorException;

            #endregion

            #region 构造成实体对象

            string result = response.Content.TrimStart('(').TrimEnd(')');

            JObject jObj = JObject.Parse(result);

            BanggoProduct bProduct = new BanggoProduct();

            JToken goodsInfo = jObj.SelectToken("goodsInfo");
             
            bProduct.BrandCode = goodsInfo.SelectToken("brand_code").ToString();
            bProduct.GoodsSn = goodsInfo.SelectToken("goods_sn").ToString();
            bProduct.MarketPrice = goodsInfo.SelectToken("market_price").Value<double>();
            bProduct.VipPrice = goodsInfo.SelectToken("vip").Value<double>();
            bProduct.SvipPrice = goodsInfo.SelectToken("svip").Value<double>(); 
            bProduct.SalePrice = goodsInfo.SelectToken("sale_price").Value<double>();
            bProduct.SalesVolume = goodsInfo.SelectToken("sale_count").Value<int>();
            
            

           //bProduct.MarketPrice = 

             

            //             jObj.SelectToken("avl_num").Value<int>

            JToken jtAvl = jObj["avl_num"];


            JArray a = JArray.Parse(jObj["list"].ToString());

            foreach (JToken jToken in a)
            {
                string cCode = jToken["color_code"].ToString();
            }
             

            #endregion

            return null;

        }

        //todo:2013.5.15 完成以下功能
        public void GetProductBaseInfo(BanggoProduct product, BanggoRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public void GetAvailableColor(BanggoProduct product, BanggoRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public void GetAvailableSize(BanggoProduct product, BanggoRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}
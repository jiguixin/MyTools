/*
 *名称：SellDetial
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-06-04 16:18:46
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.DomainModule
{
    /// <summary>
    /// 销售详情实体表
    /// </summary>
    public class SellDetial
    {
        //public SellDetial(DataRow dr)
        //{
        //    OrderNo = Util.Get<string>(dr, "订单编号");
        //    CreateTime = Util.Get<string>(dr, "卖出时间");
        //    Source = Util.Get<string>(dr, "货源");
        //    GoodsSn = Util.Get<string>(dr, "款号");
        //    Props = Util.Get<string>(dr, "商品属性");

        //}
        public string OrderNo { get; set; }
        public string CreateTime { get; set; }
        public string Source { get; set; }
        public string GoodsSn { get; set; }
        public string Props { get; set; }
        public double MarketPrice { get; set; }
        public double SaleTotalPrice { get; set; }
        public double HePayPostage { get; set; }
        public double SinglePrice { get; set; }
        public double Count { get; set; }
        public double MyPayPostage { get; set; }
        public double CostPrice { get; set; }
        public string PayStatus { get; set; }
        public string PayTime { get; set; }
        public string Account { get; set; }
        public string Remark { get; set; }
 
    }
}
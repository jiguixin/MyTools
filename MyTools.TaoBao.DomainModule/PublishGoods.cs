/*
 *名称：PublishGoods
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-29 09:31:50
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Infrastructure.Crosscutting.Utility;
using Newtonsoft.Json; 

namespace MyTools.TaoBao.DomainModule
{
    /// <summary>
    /// 重EXCEL读取来要发布的产品实体
    /// </summary>
    public class PublishGoods
    { 
        #region Constructor
         
        public PublishGoods(DataRow dr)
        {
            Url = Util.Get<string>(dr, "产品地址");
            GoodsSn = Util.Get<string>(dr, "款号");
            SalePrice = Util.Get<Double>(dr, "售价");
            Stock = Util.Get<int>(dr, "库存");
            string suk;
            if ((suk = Util.Get<string>(dr, "SKU")) != null)
            {
                ProductColors = JsonConvert.DeserializeObject<List<ProductColor>>(suk);
                //如果用户填了售价才更新价格，没有填就用导出的默认值。
                if (SalePrice > 0)
                {
                    foreach (var size in ProductColors.SelectMany(color => color.SizeList))
                    {
                        size.MySalePrice = SalePrice;
                    }    
                }
            }

            IsSoldOut = Util.Get<bool>(dr, "售完");


        }

        #endregion

        #region Members

        public string Url { get; private set; }
        public string GoodsSn { get; private set; }
        public double SalePrice { get; private set; }
        public int Stock { get; private set; }
        public List<ProductColor> ProductColors { get; private set; }
        public bool IsSoldOut { get; private set; }

        #endregion
         
        #region Public Methods

        #endregion
         
        #region Private Methods

        #endregion
    } 
}
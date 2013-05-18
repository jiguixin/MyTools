/*
 *名称：ShopLocalData
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 09:44:23
 *修改时间：
 *备注：
 */

using System;
using MyTools.TaoBao.Interface;

namespace MyTools.TaoBao.Impl
{
    /// <summary>
    ///     通过查询本地已经做好的数据
    /// </summary>
    public class ShopLocalData:IShop
    { 

        #region Members

        #endregion

        #region Constructor

        #endregion


        #region Public Methods

        //todo 需要创建店铺的类别对应数据表
        public string GetSellerCids(string userNick, string parentSellCatName, params string[] childSellCatsNames)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Private Methods

        #endregion

        
    }
}
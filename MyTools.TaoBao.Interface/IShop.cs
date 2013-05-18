/*
 *名称：IShop
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 09:41:26
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.Interface
{
    public interface IShop
    {
        /// <summary>
        /// 获取商品所属的店铺类目列表
        /// </summary>
        /// <param name="userNick">淘宝用户名</param>
        /// <param name="parentSellCatName">店铺的父组类目</param>
        /// <param name="childSellCatsNames">子类目列表</param>
        string GetSellerCids(string userNick, string parentSellCatName, params string[] childSellCatsNames); 
    }
}
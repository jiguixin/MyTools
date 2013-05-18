/*
 *名称：ICatalog
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 09:55:05
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.Interface
{
    public interface ICatalog
    {
        /// <summary>
        /// 根据父类目和子类目->获取后台供卖家发布商品的标准商品类目id
        /// </summary>
        /// <param name="parentCatalog">父类目</param>
        /// <param name="childCatalog">子类目</param>        
        string GetCid(string parentCatalog, string childCatalog);

        /// <summary>
        /// 得到待发布商品的属性串
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        string GetItemProps(string cid);
    }
}
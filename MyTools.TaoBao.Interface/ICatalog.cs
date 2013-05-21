/*
 *名称：ICatalog
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 09:55:05
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;

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
        /// 只先提取必填项
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        string GetItemProps(string cid);

        /// <summary>
        /// 得到相关属性串
        /// </summary>
        /// <param name="propName">要查询属性的名字</param>
        /// <param name="cid">对应的淘宝目录编号</param>
        /// <returns></returns>
        List<string> GetProps(string propName, string cid);

        /// <summary>
        /// 得到销售属性，如颜色，大小，
        /// </summary>
        /// <param name="isColorProp">是获取颜色属性</param>
        /// <param name="cid">对应的淘宝目录编号</param>
        /// <returns></returns>
        List<string> GetSaleProp(bool isColorProp, string cid);

    } 
}
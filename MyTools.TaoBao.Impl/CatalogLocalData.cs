﻿/*
 *名称：CatalogLocalData
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 09:59:31
 *修改时间：
 *备注：
 */

using System;
using MyTools.TaoBao.Interface;

namespace MyTools.TaoBao.Impl
{
    /// <summary>
    /// 通过查询本地数据库得到相应的目录信息
    /// </summary>
    public class CatalogLocalData:ICatalog
    {

        #region Members

        #endregion


        #region Constructor

        #endregion


        #region Public Methods
        //todo 在CatalogMap表中取值
        public string GetCid(string parentCatalog, string childCatalog)
        {
            throw new NotImplementedException();
        }

        public string GetItemProps(string cid)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Private Methods

        #endregion

       
    }
}
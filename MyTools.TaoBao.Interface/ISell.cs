/*
 *名称：ISell
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-06-04 04:19:57
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.Interface
{
    public interface ISell
    {
        /// <summary>
        /// 导出销售详情
        /// </summary>
        /// <param name="sourcePath">淘宝下载的销售数据</param>
        void ExportSellDetail(string sourcePath);
    }
}
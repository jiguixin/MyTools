/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using MyTools.TaoBao.DomainModule;

namespace MyTools.TaoBao.Interface
{
    public interface IBanggoMgt
    {
        /// <summary>
        /// 得到单个产品信息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        BanggoProduct GetGoodsInfo(BanggoRequestModel requestModel);
    }
}
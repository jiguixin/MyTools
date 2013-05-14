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

        /// <summary>
        /// 读取或构造单个产品的基础信息。
        /// 包括：标题、价格、销量、产品描述        
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        void GetProductBaseInfo(BanggoProduct product, BanggoRequestModel requestModel);

        /// <summary>
        /// 得到可售商品颜色
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        void GetAvailableColor(BanggoProduct product, BanggoRequestModel requestModel);

        /// <summary>
        /// 得到可售商品颜色
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        void GetAvailableSize(BanggoProduct product, BanggoRequestModel requestModel);


    }
}
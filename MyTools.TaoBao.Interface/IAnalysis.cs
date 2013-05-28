/*
 *名称：IAnalysis
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-24 03:58:39
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.Interface
{
/*1	获得自己在售的商品列表，可以加相应的条件来查询，只用得到num_iid（taobao.items.onsale.get 获取当前会话用户出售中的商品列表）。
2	通过调用taobao.item.get 得到单个商品详细信息。
3	得到款号和品牌对比分析淘宝的价格值。
4	提取property_alias数据，以‘;’分割成，以另名(155(s))为Key,props为value，放到dictionary中。
5	将所有的skus中的数量设置为0，价格设置价格为第3步的结果。（）
6	用banggo的color+size （别名）得到在dictionary中的values，然后将color和size用”;”号组成prop。
7	遍历每个SKU属性，用上面得到的prop在检查每个sku的properties的值是否一样。
7.1	如果根据步骤6的结果发现skus没有相应的都需要重新添加改sku属性。
8	得到符合要求的sku修改其价格和数量。
9	再次执行步骤2*/
     
    public interface IAnalysis
    {
        /// <summary>
        /// 得到分析后的价格
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        double GetAnalysisPrice(string query);

        /// <summary>
        /// 导出竞争对手的产品信息
        /// </summary>
        /// <param name="query">在淘宝中输入的产品信息</param>
        /// <param name="marketPrice">该产品的市场价</param>
        /// <param name="salePrice">我的售价</param>
        void ExportRivalGoodsInfo(string query, double marketPrice = 0, double salePrice = 0);

        /// <summary>
        /// 导出该产品banggo的数据及淘宝竞争对手的数据，并生成EXCEL
        /// </summary>
        /// <param name="goodsUrl">产品URL</param>
        void ExportBanggoAndTaobaoGoodsInfo(string goodsUrl);
    }
}
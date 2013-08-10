///////////////////////////////////////////////////////////
//  itemCatsApi.cs
//  Implementation of the Class itemCatsApi
//  Generated by Enterprise Architect
//  Created on:      08-五月-2013 0:20:07
//  Original author: 吉桂昕
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.IoC;
using MyTools.Framework.Common;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Infrastructure.Crosscutting.Declaration;

namespace MyTools.TaoBao.Impl
{
    /// <summary>
    ///     通过已配置好的Cid,Props表中获取数据
    /// </summary>
    public class ItemCatsApi : IItemCatsApi
    {
        ITopClient client = InstanceLocator.Current.GetInstance<ITopClient>();

        //根据淘宝的父类目和子类目->获取后台供卖家发布商品的标准商品类目id
        /// <summary>
        /// 根据淘宝的父类目和子类目->获取后台供卖家发布商品的标准商品类目id
        /// </summary>
        /// <param name="parentCatalog">父类目， 如：男装</param>
        /// <param name="childCatalog">子类目，如：T恤</param>        
        public string GetCid(string parentCatalog,string childCatalog)
        {
            var parentItemCat = GetAllItemCatByApi(0).Find(c => c.Name.Contains(parentCatalog));
              
            if (parentItemCat == null)
            {
                //如果在淘宝中没有找到就在映射中找 
                parentItemCat = GetAllItemCatByApi(0).Find(c => c.Name.Contains(SysUtils.GetCustomCategoryMap(parentCatalog)));
    
                if (parentItemCat == null)
                    throw new Exception(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new System.Diagnostics.StackTrace().ToString()));
            }


            ItemCat childItemCat = null;


            childItemCat = GetAllItemCatByApi(parentItemCat.Cid).Find(c => c.Name.Contains(childCatalog));


            //如果淘宝子类别下还有子类别,如：女童->外套->普通外套等。
            //那么直到遍历完相应子目录才结束。
            while (childItemCat != null && childItemCat.IsParent)
            {
                childItemCat = GetAllItemCatByApi(childItemCat.Cid).Find(c => c.Name.Contains(childCatalog)); 
            } 

            if (childItemCat == null)
            {
                //没有找到然后在到GetCustomCidMap中查找 
                return SysUtils.GetCustomCidMap(parentCatalog, childCatalog);
            }

            return childItemCat.Cid.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 得到待发布商品的属性串
        /// 只先提取必填项
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public string GetItemProps(string cid)
        {
            var sb = new StringBuilder();
            var lstMustProps = GetPropsByCid(cid.ToLong()).Where(p=>p.Must);

            foreach (var prop in lstMustProps)
            {   
                sb.AppendFormat("{0}:{1};", prop.Pid, prop.PropValues[0].Vid);
            }

            return sb.ToString(); 
        }

        /// <summary>
        /// 得到相关属性串
        /// </summary>
        /// <param name="propName">要查询属性的名字</param>
        /// <param name="cid">对应的淘宝目录编号</param>
        /// <returns></returns>
        public List<string> GetProps(string propName, string cid)
        {
            var props = GetPropsByCid(cid.ToLong());

            var prop = (from p in props where p.Name.Contains(propName) select p).First();

            return prop.PropValues.Select(pValue => "{0}:{1}".StringFormat(prop.Pid, pValue.Vid)).ToList();

            #region 原始方法

            /*
            foreach (var pValue in prop.PropValues)
            {

                listResult.Add(string.Format("{0}:{1};", prop.Pid, pValue.Vid));

            }*/

            #endregion
              
        }
        /// <summary>
        /// 得到销售属性，如颜色，大小，
        /// </summary>
        /// <param name="isColorProp">是获取颜色属性 如果是为true 不是为false</param>
        /// <param name="cid">对应的淘宝目录编号</param>
        /// <returns></returns>
        public List<string> GetSaleProp(bool isColorProp, string cid)
        {
            var prop = GetPropsByCid(cid.ToLong(),isColorProp,true).First();

            return prop.PropValues.Select(pValue => "{0}:{1}".StringFormat(prop.Pid, pValue.Vid)).ToList(); 
        }


        //得到淘宝的所有商品类目
        /// <summary>
        /// 得到淘宝的所有商品类目
        /// taobao.itemcats.get 获取后台供卖家发布商品的标准商品类目
        /// </summary>
        /// <returns></returns>
        public List<ItemCat> GetAllItemCatByApi(long parentCid)
        {  
            ItemcatsGetRequest req = new ItemcatsGetRequest();
            req.Fields = "cid,parent_cid,name,is_parent";
            req.ParentCid = parentCid;
            ItemcatsGetResponse response = client.Execute(req);

            return response.ItemCats; 
        }

        //类目API taobao.itemprops.get 获取标准商品类目属性（包括：货号、品牌、衣长等）
        /// <summary>
        /// 类目API taobao.itemprops.get 获取标准商品类目属性（包括：货号、品牌、衣长等）
        /// 注：1，货号，是放在input_str，input_pids 中，如：【（input_pids：input_str）
        /// （1632501：238286）】
        /// 2，品牌，如果淘宝类目中有该品牌如：那么就加到props中，如果没有，需要自定义品牌者加到
        /// input_str，input_pids，如：【（input_pids：input_str）（20000：莱克）】
        /// 3,SUK（销售）属性，sku_properties中有的属性props也必须存在。
        /// sku_properties中以，分开。
        /// </summary>
        /// <param name="cid">淘宝所属类目ID</param>
        /// <param name="isColorProp">是否为颜色属性</param>
        /// <param name="isSaleProp">是否为销售属性</param>
        public List<ItemProp> GetPropsByCid(long cid, bool? isColorProp = null, bool? isSaleProp = null)
        {
            ItempropsGetRequest req = new ItempropsGetRequest();
            req.Fields = "pid,name,must,multi,prop_values,is_color_prop,is_sale_prop";
            req.Cid = cid;
            req.IsColorProp = isColorProp;
            req.IsSaleProp = isSaleProp;
            
            ItempropsGetResponse response = client.Execute(req);
            return response.ItemProps; 
        }

        ~ItemCatsApi()
        {
        }

        public virtual void Dispose()
        {
        }


    }
}

//end itemCatsApi
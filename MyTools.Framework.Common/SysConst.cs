/*
 *名称：SysConst
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-10 10:54:48
 *修改时间：
 *备注：
 */

using System.Collections.Generic;
using System.Configuration;

namespace MyTools.Framework.Common
{
    public class SysConst
    {
        public static readonly string AppKey = ConfigurationManager.AppSettings["appKey"];
        public static readonly string AppSecret = ConfigurationManager.AppSettings["appSecret"];
        public static readonly string PostageId = ConfigurationManager.AppSettings["PostageId"]; 
        public static readonly string DiscountRatio = ConfigurationManager.AppSettings["DiscountRatio"]; 
        public static readonly string PrefixTitle = ConfigurationManager.AppSettings["PrefixTitle"];

        public static readonly string LocationState = ConfigurationManager.AppSettings["LocationState"];
        public static readonly string LocationCity = ConfigurationManager.AppSettings["LocationCity"];

        
        


        public static readonly Dictionary<string, string> CategoryBanggoToTaobaoMap = new Dictionary<string, string>();
        public static readonly Dictionary<string, string> ChildCatalogBanggoToTaobaoCid = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> ManCatalogBanggoToTaobaoCid = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> WomenCatalogBanggoToTaobaoCid = new Dictionary<string, string>();

        static SysConst()
        {
            CategoryBanggoToTaobaoMap.Add("男童", "童装");
            CategoryBanggoToTaobaoMap.Add("女童", "童装");
            CategoryBanggoToTaobaoMap.Add("ME-CITY", "ME&CITY");
             
            ChildCatalogBanggoToTaobaoCid.Add("休闲裤", "50013618"); //对应的是裤子
            ChildCatalogBanggoToTaobaoCid.Add("牛仔裤", "50013618"); //对应的是裤子
            ChildCatalogBanggoToTaobaoCid.Add("针织裤", "50013618"); //对应的是裤子
            ChildCatalogBanggoToTaobaoCid.Add("裤类", "50013618"); //对应的是裤子 
            ChildCatalogBanggoToTaobaoCid.Add("茄克", "50010519"); //夹克\/皮衣 
            ChildCatalogBanggoToTaobaoCid.Add("大衣", "50010520"); //呢大衣
            ChildCatalogBanggoToTaobaoCid.Add("开衫", "50010539"); //毛衣\/针织衫 
            ChildCatalogBanggoToTaobaoCid.Add("裙类", "50013693"); //裙子
            ChildCatalogBanggoToTaobaoCid.Add("配件", "50014503");//挎包\/拎包\/休闲包


            WomenCatalogBanggoToTaobaoCid.Add("裙类", "50010850"); //连衣裙
            WomenCatalogBanggoToTaobaoCid.Add("裤类", "162205"); //牛仔裤
            WomenCatalogBanggoToTaobaoCid.Add("配件", "162404");//挎包\/拎包\/休闲包


            ManCatalogBanggoToTaobaoCid.Add("裤类", "50010167");//牛仔裤
            ManCatalogBanggoToTaobaoCid.Add("西装", "50011130");//西服套装
            ManCatalogBanggoToTaobaoCid.Add("茄克", "50010158");//夹克
            ManCatalogBanggoToTaobaoCid.Add("配件", "50005867");//工装制服


        }

    }
}
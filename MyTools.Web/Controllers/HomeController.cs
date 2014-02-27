using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using MyTools.Framework.Common;
using MyTools.Framework.Common.ExceptionDef;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;

namespace MyTools.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGoodsPublish _goodsPublish = InstanceLocator.Current.GetInstance<IGoodsPublish>(Resource.SysConfig_Banggo);
        private readonly IRequest _request = InstanceLocator.Current.GetInstance<IRequest>(Resource.SysConfig_Banggo);

        //App Key与App Secret在"应用证书"得到 
        private ITopClient client = InstanceLocator.Current.GetInstance<ITopClient>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var authorUrl = "https://oauth.taobao.com/authorize?response_type=code&client_id={0}&redirect_uri=http://www.dabaolo.com/Home/Contact/&state=1212&view=web".StringFormat(SysConst.AppKey);

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //todo:跳转不回调
            return Redirect(authorUrl); 
        }



        public ActionResult About()
        {
           // var authorizeUrl = Resource.SysConfig_AuthorizeUrl.StringFormat(SysConst.AppKey);

            //Redirect(authorizeUrl);

            var authorUrl = "https://oauth.taobao.com/authorize?response_type=code&client_id={0}&redirect_uri=http://www.dabaolo.com/Home/Contact/&state=1212&view=web".StringFormat(SysConst.AppKey);

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //todo:跳转不回调
            return Redirect(authorUrl);
            //return new EmptyResult();
            //return Empty();
        }

        public ActionResult Contact()
        {
           var code = Request.QueryString["code"].ToString();

            Dictionary<string, string> dicParam = new Dictionary<string, string>();
            dicParam.Add("client_id", SysConst.AppKey);
            dicParam.Add("client_secret", SysConst.AppSecret);
            dicParam.Add("grant_type", "authorization_code");
            dicParam.Add("code", code);
            dicParam.Add("redirect_uri", "http://www.dabaolo.com/Home/Contact"); 


            Top.Api.Util.WebUtils webUtils = new Top.Api.Util.WebUtils();
            var result = webUtils.DoPost("https://oauth.taobao.com/token", dicParam);

            dynamic objResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
           
            var sessionKey = objResult.access_token;

            InstanceLocator.Current.RegisterInstance<AuthorizedContext>(new AuthorizedContext() { SessionKey = sessionKey, AppKey = SysConst.AppKey, UserNick = objResult.taobao_user_nick, UserId = objResult.taobao_user_id, RefreshToken = objResult.refresh_token });

//            var context = TopUtils.GetTopContext("6201a25b6a85595fe8af978a4c64ededdfh6ff412cdcb8b820330575"); 

            /*var context = new TopContext();
            context.SessionKey.SessionKey = "6201a25b6a85595fe8af978a4c64ededdfh6ff412cdcb8b820330575";

            InstanceLocator.Current.RegisterInstance<TopContext>(context);*/

            //ViewBag.Message = result;

            //todo SessionKey 是获取正确的，但是TopContext 有问题，得重构代码
            var req = new DeliveryTemplatesGetRequest { Fields = "template_id,template_name" };

            var tContext = InstanceLocator.Current.GetInstance<AuthorizedContext>();

            DeliveryTemplatesGetResponse response = client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);

            var res = response.DeliveryTemplates;

            if (res != null)
            {
                ViewBag.Message = "{0} 个+{1}".StringFormat(res.Count, result);
            }
            else
            {
                ViewBag.Message = "0 个";
            }

           

            /*
            _goodsPublish.UpdateGoodsFromOnSale(_request,new string[] {"222989"}, false);
*/
            return View();
        }
    }
}

﻿#region Copyright (C) 2017 Kevin (OS系列开源项目)

/***************************************************************************
*　　	文件功能描述：公号的功能接口 —— 卡券投放接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-1-22
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using Newtonsoft.Json;
using OSS.Common.ComModels.Enums;
using OSS.Http;
using OSS.Http.Models;
using OSS.Social.WX.Offcial.Card.Mos;
using OSS.Social.WX.SysUtils.Mos;

namespace OSS.Social.WX.Offcial.Card
{
   public  partial class WxOffCardApi
    {
        #region   投放卡券

        /// <summary>
        ///   生成单卡券投放二维码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="cardQrMo"></param>
        /// <returns></returns>
        public WxCardQrCodeResp CreateCardQrCode(WxQrCodeType type, int expireSeconds, WxCardQrMo cardQrMo)
        {
            var actionInfo = new WxCreateCardQrReq()
            {
                expire_seconds = expireSeconds,
                action_name = type,
                action_info = new { card = cardQrMo }
            };
            return CreateCardQrCode(actionInfo);
        }

        /// <summary>
        ///   生成多卡券投放二维码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public WxCardQrCodeResp CreateMultiCardQrCode(WxQrCodeType type, int expireSeconds, List<WxCardQrMo> cardList)
        {
            if (cardList == null || cardList.Count > 5)
                return new WxCardQrCodeResp() { Ret = (int)ResultTypes.ParaNotMeet, Message = "卡券数目不和要求，请不要为空或超过五个！" };

            var actionInfo = new WxCreateCardQrReq()
            {
                expire_seconds = expireSeconds,
                action_name = type,
                action_info = new { multiple_card = new { card_list = cardList } }
            };
            return CreateCardQrCode(actionInfo);
        }


        /// <summary>
        /// 生成卡券投放二维码
        /// </summary>
        /// <param name="actionInfo"></param>
        /// <returns></returns>
        private WxCardQrCodeResp CreateCardQrCode(WxCreateCardQrReq actionInfo)
        {
            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = string.Concat(m_ApiUrl, "/card/qrcode/create");
            req.CustomBody = JsonConvert.SerializeObject(actionInfo);

            return RestCommonOffcial<WxCardQrCodeResp>(req);
        }



        /// <summary>
        ///   导入卡券code
        /// </summary>
        /// <param name="cardId">需要进行导入code的卡券ID</param>
        /// <param name="codes">需导入微信卡券后台的自定义code，上限为100个</param>
        /// <returns></returns>
        public WxImportCardCodeResp ImportCardCode(string cardId, List<string> codes)
        {
            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = string.Concat(m_ApiUrl, "/card/code/deposit");
            req.CustomBody = JsonConvert.SerializeObject(new { card_id = cardId, code = codes });

            return RestCommonOffcial<WxImportCardCodeResp>(req);
        }

        /// <summary>
        ///   查询导入code数目接口
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WxGetImportCodeCountResp GetImportCodeCount(string cardId)
        {
            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = string.Concat(m_ApiUrl, "/card/code/getdepositcount");
            req.CustomBody = $"{{\"card_id\":\"{cardId}\"}}";

            return RestCommonOffcial<WxGetImportCodeCountResp>(req);
        }

        /// <summary>
        ///   验证已经导入的code信息
        /// </summary>
        /// <param name="cardId">需要进行导入code的卡券ID</param>
        /// <param name="codes">需导入微信卡券后台的自定义code，上限为100个</param>
        /// <returns></returns>
        public WxCheckImportCodeResp CheckImportCode(string cardId, List<string> codes)
        {
            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = string.Concat(m_ApiUrl, "/card/code/getdepositcount");
            req.CustomBody = JsonConvert.SerializeObject(new { card_id = cardId, code = codes });

            return RestCommonOffcial<WxCheckImportCodeResp>(req);
        }


        /// <summary>
        ///   获取图文推送的卡券信息
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public WxGetCardArticleContentResp GetArticleContent(string cardId)
        {
            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = string.Concat(m_ApiUrl, "/card/mpnews/gethtml");
            req.CustomBody = $"{{\"card_id\":\"{cardId}\"}}";

            return RestCommonOffcial<WxGetCardArticleContentResp>(req);
        }


        /// <summary>
        ///  创建卡券投放货架接口
        /// </summary>
        /// <param name="pageReq"></param>
        /// <returns></returns>
        public WxCreateCardLandPageResp CreateLandPage(WxCreateCardLandPageReq pageReq)
        {
            var req = new OsHttpRequest();

            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = string.Concat(m_ApiUrl, "/card/landingpage/create");
            req.CustomBody = JsonConvert.SerializeObject(pageReq);

            return RestCommonOffcial<WxCreateCardLandPageResp>(req);
        }

        #endregion



        #region  设置白名单

       /// <summary>
       /// 设置卡券测试白名单
       /// </summary>
       /// <param name="openIds"> 可选 openid列表 </param>
       /// <param name="names">可选  微信号列表  二者必填其一</param>
       /// <returns></returns>
       public WxBaseResp SetTestWhiteList(List<string> openIds, List<string> names)
       {
           var req = new OsHttpRequest();

           req.HttpMothed = HttpMothed.POST;
           req.AddressUrl = string.Concat(m_ApiUrl, "/card/testwhitelist/set");
           req.CustomBody = JsonConvert.SerializeObject(new {openid = openIds, username = names});

           return RestCommonOffcial<WxBaseResp>(req);
       }

       #endregion


    }
}

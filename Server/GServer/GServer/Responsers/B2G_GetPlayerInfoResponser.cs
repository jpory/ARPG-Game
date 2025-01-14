﻿using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;
using System.Linq;
using GServer.Managers;

namespace GServer.Responsers
{
    [HandleType(typeof(B2G_GetPlayerInfo),HandleResponserType.SERVER_SERVER)]
    public class B2G_GetPlayerInfoResponser : Responser<B2G_GetPlayerInfo, G2B_GetPlayerInfo>
    {
        public B2G_GetPlayerInfoResponser()
        {
            NeedAccess = false;
        }

        public override G2B_GetPlayerInfo DoResponse(B2G_GetPlayerInfo request, Client client)
        {
            Managers.UserData data;
            if (!MonitorPool.S.Get<UserDataManager>().TryToGetUserData(request.UserID, out data))
            {
                return new G2B_GetPlayerInfo { Code = ErrorCode.NoGamePlayerData };
            }


            return new G2B_GetPlayerInfo
            {
                Code = ErrorCode.OK,
                Package = data.GetPackage(),
                Hero = data.GetHero()
            };
        }
    }
}


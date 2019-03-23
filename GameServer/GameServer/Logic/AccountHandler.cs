using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol;
using Protocol.Dto;

namespace GameServer.Logic
{
    /// <summary>
    ///  账号处理的逻辑层
    /// </summary>
    public class AccountHandler : IHandler
    {
        public void OnDisconnect(ClientPeer client)
        {
            
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountCode.REGIST_CREQ:
                    {

                    }
                    break;
                case AccountCode.LOGIN:
                    {

                    }
                    break;
                default:
                    break;
            }
        }
    }
}

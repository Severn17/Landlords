using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Protocol.Dto;
using GameServer.Cache;


namespace GameServer.Logic
{
    /// <summary>
    ///  账号处理的逻辑层
    /// </summary>
    public class AccountHandler : IHandler
    {
        AccountCache accountCache = Caches.Account;

        public void OnDisconnect(ClientPeer client)
        {
            if(accountCache.isOnline(client))
            accountCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountCode.REGIST_CREQ:
                    {
                        AccountDto dto = value as AccountDto;
                        regist(client, dto.Account, dto.Password);
                    }
                    break;
                case AccountCode.LOGIN:
                    {
                        AccountDto dto = value as AccountDto;
                        login(client, dto.Account, dto.Password);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void regist(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (accountCache.IsExist(account))
                {
                    //账号已经存在
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号已经存在");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -1);
                    return;
                }
                if (string.IsNullOrEmpty(account))
                {
                    //账号输入不合法
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号输入不合法");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -2);

                    return;
                }
                if (string.IsNullOrEmpty(password) || password.Length < 4 || password.Length > 16)
                {
                    //密码不合法
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "密码不合法");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -3);

                    return;
                }
                // 可以注册
                accountCache.Create(account, password);
                //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "注册成功");
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, 0);

            });


        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void login(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                if (!accountCache.IsExist(account))
                {
                    // 账号不存在
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号不存在");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -1);
                    return;
                }
                if (accountCache.isOnline(account))
                {
                    //账号输入不合法
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号在线");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -2);
                    return;
                }
                if (!accountCache.IsMatch(account, password))
                {
                    //账号密码不匹配
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号密码不匹配");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -3);
                    return;
                }
                // 登陆成功
                accountCache.Online(client, account);
                //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "登陆成功");
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);
            });
        }
    }
}

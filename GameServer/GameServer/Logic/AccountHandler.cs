using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol;
using Protocol.Dto;
using GameServer.Cache;
using Protocol.Code;

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
                if (accountCache.isExist(account))
                {
                    //账号已经存在
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, "账号已经存在");
                    return;
                }
                if (string.IsNullOrEmpty(account))
                {
                    //账号输入不合法
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, "账号输入不合法");
                    return;
                }
                if (string.IsNullOrEmpty(password) || password.Length < 4 || password.Length > 16)
                {
                    //密码不合法
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, "密码不合法");
                    return;
                }
                // 可以注册
                accountCache.Create(account, password);
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, "注册成功");
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
                if (!accountCache.isExist(account))
                {
                    // 账号不存在
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号不存在");
                    return;
                }
                if (accountCache.isOnline(account))
                {
                    //账号输入不合法
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号已在线");
                    return;
                }
                if (!accountCache.IsMatch(account, password))
                {
                    //账号密码不匹配
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号密码不匹配");
                    return;
                }
                // 登陆成功
                accountCache.Online(client, account);
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "登陆成功");
            });
        }
    }
}

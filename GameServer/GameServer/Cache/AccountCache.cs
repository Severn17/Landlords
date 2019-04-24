using AhpilyServer;
using AhpilyServer.Util.ConCurrent;
using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    /// <summary>
    /// 账号缓存
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        ///  账号对应的 账号数据模型
        /// </summary>
        private Dictionary<string, AccountModel> accModelDict = new Dictionary<string, AccountModel>();

        /// <summary>
        /// 账号对应的 连接对象
        /// </summary>
        private Dictionary<string, ClientPeer> accClientDict = new Dictionary<string, ClientPeer>();
        /// <summary>
        /// 账号对应的 连接对象
        /// </summary>
        private Dictionary<ClientPeer, string> clientAccDict = new Dictionary<ClientPeer, string>();

        /// <summary>
        /// 是否存在账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsExist(string account)
        {
            return accModelDict.ContainsKey(account);
        }
        // 用来存储账号的id
        private ConCurrentInt id = new ConCurrentInt(-1);
        /// <summary>
        /// 创建账号数据模型信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void Create(string account,string password)
        {
            AccountModel model = new AccountModel(id.Add_Get(), account, password);
            accModelDict.Add(model.Account, model);
        }
        /// <summary>
        /// 获取账号对应的数据模型
        /// </summary>
        /// <param name="account"></param>
        public AccountModel GetModel(string account)
        {
            return accModelDict[account];
        }
        /// <summary>
        /// 账号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsMatch(string account,string password)
        {
            AccountModel model = accModelDict[account];
            return model.Password == password;
        }

        /// <summary>
        /// 是否在线
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        public bool isOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        /// <param name="client">连接对象</param>
        /// <returns></returns>
        public bool isOnline(ClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }
        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        public void Online(ClientPeer client,string account)
        {
            accClientDict.Add(account, client);
            clientAccDict.Add(client, account);
        }
        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            string account = clientAccDict[client];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }
        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(string account)
        {
            ClientPeer client = accClientDict[account];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }
        /// <summary>
        /// 获取在线玩家的id
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetId(ClientPeer client)
        {
            string account = clientAccDict[client];
            AccountModel model = accModelDict[account];
            return model.Id;
        }
    }
}

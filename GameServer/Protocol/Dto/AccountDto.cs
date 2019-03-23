using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class AccountDto
    {
        public string account;
        public string password;

        public AccountDto()
        {

        }

        public AccountDto(string acc,string pwd)
        {
            this.account = acc;
            this.password = pwd;
        }

    }
}

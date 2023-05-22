using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZohoGetPost
{
    //for when a new access token is obtained using the refresh token

    public class AccessTokenOnRefresh
    {
        public class Root
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
    }
}

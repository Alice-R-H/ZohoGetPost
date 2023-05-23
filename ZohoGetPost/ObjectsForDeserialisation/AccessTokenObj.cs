using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZohoGetPost.ObjectsForDeserialisation
{
    //initial access token object for when a new code is entered

    public class AccessTokenObj
    {
        public class Root
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string api_domain { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

    }
}

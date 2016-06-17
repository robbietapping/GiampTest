using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliidoAPI
{
    public class TokenResponse
    { 
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string asclient_id { get; set; }
        public string email { get; set; }
        public string quickblox_id { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }

}

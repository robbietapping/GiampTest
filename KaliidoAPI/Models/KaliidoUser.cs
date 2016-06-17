using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaliidoAPI.Models
{

    /// <summary>
    /// /COPY JSON DATA THEN GO TO EDIT > PAST SPECIAL > JSON AS CLASS
    /// </summary>

    public class KaliidoUser
    {
        public int id { get; set; }
        public int quickbloxUserId { get; set; }
        public string fullName { get; set; }
        public string photoUID { get; set; }
        public Interest[] interests { get; set; }
        public Relations relations { get; set; }
        public int userOrder { get; set; }
    }

    public class Relations
    {
        public bool isFavorite { get; set; }
        public object isFriend { get; set; }
        public object requestFriendMessage { get; set; }
        public bool isBlock { get; set; }
        public bool isReporting { get; set; }
        public object note { get; set; }
    }

    public class Interest
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}

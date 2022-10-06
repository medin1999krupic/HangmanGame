using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HangMan
{

    public class Users
    {
        [XmlElement("User")]
        public List<User> User = new List<User>();
    }
    public class User
    {
        public User()
        {
            Score = new Score();
        }
        public string Name { get; set; }
        public string Password { get; set; }

        public Score Score { get; set; }


    }
}

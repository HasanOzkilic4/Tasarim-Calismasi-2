using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{

    public class User
    {
      
        public User(string name)
        {
            Name = name;
        }
        public string Name;

        public long Score;

   

        public string Password;
        //public Dictionary<int, int> GameHistory { get; set; }

    }
}

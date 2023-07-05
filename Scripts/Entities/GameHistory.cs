using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public  class GameHistory
    {
        public string RoomOwner { get; set; }
        public GameHistory(string roomOwner)
        {
            RoomOwner = roomOwner;
        }
        public MasterClientInfo MasterClientInfo { get; set; }
        public NoMasterClientInfo NoMasterClientInfo { get; set; }
        public string Game1Winner { get; set; }
        public string Game2Winner { get; set; }
        public string Game3Winner { get; set; }

    }
    public class MasterClientInfo
    {
        public int Game1 { get; set; }
        public int Game2 { get; set; }
        public int Game3 { get; set; }
    }

    public class NoMasterClientInfo
    {
        public int Game1 { get; set; }
        public int Game2 { get; set; }
        public int Game3 { get; set; }
    }
}

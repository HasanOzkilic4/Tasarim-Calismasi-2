using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities
{
    public class TheGameHistory
    {
        public string GameOwner;
        public TheGameHistory(string gameOwner)
        {
            this.GameOwner = gameOwner;
        }
        public int MasterClientScore;
        public int NoMasterClientScore;
        public int MasterClient1Score;
        public int MasterClient2Score;
        public int MasterClient3Score;
        public int NoMasterClient1Score;
        public int NoMasterClient2Score;
        public int NoMasterClient3Score;
    }
}

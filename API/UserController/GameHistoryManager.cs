using Assets.Abstract;
using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.API.UserController
{
    internal class GameHistoryManager : IGameHistoryService
    {
        IGameHistoryDal _iGameHistoryDal;
        public GameHistoryManager(IGameHistoryDal iGameHistoryDal)
        {
            _iGameHistoryDal = iGameHistoryDal;
        }

       

       public void AddGameHistory(GameHistory gameHistory)
        {
            _iGameHistoryDal.Add(gameHistory);
        }
    }
}

using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Abstract
{
    public interface IGameHistoryService
    {

        //public Task<IResult> AddGameHistory(GameHistory gameHistory);
        public void AddGameHistory(GameHistory gameHistory);
    }
}

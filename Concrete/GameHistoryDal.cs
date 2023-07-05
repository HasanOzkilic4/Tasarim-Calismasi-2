using Assets.Abstract;
using Assets.Scripts.Entities;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Concrete
{
    public class GameHistoryDal : IGameHistoryDal
    {

        FirebaseDatabase database;
        DatabaseReference reference;
        public GameHistoryDal()
        {
            database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");

            reference = database.RootReference;
        }
        public async void Add(GameHistory entity)
        {
            string gameHistoryJson = JsonUtility.ToJson(entity);
            await reference.Child("GameHistory").Child(entity.RoomOwner).SetRawJsonValueAsync(gameHistoryJson);
        }

        public void Delete(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<GameHistory> Get(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<List<GameHistory>> GetAll(Func<GameHistory, bool> filter = null)
        {
            throw new NotImplementedException();
        }

        public void Update(string currentUsername, GameHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}

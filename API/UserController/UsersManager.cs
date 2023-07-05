using Assets.Abstract;
using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Data.Common;

namespace Assets.API.UserController
{
    public class UsersManager : IUserService
    {

        IUserDal _userDal;

        public UsersManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<IResult> AddUser(User user)
        {
            IResult logic = await IsThereSuchAUsernameInTheSystem(user.Name);
            if (logic.IsSuccess)
            {
                return new Result(false, logic.Message);
            }
            _userDal.Add(user);
            return new Result(true);
        }

        public async Task<IResult> Delete(string userName)
        {

            IResult logic = await IsThereSuchAUsernameInTheSystem(userName);
            if (logic.IsSuccess)
            {
                _userDal.Delete(userName);
                return new Result(true);
            }
            return new Result(false, "Böyle bir kullanıcı bulunamadı");



        }

        public async Task<IDataResult<User>> Get(string userName)
        {


            IResult logic = await IsThereSuchAUsernameInTheSystem(userName);
            if (logic.IsSuccess)
            {
                var user = await _userDal.Get(userName);
                return new DataResult<User>(true, user);
            }
            return new DataResult<User>(false, "Böyle bir kullanıcı bulunamadı", null);
        }


        public async Task<IDataResult<List<User>>> GetAll()
        {
            List<User> users = await _userDal.GetAll();
            return new DataResult<List<User>>(true, users);
        }

        public async Task<IDataResult<List<User>>> GetByScoreValue(int min, int max)
        {
            var result = await _userDal.GetAll(u => u.Score > min && u.Score < max);
            return new DataResult<List<User>>(true, result);
        }

        public async Task<IDataResult<User>> LogIn(string username, string password)
        {

            var result = await Get(username);
            User user = null;


            if (result.IsSuccess)
            {
                user = result.Data;
                if (user.Password == password)
                {
                    return new DataResult<User>(true, user);
                }
                else
                {
                    return new DataResult<User>(false, "Şifre hatalı", null);
                }

            }
            else
            {
                return new DataResult<User>(false, "Böyle bir kullanıcı bulunamadı", null);
            }
        }

        public async Task<IDataResult<List<User>>> RankedArrangement()
        {
            var getAllUsers= await GetAll();
            List<User> users = null;
            List<User> sortPlayersByTrophyCount = new List<User>();
            User bestPlayer = new User("default");

            if (getAllUsers.IsSuccess)
            {
                users = getAllUsers.Data;
                bestPlayer = users[0];
            }
          
            while (users.Count != 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Score > bestPlayer.Score)
                    {
                        bestPlayer = users[i];
                    }
                }

                sortPlayersByTrophyCount.Add(bestPlayer);
                users.Remove(bestPlayer);

                if (users.Count !=0)
                {
                    bestPlayer = users[0];
                }     
            }

             return new DataResult<List<User>>( true, sortPlayersByTrophyCount);
        }

        public async Task<IResult> Update(string currentUsername, User user)
        {
            IResult logic = await IsThereSuchAUsernameInTheSystem(user.Name);
            if (logic.IsSuccess && currentUsername != user.Name)
            {
                return new Result(false, logic.Message);
            }

            _userDal.Update(currentUsername, user);
            return new Result(true);
        }

        async Task<IResult> IsThereSuchAUsernameInTheSystem(string userName)
        {
            IDataResult<List<User>> users = await GetAll();

            if (users.Data.Any(u => u.Name == userName))
            {
                return new Result(true, "Böyle bir kullanıcı ismi zaten mevcut");

            }

            return new Result(false);

        }
    }
}

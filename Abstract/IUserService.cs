using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Abstract
{
    public interface IUserService
    {
        public Task<IResult> AddUser(User user);
        public Task<IResult> Delete(string userName);
        public Task<IResult> Update(string currentUsername , User user);
        public Task<IDataResult<User>> Get(string userName);
        public Task<IDataResult<List<User>>> GetAll();
        public Task<IDataResult<List<User>>> GetByScoreValue(int min, int max);
        public Task<IDataResult<User>> LogIn(string username, string password);
        public Task<IDataResult<List<User>>> RankedArrangement();

    }
}

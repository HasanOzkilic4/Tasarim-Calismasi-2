using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Results
{
    public class Result : IResult
    {
        public bool IsSuccess { get; }

        public string Message { get; } = "Message";

        public Result(bool isSuccess, string message) : this(isSuccess)
        {
           
            Message = message;

        }

        public Result(bool isSuccess)
        {
            IsSuccess= isSuccess;
        }
    }
}

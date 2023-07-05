using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Results
{
    public interface IResult
    {
        public bool IsSuccess { get;}
        public string Message { get;}

    }
}

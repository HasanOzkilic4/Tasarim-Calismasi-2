using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Results
{
    public class DataResult<T> :Result , IDataResult<T> 
    {
        public T Data { get; }
        public DataResult(bool isSuccess , string message , T data) : base(isSuccess , message)
        {
            Data = data;    
        }
        public DataResult(bool isSuccess , T data) : base(isSuccess)
        {
            Data = data;
        }
       

       
    }
}

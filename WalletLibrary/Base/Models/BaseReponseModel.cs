using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WalletLibrary.Base.Models
{
    public class BaseReponseModel<T>
    {
        /// <summary>
        /// 定義狀態碼
        /// </summary>
        public int StatusCode { get; set; }

        //public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public T Data { get; set; }
    }
}

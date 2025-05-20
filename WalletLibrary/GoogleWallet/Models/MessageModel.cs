using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletLibrary.GoogleWallet.Models.Languages;

namespace WalletLibrary.GoogleWallet.Models
{
    public class MessageModel : LocalizedItem
    {
        public string MessageType { get; set; }
        public DateTime MessageStart { get; set; }
        public DateTime MessageEnd { get; set; }
    }
}

using WalletLibrary.GoogleLibrary.Models.Languages;

namespace WalletLibrary.GoogleLibrary.Models
{
    public class MessageItem : LocalizedItem
    {
        public string MessageType { get; set; }
        public DateTime MessageStart { get; set; }
        public DateTime MessageEnd { get; set; }

        public MessageItem(
            string? header,
            string? body,
            LocalizedStringItem? localizedHeader,
            LocalizedStringItem? localizedBody
        )
            : base(header, body, localizedHeader, localizedBody) { }
    }
}

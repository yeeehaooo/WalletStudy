namespace WalletLibrary.GoogleWallet.Models.Images
{
    public class ImageDataItem
    {
        public ImageDataItem(string id, ImageItem mainImage)
        {
            Id = id;
            MainImage = mainImage;
        }

        public ImageDataItem(string id, string uri, string description)
        {
            Id = id;
            MainImage = new ImageItem(uri, description);
        }

        public string Id { get; set; }
        public ImageItem MainImage { get; set; }
    }
}

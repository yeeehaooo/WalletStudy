namespace WalletLibrary.GoogleLibrary.Templates
{
    public class FieldItem
    {
        public FieldItem(
            string fieldName,
            string fieldPath,
            string? dateFormat = null,
            bool isPredefinedItem = false
        )
        {
            FieldName = fieldName;
            FieldPath = fieldPath;
            DateFormat = dateFormat;
            IsPredefinedItem = isPredefinedItem;
        }

        public string FieldName { get; set; }
        public string FieldPath { get; set; }
        public string? DateFormat { get; set; } = null;
        public bool IsPredefinedItem { get; set; } = false;
    }
}

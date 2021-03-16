namespace HetsImport.Import
{
    public class ImportMapRecord
    {
        public string TableName { get; set; }
        public string MappedColumn { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
    }
}

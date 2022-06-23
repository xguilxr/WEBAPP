namespace Bat.PortalDeCargas.Domain.DTO
{
    public class FileDetailDTO
    {
        public string DimensionName { get; set; }
        public int RowNumber { get; set; }
        public string TemplateDimensionName { get; set; }
        public int TemplateDimensionOrder { get; set; }
        public string ValidationMessage { get; set; }
        public string Value { get; set; }
        public int TotalOfItems { get; set; }
    }
}

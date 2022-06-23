namespace Bat.PortalDeCargas.Domain.DTO
{
    public class TemplateDimensionFormDTO
    {
        public int DimensionId { get; set; }
        public int Id { get; set; }
        public bool IsUpdated { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int TemplateId { get; set; }
    }
}

namespace ReportingService.Models
{
    public class DocumentReport
    {
        public string DocumentType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int TotalDocuments { get; set; }
    }
}

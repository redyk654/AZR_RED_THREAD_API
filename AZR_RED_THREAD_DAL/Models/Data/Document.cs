namespace AZR_RED_THREAD_DAL.Models.Data
{
    public class Document : Traceability
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        // Ajoute d'autres propriétés selon les besoins
    }
}
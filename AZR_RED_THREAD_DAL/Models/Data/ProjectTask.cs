namespace AZR_RED_THREAD_DAL.Models.Data
{
    public class ProjectTask : Traceability
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        // Ajoute d'autres propriétés selon les besoins
    }
}
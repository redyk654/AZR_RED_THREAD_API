namespace AZR_RED_THREAD_DAL.Models.Data
{
    public class Project : Traceability
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
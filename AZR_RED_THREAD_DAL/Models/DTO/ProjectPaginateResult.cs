using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZR_RED_THREAD_DAL.Models.DTO
{
    public class ProjectPaginateResult
    {
        public List<Project.Project> Data { get; set; }
        public int Total { get; set; }
        public int TotalActive { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

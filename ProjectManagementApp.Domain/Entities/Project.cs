using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientCompanyName { get; set; }
        public string ExecutorCompanyName { get; set; }
        public ICollection<EmployeeProject> EmployeeProject { get; set; }
        public Employee TeamLeader { get; set; }
        public int TeamLeaderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
    }
}

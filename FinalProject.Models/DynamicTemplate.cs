using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class DynamicTemplate
    {
        public int Id { get; set; }
        public string FileTemplateName { get; set; }
        public string Domain { get; set; }
        public string Category { get; set; }
        public int SchoolYear { get; set; }
        public string Roles { get; set; }
        public int StatusId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Models
{
    public class DepartmentModel
    {
        [Key]
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string MobileNumber { get; set; }
    }
}

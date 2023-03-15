using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Data.DBEntity
{
    public class DepartmentDBEntity
    {
        [Key]
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string MobileNumber { get; set; }
    }
}

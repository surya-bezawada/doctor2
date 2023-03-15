using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Data.DBEntity
{
    [Table("tblTimesMaster")]
    public class TimingDBEntity
    {
        [Key]
        public int TimeSlotId { get; set; }
        public string Timing { get; set; }
    }
}

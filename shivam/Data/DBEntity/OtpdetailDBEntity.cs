using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Data.DBEntity
{
    public class OtpdetailDBEntity
    {
        [Key]
        public int RecId { get; set; }
        public string Mobile { get; set; }
        public int? Type { get; set; }
        public string APIResponse { get; set; }
        public string APIRequest { get; set; }
        public int BookingId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

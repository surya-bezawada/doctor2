using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Data.DBEntity
{
    [Table("tblBookings")]
    public class BookingDBEntity
    {
        [Key]
        public int RecId { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please Select Booking Date")]
        public string BookingDate { get; set; }
        [Required(ErrorMessage = "Please Select Time Slot")]
        public int TimeSlotId { get; set; }
        [Required(ErrorMessage = "Please Select Department")]
        public int DeptId { get; set; }
        public string MACAddress { get; set; }
        public string DeviceInfo { get; set; }
        public string IPAddress { get; set; }
        public string SecureKey { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsExpired { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public int OTP { get; set; }
    }
}

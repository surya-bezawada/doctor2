using Microsoft.EntityFrameworkCore;
using shivam.Data.DBEntity;
using shivam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Data
{
    public class HospitalContext :DbContext
    {
       

        public HospitalContext (DbContextOptions<HospitalContext> options):base (options)
        {

        }
        public DbSet<BookingModel> tblBookings { get; set; }

        public DbSet<DepartmentModel> tblDeptMaster { get; set; }

        public DbSet<TimingDBEntity> Timings { get; set; }

        public DbSet<OtpdetailModel> tblOTPDetails { get; set; }



    }
}

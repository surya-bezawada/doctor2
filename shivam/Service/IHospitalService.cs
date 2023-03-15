using Microsoft.AspNetCore.Mvc.Rendering;
using shivam.Data.DBEntity;
using shivam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Service
{
    public interface IHospitalService
    {
        List<BookingModel> GetBookings();
       // List<DepartmentModel> GetDeptList();
         BookingModel GetbyId(int recId);
         BookingModel save(BookingDBEntity entity);
        DepartmentModel save(DepartmentModel departmentModel);
        DepartmentModel GetbydepId(int deptId);
       
        object ValidateDetails(int oTP, int recId, string secureKey);
        object ResendOtp(int recId, string secureKey);
        IEnumerable<TimingDBEntity> GetTimingList();
        SelectList GetDeptList();
    }
}

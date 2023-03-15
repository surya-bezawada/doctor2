using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shivam.Data.DBEntity;
using shivam.Models;
using shivam.Service;
using shivam.utlilty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shivam.Controllers
{
    [Route("api/Hospital")]
    [ApiController]
    public class HospitalController : ControllerBase
    {

       
      
        private readonly IHospitalService _hospital;

        public HospitalController(IHospitalService hospital)
        {
          this.  _hospital = hospital;
        }
      
        [HttpGet]
        [Route("GetBookings")]
        public List<BookingModel> GetBookings()
        {
            try
            {
                List<BookingModel> bookings = _hospital.GetBookings();

                return bookings;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpGet]
        [Route("GetDeptList")]
        public SelectList GetDeptList()
        {
           
                var res = _hospital.GetDeptList();

                return res;

        

        }

        [HttpGet]
        [Route("bookingbyid")]
        public  IActionResult GetbyId(int recId)
        {
            try
            {
                var result = _hospital.GetbyId(recId);
                if(result!=null)
                {
                    return Ok(result);
                }
                return NotFound() ;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("depbyid")]
        public IActionResult GetbydepId(int deptId)
        {
            try
            {
                var result = _hospital.GetbydepId(deptId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("save")]
        public IActionResult save(BookingDBEntity entity)
        {
            try
            {
               
                if (entity == null)
                {
                    return BadRequest();
                }
                var createdbooking = _hospital.save(entity);
                return CreatedAtAction(nameof(GetbyId), new { id = createdbooking.RecId}, createdbooking);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("saveDepartment")]
        public IActionResult save(DepartmentModel departmentModel)
        {
            try
            {
               
                if (departmentModel == null)
                {
                    return BadRequest();
                }
                var createdbooking = _hospital.save(departmentModel);
                return CreatedAtAction(nameof(GetbydepId), new { id = createdbooking.DeptId }, createdbooking);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getTimings")]
        public IEnumerable<TimingDBEntity> GetTiming()
        {
            var res = _hospital.GetTimingList();
            res = res.Where(x => (Convert.ToDateTime(x.Timing) > DateTime.Now)).ToList();

            //var a = new  SelectList(res, "TimeSlotId", "Timing");
            //return a;
            return res;
        }

       

        [HttpGet]
       [Route("validation")]
        public IActionResult ValidateDetails(int OTP, int RecId, string secureKey)
        {
            try
            {
                var result = _hospital.ValidateDetails(OTP,RecId,secureKey);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("resend")]
        public IActionResult ResendOtp( int RecId, string secureKey)
        {
            try
            {
                var result = _hospital.ResendOtp( RecId, secureKey);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

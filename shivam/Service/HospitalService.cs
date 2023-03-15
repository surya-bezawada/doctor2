using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shivam.Data;
using shivam.Data.DBEntity;
using shivam.Models;
using shivam.utlilty;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace shivam.Service
{
    public class HospitalService : IHospitalService
    {
        private readonly ISmsService _smsService;

        private readonly HospitalContext _context;

        public HospitalService(HospitalContext context, ISmsService smsService)
        {
            _context = context;
            _smsService = smsService;
        }


        public List<BookingModel> GetBookings()
        {
            return _context.tblBookings.ToList();
        }

        public DepartmentModel GetbydepId(int deptId)
        {
            return _context.tblDeptMaster.Find(deptId);
        }

        public BookingModel GetbyId(int recId)
        {
            return _context.tblBookings.Find(recId);
        }

        //public List<DepartmentModel> GetDeptList()
        //{
        //    var states = (from slist in _context.tblDeptMaster
        //                  select new { slist.DeptId, slist.DeptName }).ToList();
        //    return GetDeptList();
        //}


        public IEnumerable<TimingDBEntity> GetTimingList()
        {
            return _context.Timings.ToList();
        }

        public BookingModel save(BookingDBEntity entity)
        {
            Guid g = Guid.NewGuid();
            BookingModel model = new BookingModel()
            {

                BookingDate = entity.BookingDate,
                Name = entity.Name,
                Email = entity.Email,
                OTP = CommonUtils.GenerateOTP(),
                IsExpired = false,
                IPAddress = CommonUtils.GetUserIP(),
                CreatedOn = DateTime.Now,
                DeptId = entity.DeptId,
                SecureKey = g.ToString(),
                Status = false,
                DeviceInfo = entity.DeviceInfo,
                MACAddress = CommonUtils.GetMACAddress(),
                TimeSlotId = entity.TimeSlotId,
                RecId = entity.RecId,
                Mobile = entity.Mobile


            };
            //bookingModel.IPAddress = CommonUtils.GetUserIP();
            //bookingModel.MACAddress = CommonUtils.GetMACAddress();
            //bookingModel.OTP = CommonUtils.GenerateOTP();
            //bookingModel.CreatedOn = DateTime.Now;
            //bookingModel.IsExpired = false;
            //bookingModel.Status = false;
            //bookingModel.SecureKey = new Guid().ToString();
           

            _context.tblBookings.Add(model);
            _context.SaveChanges();

            _smsService.SendOTP(model);
            var res = new
            {
                recid = model.RecId,
                securekey = model.SecureKey,
                success = true,
                message = "OTP has been sent to your given mobile number " + model.Mobile.Substring(0, 3) + "XXXX " + model.Mobile.Substring(7, 3) + ". OTP is valid for 10 minutes only."

            };
            return model;
          



        }

        //public void sendsms(BookingDBEntity bookingentity)
        //{
       
        //}

        public DepartmentModel save(DepartmentModel departmentModel)
        {
            _context.tblDeptMaster.Add(departmentModel);
            _context.SaveChanges();
            return departmentModel;
        }

        public object ValidateDetails(int oTP, int recId, string secureKey)
        {
            try
            {
               
                    var model = _context.tblBookings.Where(x => x.RecId == recId && x.SecureKey == secureKey).FirstOrDefault();
                    if (model == null)
                    {
                        var res1 = new
                        {
                            code = 100,
                            status = true,
                            message = "Something went wrong: Record Not Found."
                        };
                        return res1;
                    }
                    if (model.IsExpired || model.Status)
                    {
                        var res11 = new
                        {
                            code = 100,
                            status = true,
                            message = "OTP already expired/used, please try with new OTP."
                        };
                        return res11;
                    }
                    System.DateTime currentTime = DateTime.Now;
                    var timeDiff = currentTime - model.CreatedOn;
                    TimeSpan span = model.CreatedOn.Subtract(currentTime);
                    if (span.Minutes > 10)
                    {
                        _smsService.UpdateOTPStatus(recId, true, false);
                        var res2 = new
                        {
                            code = 100,
                            status = true,
                            message = "OTP has been expired, please try again."
                        };
                        return res2;
                    }
                    if (model.OTP != oTP)
                    {
                        var res33 = new
                        {
                            code = 100,
                            status = true,
                            message = "Incorrect OTP Details."
                        };
                        return res33;
                    }
                    bool isValid = (model.OTP == oTP && !model.Status);
                    if (isValid)
                    {
                        _smsService.UpdateOTPStatus(recId, false, true);
                        _smsService.SendConfirmations(recId);
                        var res3 = new
                        {
                            code = 200,
                            status = true,
                            message = "You have booked an appointment."
                        };
                        return res3;
                    }
                    var res = new
                    {
                        code = 100,
                        status = false,
                        message = "Failed in Execultion"
                    };
                    return res;

                
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;

            }
            catch (DbUpdateException e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (var eve in e.Entries)
                {
                    sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                }
                var a = sb.ToString();

                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public object ResendOtp(int recId, string secureKey)
        {
            try
            {

               
                    var model = _context.tblBookings.Where(x =>x.RecId == recId && x.SecureKey == secureKey).FirstOrDefault();

                    if (model == null)
                    {
                        var res1 = new
                        {
                            code = 100,
                            status = true,
                            message = "Something went wrong: Record Not Found."
                        };
                        return res1;
                    }
                    _smsService.SendOTP(model);
                    var res = new
                    {
                        code = 200,
                        recid = model.RecId,
                        securekey = model.SecureKey,
                        success = true,
                        message = "OTP has been resent to your given mobile number " + model.Mobile.Substring(0, 4) + "XXXXXX. OTP is valid for 10 minutes only."
                    };
                    return res;
                
                

            }
            catch (Exception ex)
            {
                var res = new
                {
                    success = false,
                    message = "Caught Exception"
                };
                return res;
            }
        }


        private void SendOTP(BookingDBEntity booking)
        {
            string userMobile = booking.Mobile;
            string docMobile = string.Empty;
           
                docMobile = _context.tblDeptMaster.Where(x => x.DeptId == booking.DeptId)?.FirstOrDefault()?.MobileNumber;
            
            String message = HttpUtility.UrlEncode("OTP for your mobile number verification is 1212. The code will be valid for 10 minutes. %n Thanks - Shivam Clinic.");
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "PN5HT8T/bIA-QYyuiVHUE8qyXPr3Am0rVTadylMPIZ"},
                {"numbers" , docMobile},
                {"message" , message},
                {"sender" , "SSHVMM"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
            }
        }

        public SelectList GetDeptList()
        {
            var states = (from slist in _context.tblDeptMaster
                         select new { slist.DeptId, slist.DeptName }).ToList();
            return new SelectList(states, "DeptId", "DeptName");
        }
    }
}
           
            


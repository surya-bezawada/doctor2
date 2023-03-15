using shivam.Data;
using shivam.Data.DBEntity;
using shivam.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using static shivam.Enum.OTPENUMS;

namespace shivam.Service
{
    public class SmsService : ISmsService
    {
        private readonly HospitalContext _context;
        public SmsService(HospitalContext context)
        {
            _context = context;
        }
        public void SendConfirmations(int recId)
        {
            var data = (from b in _context.tblBookings
                        join d in _context.tblDeptMaster on b.DeptId equals d.DeptId
                        join t in _context.Timings on b.TimeSlotId equals t.TimeSlotId
                        where b.RecId == recId
                        select new { b.Name, b.Mobile, t.Timing, d.DeptName, d.MobileNumber }).FirstOrDefault();

            string userConfMsg = HttpUtility.UrlEncode("Thank you!%nyour appointment booked with " + data.DeptName + " with the timeslot " + data.Timing + "%nThanks - Shivam");
            string docConfMsg = HttpUtility.UrlEncode("You have an appointment for " + data.DeptName + " at the time " + data.Timing + " with " + data.Name + "%nThanks - Shivam");
            SendSMS(data.Mobile, (int)OTPType.UserConfirmation, userConfMsg, recId);
            SendSMS(data.MobileNumber, (int)OTPType.UserConfirmation, docConfMsg, recId);
        }

       

        public void UpdateOTPStatus(int recId, bool isExpired, bool status)
        {

            var data = _context.tblBookings.Find(recId);
            if (data != null)
            {
                data.IsExpired = isExpired;
                data.Status = status;
            }
            _context.SaveChanges();

        }






        private void SendSMS(string mobile, int type, string message, int bookingId = 0)
        {
            string APIKey = string.Empty;
            if (type == 1)
                APIKey = "PN5HT8T/bIA-QYyuiVHUE8qyXPr3Am0rVTadylMPIZ";
            else
                APIKey = "MekLk436AXg-cbhI43AbgPVUUTWsq4Dz702Y5mlK7q";
            using (var wb = new WebClient())
            {
                var queryParams = new NameValueCollection()
                {
                    {"apikey" , APIKey},
                    {"numbers" , mobile},
                    {"message" , message},
                    {"sender" , "SSHVMM"}
                };
                var response = wb.UploadValues("https://api.textlocal.in/send/", queryParams);
                string result = System.Text.Encoding.UTF8.GetString(response);

                SaveAPIResponse(mobile, type, ToQueryString(queryParams), result, bookingId);
            }
        }

        public string ToQueryString(NameValueCollection nameValueCollection)
        {
            NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(String.Empty);
            httpValueCollection.Add(nameValueCollection);
            return httpValueCollection.ToString();
        }

        private void SaveAPIResponse(string mobile, int type, string request, string response, int bookingId = 0)
        {
           OtpdetailModel otpDts = new OtpdetailModel()
            {
                Mobile = mobile,
                Type = type,
                APIRequest = request,
                APIResponse = response,
                BookingId = bookingId,
                CreatedOn = DateTime.Now

            };

            _context.tblOTPDetails.Add(otpDts);
            _context.SaveChanges();
           

        }

        public void SendOTP(BookingModel model)
        {
            string userMobile = model.Mobile;

            string message = HttpUtility.UrlEncode("OTP for your mobile number verification is " + model.OTP + ". The code will be valid for 10 minutes. Thanks - Shivam Clinic");
            SendSMS(userMobile, (int)OTPType.UserOTP, message, model.RecId);
        }
    }
    
}

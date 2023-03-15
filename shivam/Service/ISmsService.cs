using shivam.Data.DBEntity;
using shivam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shivam.Service
{
    public interface ISmsService
    {
       
        void UpdateOTPStatus(int recId, bool v1, bool v2);
        void SendConfirmations(int recId);
       
        void SendOTP(BookingModel model);
    }
}

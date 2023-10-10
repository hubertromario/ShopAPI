using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.ApplicationConstants
{
    public class ApplicationConstants
    {
    }


    public class CommonMessage
    {
        public const string createOperationSuccess = "Record Created Successfully";
        public const string updateOperationSuccess = "Record Updated Successfully";
        public const string deleteOperationSuccess = "Record Deleted Successfully";

        public const string createOperationFailed = "Create Operation Failed";
        public const string updateOperationFailed = "Update Operation Failed";
        public const string deleteOperationFailed = "Delete Operation Failed";

        public const string recordNotFound = "Record Not Found";
        public const string systemError = "Something Went Wrong";

        public const string registrationFailed = "user Registration has been failed";
        public const string registrationsuccess = "user Registration has been Successful";

        public const string loginFailed = "user login has been failed";
        public const string loginSuccess = "user login has been successful";
    }
}

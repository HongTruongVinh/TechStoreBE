using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechStore.Common.Constants
{
    public static class Messenger
    {
        public const string NoExitData = "No exit data";
        public const string ExitData = "Exit data";
        public const string SuccessFull = "Action is sucess";
        public const string UpdateSuccessFull = "Update data success";
        public const string GetDataSuccessful = "Get data from databse success";
        public const string BadRequest = "Bad request: The params is null or missing with the request";
        public const string SystemError = "The system error";
        public const string InvalidRefreshToken = "The refresh token is valid or expire for request";
        public const string CreateDataError = "Create data error";
        public const string LoginError = "Login to system error";
        public const string UpdateDataError = "Update data error";
        public const string EmailAlreadyExist = "The email exits! Please change email!";
        public const string PhonenNumberAlreadyExist = "The phone numer exits! Please change phone number!";
        public const string InvalidRegisterType = "Invalid register type!";
        public const string NotFoundUser = "Not found user on system! Please check again";
        public const string EmailNull = "The email is empty or null! Please check again";
        public const string EmailNotExit = "The email is not exit on system! Please check again";
        public const string LoginWithEmailGoogleNotSuccess = "Login to email is not successfull! Please check with admin";
        public const string LoginSuccessfull = "Login successfull!";
        public const string AdminLoginSuccessfull = "Hello Admin!";
        public const string IsNotAdmin = "You are not an Admin!";
        public const string NoPermission = "You have no permission to access!";
        public const string IncorrectDataFormat = "Incorrect data format!";
        public const string IncorrectAmount = "Incorrect amount!";
        public const string PaymentVerified = "Payment verified successfully!";
        public const string PaymentNotVerified = "Payment not verified!";
    }
}

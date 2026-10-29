using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PL_APP_MVC.Dtos
{
    public class OtpDto
    {
        public string EmailId {  get; set; }

        [ValidateNever]
        public int Otp {  get; set; }

        [ValidateNever]
        public string Message {  get; set; }
    }
}

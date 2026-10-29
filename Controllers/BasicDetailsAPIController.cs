using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Dtos;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;
using System.Collections;
using static System.Net.WebRequestMethods;

namespace PL_APP_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicDetailsAPIController : ControllerBase
    {
        private static Hashtable hashtable = new Hashtable();

        private readonly PlappContext _context;
        private readonly IMailService _mailService;
        public BasicDetailsAPIController(PlappContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        [HttpPost]
        [Route("GenerateOtp")]
        public ActionResult GenerateOtp(OtpDto otpDto)
        {
            //Generate 4 digit OTP randomly
            Random random = new Random();
            int otp = random.Next(1111, 9999);

            //save that otp into otpmaster table with emailid
            hashtable.Add(otpDto.EmailId, otp);

            string response = _mailService.SendEmail("LoanApp Admin", "sandeepvoleti1008@gmail.com",
                                                        "Customer", otpDto.EmailId,
                                                        "Otp Verification for Loan App",
                                                        "Here is your 4 digit Opt for Loan App Login Verification :  " + otp);
            otpDto.Message = "Opt Sent successfully";
            return Ok(otpDto);
        }

        [HttpPost]
        [Route("ValidateOtp")]
        public ActionResult ValidateOtp(OtpDto otpDto)
        {
            Basicdetail basicDetails = null;
            if (otpDto.Otp != 0 && otpDto.Otp == (int)hashtable[otpDto.EmailId])
            {
                //now that otp verification is successful, this is a valid customer
                //we need to generate lead id, and create a record for loan

                basicDetails = _context.Basicdetails.FirstOrDefault(details => details.Emailid == otpDto.EmailId);

                if (basicDetails == null)
                {
                    basicDetails = new Basicdetail();
                    basicDetails.Emailid = otpDto.EmailId;
                    basicDetails.Appstage = "OTP_VERIFIED";
                    basicDetails.Appstatus = "APPLICATION_ID_GENERATED";

                    _context.Basicdetails.Add(basicDetails);
                    _context.SaveChanges();
                }
            }
            basicDetails = _context.Basicdetails.FirstOrDefault(details => details.Emailid == otpDto.EmailId);
            RootDto rootDto = new RootDto();
            rootDto.Name = basicDetails.Firstname + " " + basicDetails.Lastname;
            rootDto.AppId = basicDetails.Id;
            rootDto.AppStage = basicDetails.Appstage;
            rootDto.AppStatus = basicDetails.Appstatus;
            rootDto.Message = "Success";

            return Ok(rootDto);
        }

        [HttpPost]
        [Route("ValidateBasicDetails")]
        public ActionResult ValidateBasicDetails(BasicDetailsDto basicDetails)
        {
            if (basicDetails == null
                || basicDetails.MobileNumber == null
                || basicDetails.FirstName == null
                || basicDetails.PanNumber == null
                || basicDetails.AadharNumber == null
                || basicDetails.Pincode == null
                || basicDetails.Dob == null
                )
            {
                return BadRequest("Required Fields are empty");

            }

            //validate pan number
            var panMaster = _context.Panmasters.Where(p => p.Pancardnumber == basicDetails.PanNumber).FirstOrDefault();
            if (panMaster == null)
            {
                return BadRequest("Invalid PAN Number");
            }
            //validate aadhar
            var aadharMaster = _context.Aadharmasters.Where(a => a.Aadharnumber == basicDetails.AadharNumber).FirstOrDefault();
            if (aadharMaster == null)
            {
                return BadRequest("Invalid Aadhar Number");
            }

            //validate pincode
            var pincodeMaster = _context.Pincodemasters.Where(p => p.Pincode == Convert.ToInt32(basicDetails.Pincode)).FirstOrDefault();
            if (pincodeMaster == null)
            {
                return BadRequest("Invalid Pincode");
            }

            //validate age based on dob -- assignment
            //identify dob and derive no of years from dob
            int age = 20;

            Rulesmaster rules = _context.Rulesmasters.FirstOrDefault(rule => rule.Rulename == "age");
            if (age < rules.Minvalue && age > rules.Maxvalue)
            {
                return BadRequest("Invalid Age");
            }


            //Save the details to CustomerBasicDetails table
            Basicdetail basicDetail = _context.Basicdetails.FirstOrDefault(details => details.Emailid == basicDetails.EmailId);

            basicDetail.Firstname = basicDetails.FirstName;
            basicDetail.Lastname = basicDetails.LastName;
            basicDetail.Pannumber = basicDetails.PanNumber;
            basicDetail.Aadharnumber = basicDetails.AadharNumber;
            basicDetail.Pincode = basicDetails.Pincode;
            basicDetail.Dob = DateOnly.FromDateTime(basicDetails.Dob);
            basicDetail.Mobile = basicDetails.MobileNumber;
            basicDetail.Leadid = basicDetail.Id + "-" + basicDetails.MobileNumber;
            basicDetail.Appstage = "BASIC_DETAILS";
            basicDetail.Appstatus = "APPLICATION_IN_PROGRESS";

            _context.Basicdetails.Update(basicDetail);
            _context.SaveChanges(); 
            
            basicDetails.AppStatus = basicDetail.Appstatus;
            basicDetails.AppStage = basicDetail.Appstage;
            basicDetails.AppId = basicDetail.Id;
            basicDetails.Message = "Basic Details Saved Successfully";
            basicDetails.Name = basicDetail.Firstname + " " + basicDetail.Lastname;
        
            return Ok(basicDetails);
        }
    }
}

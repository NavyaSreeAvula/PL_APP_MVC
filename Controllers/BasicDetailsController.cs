
using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Dtos;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;
using System.Collections;

namespace PL_APP_MVC.Controllers
{
    public class BasicDetailsController : Controller
    {
        private static Hashtable hashtable = new Hashtable();

        private readonly PlappContext _context;
        private readonly IMailService _mailService;
        public BasicDetailsController(PlappContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OtpVerification(string EmailId) 
        {
            //Generate 4 digit OTP randomly
            Random random = new Random();
            int otp = random.Next(1111,9999);
            
            //save that otp into otpmaster table with emailid
            hashtable.Add(EmailId,otp);

            string response = _mailService.SendEmail("LoanApp Admin", "sandeepvoleti1008@gmail.com", 
                                                        "Customer", EmailId, 
                                                        "Otp Verification for Loan App", 
                                                        "Here is your 4 digit Opt for Loan App Login Verification :  "+ otp);
            if (response == "OK")
            {
                ViewBag.EmailId = EmailId;
                return View("OtpVerification");
            }
            else {

                return View("Index");
            }

        }
        public IActionResult ValidateOtp(int otp, string Emailid) 
        {
            //instead of hardcoded otp, check the received otp with the one in db

            if (otp != 0 && otp == (int) hashtable[Emailid]) {

                //now that otp verification is successful, this is a valid customer
                //we need to generate lead id, and create a record for loan
                Basicdetail basicDetails = null;
                
                basicDetails = _context.Basicdetails.FirstOrDefault(details=> details.Emailid == Emailid);
                if (basicDetails == null)
                {
                    basicDetails = new Basicdetail();
                    basicDetails.Emailid = Emailid;
                    basicDetails.Appstage = "OTP_VERIFIED";
                    basicDetails.Appstatus = "APPLICATION_ID_GENERATED";

                    _context.Basicdetails.Add(basicDetails);
                    _context.SaveChanges();
                    ViewBag.Emailid = Emailid;
                    return View("BasicDetails");
                }
                else {
                    //if basicdetails != null means he is an existing customer.
                    //we should route him to the current screen as per his journey
                    string mapping = StageViewMapper.GetView(basicDetails.Appstage);
                    string[] words = mapping.Split('/'); //[0]:controllername, [1]:actionname
                                                        //Company/Index/1
                    return RedirectToAction(words[1], words[0], new { id= basicDetails.Id});
                }                
            }

            ViewBag.msg = "Wrong OTP";
            return View("OtpVerification");
        }

        public IActionResult ValidateBasicDetails(BasicDetailsDto basicDetails)
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
                TempData["Error"] = "All fields are required";
                return RedirectToAction("index");
                
            }

            //validate pan number
            var panMaster = _context.Panmasters.Where(p => p.Pancardnumber == basicDetails.PanNumber).FirstOrDefault();
            if (panMaster == null) {
                TempData["Error"] = "Invalid Pan Number";
                return RedirectToAction("index");
            }
            //validate aadhar
            var aadharMaster = _context.Aadharmasters.Where(a => a.Aadharnumber == basicDetails.AadharNumber).FirstOrDefault();
            if (aadharMaster == null) {
                TempData["Error"] = "Invalid Aadhar Number";
                return RedirectToAction("index");
            }

            //validate pincode
            var pincodeMaster = _context.Pincodemasters.Where(p => p.Pincode == Convert.ToInt32(basicDetails.Pincode)).FirstOrDefault();
            if (pincodeMaster == null) {
                TempData["Error"] = "Invalid Pincode";
                return RedirectToAction("Index");
            }

            //validate age based on dob -- assignment
            //identify dob and derive no of years from dob
            int age=20;

            Rulesmaster rules = _context.Rulesmasters.FirstOrDefault(rule => rule.Rulename == "age");
            if (age < rules.Minvalue && age > rules.Maxvalue)
            {
                TempData["Error"] = "Invalid Age";
                return RedirectToAction("Index");
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

            string mapping = StageViewMapper.GetView(basicDetail.Appstage);
            string[] words = mapping.Split('/');
            return  RedirectToAction(words[1], words[0]);
        }
    }
}

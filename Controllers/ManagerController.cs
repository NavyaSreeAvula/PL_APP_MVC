using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Models;

namespace PL_APP_MVC.Controllers
{
    public class ManagerController : Controller
    {
        private readonly PlappContext _context;

        public ManagerController(PlappContext context) { 
            _context = context;
        }
        public IActionResult Index()
        {
            List<Basicdetail> appList = _context.Basicdetails.Where(app=>app.Appstatus== "APPLICATION_IN_PROGRESS").ToList();
            
            return View(appList);
        }
        public IActionResult ViewApplication(int id) {

            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == id);
            Companydetail companydetail = _context.Companydetails.FirstOrDefault(company => company.AppId == id);
            Loandetail loandetail = _context.Loandetails.FirstOrDefault(loan => loan.Appid == id);
            Personaldetail personaldetail = _context.Personaldetails.FirstOrDefault(personal => personal.Appid == id);
            Bankdetail bankdetail = _context.Bankdetails.FirstOrDefault(bank => bank.Appid == id);
            Docuploaddetail docuploaddetail = _context.Docuploaddetails.FirstOrDefault(doc => doc.Appid == id); 

            ViewBag.Id = id;
            ViewBag.Basicdetail = basicdetail;
            ViewBag.Companydetail = companydetail;
            ViewBag.Loandetail = loandetail;
            ViewBag.Personaldetail = personaldetail;
            ViewBag.Bankdetail = bankdetail;
            ViewBag.Docuploaddetail = docuploaddetail;

            return View();
        }

        public IActionResult UpdateLoanStatus(int id, int ApprovedLoanAmt, int ApprovedTenure, decimal ApprovedRoi) {

            int years = ApprovedTenure / 12;
            decimal roi_perAnnam = (ApprovedLoanAmt * ApprovedRoi) / 100;
            decimal totalroi = roi_perAnnam * years;
            decimal emi = (totalroi + ApprovedLoanAmt) / ApprovedTenure;

            
            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == id);
            basicdetail.Appstatus = "LOAN_APPROVED";

            Loandetail loandetail = _context.Loandetails.FirstOrDefault(loan => loan.Appid == id);
            loandetail.Approvedamount = ApprovedLoanAmt;
            loandetail.Approvedtenure = ApprovedTenure;
            loandetail.Approvedroi = ApprovedRoi;
            loandetail.Approvedemi = Math.Round(emi,2);
            

            _context.Loandetails.Update(loandetail);
            _context.Basicdetails.Update(basicdetail);

            _context.SaveChanges();

            //send mail to customer

            return RedirectToAction("Index");
        }
    }
}

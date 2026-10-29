using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Dtos;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;

namespace PL_APP_MVC.Controllers
{
    public class LoanDetailsController : Controller
    {
        private readonly PlappContext _context;

        public LoanDetailsController(PlappContext context) { 
            _context = context;
        }
        public IActionResult Index(int id)
        {
            List<Rulesmaster> rulesmasters = _context.Rulesmasters.ToList();
            Companydetail companyDetail = _context.Companydetails.FirstOrDefault(company=> company.AppId == id);
            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic=> basic.Id == id);
            var today = DateTime.Today;
            string[] dates = basicdetail.Dob.ToString().Split("/");

            int age = today.Year - Int32.Parse(dates[2]);
            int grossIncome = (int)companyDetail.Grossincome;
            int obligations = (int)companyDetail.Obligations;

            LoanCalculatorService service = new LoanCalculatorService();
            LoanDetailsDto dto = service.CalculateLoanDetails(age, grossIncome, obligations, rulesmasters);
            dto.AppId = id;
            return View(dto);//Model binding
        }

        public IActionResult SaveLoanDetails(LoanDetailsDto loanDto) {

            Loandetail loandetail = _context.Loandetails.FirstOrDefault(loan => loan.Appid == loanDto.AppId);
            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == loanDto.AppId);

            Boolean isExistingCust = true;
            if (loandetail == null) {
                isExistingCust = false;
                loandetail = new Loandetail();
            }

            loandetail.Appid = loanDto.AppId;
            loandetail.Appliedamount = (int)loanDto.MaxLoanAmount;
            loandetail.Appliedtenure = loanDto.Tenure;
            loandetail.Roi = (decimal)loanDto.Roi;
            loandetail.Emi = loanDto.Emi;

            basicdetail.Appstage = "LOAN_DETAILS";
            
            if (isExistingCust)
            {
                _context.Loandetails.Update(loandetail);
                _context.Basicdetails.Update(basicdetail);
            }
            else { 
                _context.Loandetails.Add(loandetail);
                _context.Basicdetails.Update(basicdetail);
            }
            _context.SaveChanges();


            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
    }
}

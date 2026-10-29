using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Dtos;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;

namespace PL_APP_MVC.Controllers
{
    public class CompanyDetailsController : Controller
    {

        private readonly PlappContext _context;

        public CompanyDetailsController(PlappContext context) { 
            _context = context;
        }

        public IActionResult Index(int id)//to be saved as fk in companyDetails
        {
            ViewBag.Appid = id;
            return View();
        }

        public IActionResult SaveCompanyDetails(CompanyDetailsDto companyDto)
        {
            bool isNewCustomer = false;
            bool isRejected = false;
            Rulesmaster rules_income = _context.Rulesmasters.FirstOrDefault(rule=>rule.Rulename == "income");
            Rulesmaster rules_obligation = _context.Rulesmasters.FirstOrDefault(rule=> rule.Rulename == "oblpercent");
            Companymaster companymaster = _context.Companymasters.FirstOrDefault(company=>company.Companyname == companyDto.Companyname);

            Companydetail companydetail = _context.Companydetails.FirstOrDefault(company=> company.AppId == companyDto.AppId);
            if(companydetail == null)
            {
                companydetail = new Companydetail();
                isNewCustomer = true;
            }
                

            //set all the values received from companyDto
            companydetail.Companyname = companyDto.Companyname;
            companydetail.AppId = companyDto.AppId;
            companydetail.Companymailid = companyDto.Companymailid;
            companydetail.Companyaddress = companyDto.Companyaddress;
            companydetail.Grossincome = companyDto.Grossincome;
            companydetail.Obligations = companyDto.Obligations;

            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == companyDto.AppId);
            

            //1. validate income rule :: if not met, reject customer :: save basic details db with 
            if (companyDto.Grossincome <= rules_income.Minvalue ||
                companyDto.Grossincome >= rules_income.Maxvalue) 
            {   
                isRejected = true;
                basicdetail.Statusremarks = "INCOME_RULE_FAILED";                
            }

            //2. validate obligation rule :: if not met, reject customer
            int obglPercent = (int)(companyDto.Obligations * 100) / companyDto.Grossincome;
            
            if (obglPercent > rules_obligation.Maxvalue) {
                //reject the customer
                isRejected = true;
                basicdetail.Statusremarks = "OBLIGATION_RULE_FAILED";
            }

            //3. validate company category to decide the roi
            if (companymaster == null)
            {
                companydetail.Category = "Others";
            }
            else { 
                companydetail.Category = companymaster.Category;
            }

            if (isRejected) {

                basicdetail.Appstatus = "APPLICATION_REJECTED";
                basicdetail.Appstage = "COMPANY_DETAILS";

                if (isNewCustomer)
                {
                    _context.Companydetails.Add(companydetail);

                }
                else
                {
                    _context.Companydetails.Update(companydetail);
                }

                _context.Basicdetails.Update(basicdetail);
                _context.SaveChanges();

                return View("Rejected/Thankyou");
            }

            //4. save details into db and redirect to next screen
            else
            {
                basicdetail.Appstage = "COMPANY_DETAILS";
                if (isNewCustomer)
                {
                    _context.Companydetails.Add(companydetail);

                }
                else
                {
                    _context.Companydetails.Update(companydetail);
                }
                _context.Basicdetails.Update(basicdetail);
                _context.SaveChanges();

                string mapping = StageViewMapper.GetView(basicdetail.Appstage);
                string[] words = mapping.Split('/'); //[0]:controllername, [1]:actionname
                                                     //Company/Index/1
                return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
            }           

        }
    }
}

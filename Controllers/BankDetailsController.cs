using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Dtos;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;

namespace PL_APP_MVC.Controllers
{
    public class BankDetailsController : Controller
    {
        private readonly PlappContext _context;
        public BankDetailsController(PlappContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == id);
            var bankdetail = _context.Bankdetails.FirstOrDefault(bank => bank.Appid == id);

            BankDetailsDto dto = new BankDetailsDto();
            dto.AppId = id;
            return View(dto);
        }

        public IActionResult SaveBankDetails(BankDetailsDto bankDetailsDto)
        {
            var bankdetail = _context.Bankdetails.FirstOrDefault(bank => bank.Appid == bankDetailsDto.AppId);
            var basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == bankDetailsDto.AppId);

            if (bankDetailsDto == null || bankDetailsDto.Bankname == null || bankDetailsDto.Bankbranch == null || 
                bankDetailsDto.Accnumber == null || bankDetailsDto.Acctype == null || bankDetailsDto.Accholdername == null 
                || bankDetailsDto.Ifsccode == null)
            {
                TempData["Error"] = "All fields are required";
                return RedirectToAction("index");
            }

            //Validate Bank IFSC Code
            var ifscValidate = _context.Bankdetails.FirstOrDefault(ifsc => ifsc.Ifsccode == bankDetailsDto.Ifsccode);
            if (ifscValidate == null)
            {
                TempData["Error"] = "Invalid IFSC Code";
                return RedirectToAction("index");
            }

            //Validate Bank Account Number
            var accValidate = _context.Bankdetails.FirstOrDefault(acc => acc.Accnumber == bankDetailsDto.Accnumber);
            if (accValidate == null)
            {
                TempData["Error"] = "Invalid Account Number";
                return RedirectToAction("index");
            }

            basicdetail.Appstage = "BANK_DETAILS";

            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0]);
        }
    }
}

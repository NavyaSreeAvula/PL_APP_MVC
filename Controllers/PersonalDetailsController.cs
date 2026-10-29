using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL_APP_MVC.Dtos;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;

namespace PL_APP_MVC.Controllers
{
    public class PersonalDetailsController : Controller
    {
        private readonly PlappContext _context;
        public PersonalDetailsController(PlappContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            Personaldetail personaldetail = _context.Personaldetails.FirstOrDefault(personal => personal.Appid == id);
            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == id);

            PersonalDetailsDto dto = new PersonalDetailsDto();
            dto.AppId = id;
            return View(dto);
        }

        public IActionResult SavePersonalDetails(PersonalDetailsDto personalDetailsDto)
        {
            Personaldetail personaldetail = _context.Personaldetails.FirstOrDefault(personal => personal.Appid == personalDetailsDto.AppId);
            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == personalDetailsDto.AppId);

            Boolean isExistingPrsn = true;
            if (personaldetail == null)
            {
                isExistingPrsn = false;
                personaldetail = new Personaldetail();
            }

            personaldetail.Appid = personalDetailsDto.AppId;
            personaldetail.Fathername = personalDetailsDto.Fathername;
            personaldetail.Mothername = personalDetailsDto.Mothername;
            personaldetail.Permanentaddress = personalDetailsDto.Permanentaddress;
            personaldetail.Currentaddress = personalDetailsDto.Currentaddress;
            personaldetail.Reference1name = personalDetailsDto.Reference1name;
            personaldetail.Reference1email = personalDetailsDto.Reference1email;
            personaldetail.Reference1mobile = personalDetailsDto.Reference1mobile;
            personaldetail.Reference1relation = personalDetailsDto.Reference1relation;
            personaldetail.Reference2name = personalDetailsDto.Reference2name;
            personaldetail.Reference2email = personalDetailsDto.Reference2email;
            personaldetail.Reference2mobile = personalDetailsDto.Reference2mobile;
            personaldetail.Reference2relation = personalDetailsDto.Reference2relation;


            basicdetail.Appstage = "PERSONAL_DETAILS";

            if (isExistingPrsn)
            {
                _context.Personaldetails.Update(personaldetail);
                _context.Basicdetails.Update(basicdetail);
            }
            else
            {
                _context.Personaldetails.Add(personaldetail);
                _context.Basicdetails.Update(basicdetail);
            }
            _context.SaveChanges();


            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }

    }
}

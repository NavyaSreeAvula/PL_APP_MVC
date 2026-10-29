using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Models;
using PL_APP_MVC.Services;

namespace PL_APP_MVC.Controllers
{
    public class DocUploadController : Controller
    {
        private readonly PlappContext _context;
        public DocUploadController(PlappContext context) { 
            _context = context;
        }
        public IActionResult Index(int id)
        {
            List<Docuploaddetail> docList = _context.Docuploaddetails.ToList();
            ViewBag.Id = id;
            return View(docList);
        }

        public IActionResult AddNewDocument(int id) {

            ViewBag.Id = id;
            return View();
        }
        public IActionResult UploadDocument(IFormFile document, int id, string docType)
        {
            // Validate that the application exists in basicdetails
            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == id);
            if (basicdetail == null)
            {
                return NotFound($"Application with ID {id} not found");
            }

            string UploadPath = "E:\\sandeep\\PL_APP_MVC\\Documents\\" + id+ "\\" + document.FileName;

            //saving document to server folder
            string directoryPath = Path.GetDirectoryName(UploadPath);
            Directory.CreateDirectory(directoryPath);

            var stream = new FileStream(UploadPath, FileMode.Create);
            document.CopyTo(stream);

            //saving uploaded document path to db
            Docuploaddetail details = new Docuploaddetail();
            details.Doctype = docType;
            details.Docpath = UploadPath;
            details.Appid = id;
            _context.Docuploaddetails.Add(details);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult SaveChanges(int id) {

            Basicdetail basicdetail = _context.Basicdetails.FirstOrDefault(basic => basic.Id == id);
            basicdetail.Appstage = "DOC_UPLOADED";

            _context.Basicdetails.Update(basicdetail);
            _context.SaveChanges();

            string mapping = StageViewMapper.GetView(basicdetail.Appstage);
            string[] words = mapping.Split('/');
            return RedirectToAction(words[1], words[0], new { id = basicdetail.Id });
        }
    }
}

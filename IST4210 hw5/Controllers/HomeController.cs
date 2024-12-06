using IST4210_hw5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Diagnostics;

namespace IST4210_hw5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            ViewBag.IsLoggedIn = false;
            _logger = logger;
        }

        public IActionResult Index(string? errorMessage = "")
        {
            ViewBag.IsLoggedIn = false;
            var loginModel = new Models.Login() { AuthenticationError = errorMessage };
            return View(loginModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult NewUser(string firstName, string lastName, string email, string password,
                                  string gender, string height, string dept, string major)
        {
            var hashedPassword = PasswordOneWayHash.GetHash(password);
            DatabaseHelper.InsertNew(firstName, lastName, email, hashedPassword, gender, int.Parse(height), dept, major);
            return Json(new { Message = $"User {firstName} created sucessfully!" });
        }
        [HttpGet]
        public IActionResult Enrollment()
        {
            ViewBag.IsLoggedIn = true;
            var students = DatabaseHelper.GetStudents();
            List<Student> studentWithEnrollmentInfo = UpdateEnrollment(students);
            return View(studentWithEnrollmentInfo);
        }

        private static List<Student> UpdateEnrollment(IEnumerable<Student> students)
        {
            var studentWithEnrollmentInfo = new List<Student>();
            foreach (var student in students)
            {
                student.EnrollmentStatus = DatabaseHelper.CheckStudent(student.StudentId) ? "Enrolled" : "Not Enrolled";
                studentWithEnrollmentInfo.Add(student);
            }

            return studentWithEnrollmentInfo;
        }

        [HttpPost]
        public IActionResult Enrollment(Student pageModel)
        {
            DatabaseHelper.InsertEnrollment(pageModel.StudentId, "Fall", "2024");
            ViewBag.IsLoggedIn = true;
            var students = DatabaseHelper.GetStudents();
            var studentsWithEnrollment = UpdateEnrollment(students);
            return View(studentsWithEnrollment);
        }
        public IActionResult StudentHome(Models.Login model)
        {
            if (model == null)
            {
                return Index();
            }
            var loggedInStudent = DatabaseHelper.GetStudent(model.UserName.ToLower());
            if (loggedInStudent == null)
            {
                return RedirectToAction("Index", new { errorMessage = "Invalid UserName" });
            }
            bool isCorrectPassword = loggedInStudent.Password == model.Password;
            if (!isCorrectPassword)
            {
                return RedirectToAction("Index", new { errorMessage = "Invalid Password" });

            }
            ViewBag.IsLoggedIn = true;
            return View();
        }
        public IActionResult Apply()
        {
            ViewBag.IsLoggedIn = true;

            return View();
        }
        [HttpPost]
        public IActionResult Apply(string firstName, string lastName, string email,
            string address1, string address2, string city, string state, string phone, string resume)
        {
            using var resumeFile = System.IO.File.OpenRead(resume);
            byte[] holder = new byte[resume.Length];
            resumeFile.Read(holder, 0, holder.Length);

            string[] resumeContent = System.IO.File.ReadAllLines(resume);
            ViewData["Resume"] = resumeContent;

            ViewBag.IsLoggedIn = true;

            return Json(new { name = resumeContent[0] });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

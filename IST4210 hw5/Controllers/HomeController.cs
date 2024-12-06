using IST4210_hw5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IST4210_hw5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseHelper _databaseHelper;

        public HomeController(ILogger<HomeController> logger, DatabaseHelper databaseHelper)
        {
            _logger = logger;
            _databaseHelper = databaseHelper;
            ViewBag.IsLoggedIn = false;
        }

        // Login Page
        public IActionResult Index(string errorMessage = "")
        {
            ViewBag.IsLoggedIn = false;
            var loginModel = new logIn { AuthenticationError = errorMessage };
            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult NewUser(string firstName, string lastName, string email, string password, string gender,
            int height, string dept, string major)
        {
           // var hashedPassword = PasswordOneWayHash.GetHash(password);
            _databaseHelper.InsertStudent(firstName, lastName, email, password, gender, height, dept, major );
            return Json(new { Message = $"User {firstName} created successfully!" });
        }

        // Enrollment Page (GET)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Enrollment()
        {
            ViewBag.IsLoggedIn = true;
            var students = _databaseHelper.GetStudents();
            var studentsWithEnrollmentInfo = UpdateEnrollmentStatus(students);
            return View(studentsWithEnrollmentInfo);
        }

        // Enrollment Page (POST)
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Enrollment(Student pageModel)
        {
            _databaseHelper.InsertEnrollment(pageModel.StudentId, "Fall", "2024");
            ViewBag.IsLoggedIn = true;

            var students = _databaseHelper.GetStudents();
            var studentsWithEnrollmentInfo = UpdateEnrollmentStatus(students);

            return View(studentsWithEnrollmentInfo);
        }

        private List<Student> UpdateEnrollmentStatus(IEnumerable<Student> students)
        {
            var studentsWithEnrollmentInfo = new List<Student>();
            foreach (var student in students)
            {
                student.EnrollmentStatus = _databaseHelper.CheckEnrollment(student.StudentId) ? "Enrolled" : "Not Enrolled";
                studentsWithEnrollmentInfo.Add(student);
            }
            return studentsWithEnrollmentInfo;
        }

        // Student Home Page
        public IActionResult StudentHome(logIn model)
        {
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var loggedInStudent = _databaseHelper.GetStudent(model.Username.ToLower());
            if (loggedInStudent == null)
            {
                return RedirectToAction(nameof(Index), new { errorMessage = "Invalid username!" });
            }

            bool isPasswordCorrect = loggedInStudent.Password == model.Password;
            if (!isPasswordCorrect)
            {
                return RedirectToAction(nameof(Index), new { errorMessage = "Invalid password!" });
            }

            ViewBag.IsLoggedIn = true;
            ViewBag.StudentName = $"{loggedInStudent.FirstName} {loggedInStudent.LastName}";
            return View(loggedInStudent); // Pass logged-in student to the view for personalized content
        }

        // Apply Page (GET)
        public IActionResult Apply()
        {
            ViewBag.IsLoggedIn = true;
            return View();
        }

        // Apply Page (POST)
        [HttpPost]
        public IActionResult Apply(string firstName, string lastName, string email, string address1, string address2,
            string city, string state, string phone, string resumePath)
        {
            var resumeContent = System.IO.File.ReadAllLines(resumePath);
            ViewData["Resume"] = resumeContent;
            ViewBag.IsLoggedIn = true;

            // Optionally save application data to the database
            return Json(new { Message = "Application submitted successfully!" });
        }

        // Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

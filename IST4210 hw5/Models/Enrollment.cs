using Microsoft.AspNetCore.Mvc;

namespace IST4210_hw5.Models
{
    public class Enrollment : Controller
    {
        public IEnumerable<Student> Students { get; set; } = Enumerable.Empty<Student>();

        public int SelectedStudents { get; set; } 
    }
    }


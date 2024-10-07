using Microsoft.AspNetCore.Mvc;

namespace SchoolConnect_Web_App.Controllers
{
    public class PrincipalController : Controller
    {
        public IActionResult PrincipalLandingPage()
        {
            return View();
        }
        public IActionResult PrincipalViewProfile()
        {
            return View();
        }

		public IActionResult PrincipalMakeAnnouncements()
		{
			return View();

		}

		public IActionResult PrincipalViewAnnouncements()
		{
			return View();
		}


		public IActionResult PrincipalViewGrades()
		{
			return View();
		}

        public IActionResult PrincipalGradeLandingPage()
        {
            return View();
        }

        public IActionResult PrincipalViewAttendance()
        {
            return View();
        }

        public IActionResult PrincipalLearnerProfile()
        {
            return View();
        }

        public IActionResult PrincipalViewReports()
        {
            return View();
        }


    }
}

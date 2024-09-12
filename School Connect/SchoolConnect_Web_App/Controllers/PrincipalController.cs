using Microsoft.AspNetCore.Mvc;

namespace schoolconnect.Controllers
{
    public class PrincipalController : Controller
    {
        public IActionResult PrincipalLandingPage()
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

        public IActionResult PrincipalViewReports()
        {
            return View();
        }


    }
}

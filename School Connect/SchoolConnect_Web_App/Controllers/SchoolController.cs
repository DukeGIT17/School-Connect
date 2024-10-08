using Microsoft.AspNetCore.Mvc;
using SchoolConnect_DomainLayer.Models;
using SchoolConnect_Web_App.IServices;

namespace SchoolConnect_Web_App.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> ListOfSchools()
        {
            var result = await _schoolService.GetSchools();
            var success = result.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success key.");
            

            if ((bool)success)
            {
                var schools = result.GetValueOrDefault("Result") ?? throw new Exception("Could not find the Result key.");
                return View(schools);
            }
                

            throw new Exception(result.GetValueOrDefault("ErrorMessage")!.ToString());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Create(School school)
        {
            if (ModelState.IsValid)
            {
                school.DateRegistered = DateTime.Now;
                var result = await _schoolService.RegisterSchoolAsync(school);
                var success = result.GetValueOrDefault("Success") ?? throw new Exception("Could not find the Success key.");

                if ((bool)success)
                    return RedirectToAction(nameof(ListOfSchools));

                ModelState.AddModelError(string.Empty, result.GetValueOrDefault("ErrorMessage")!.ToString()!);
                return View("Register", school);
            }
            return View("Register", school);
        }
    }
}

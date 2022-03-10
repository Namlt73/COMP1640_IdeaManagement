using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COMP1640_IdeaManagement.Models;
using COMP1640_IdeaManagement.Data;

namespace COMP1640_IdeaManagement.Controllers
{
    public class AcademicYearController : Controller
    {

        // Idea
        public IActionResult ListIdea()
        {
            return View();
        }

        public IActionResult CreateIdea()
        {
            return View();
        }

        public IActionResult DetailIdea()
        {
            return View();
        }

        public IActionResult UpdateIdea()
        {
            return View();
        }

        public IActionResult DeleteIdea()
        {
            return View();
        }


        //Category

        public IActionResult ListCategory()
        {
            return View();
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        public IActionResult DetailCategory()
        {
            return View();
        }

        public IActionResult UpdateCategory()
        {
            return View();
        }

        public IActionResult DeleteCategory()
        {
            return View();
        }
    }
}

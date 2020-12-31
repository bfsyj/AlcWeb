using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alcEntity;
using alcService;
using Microsoft.AspNetCore.Mvc;

namespace alcWeb.Controllers
{
    public class EngineeringCaseController : Controller
    {
        private readonly MyContext _context;
        public EngineeringCaseController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            List<EngineeringCase> list = _context.EngineeringCase.Where(p => p.isShow == 1).OrderByDescending(p => p.date).ToList();
            foreach (var model in list)
            {
                model.content ="";
            }
            return View(list);
        }

        public IActionResult Detail(string id)
        {
            EngineeringCase model = new EngineeringCase();
            if (!string.IsNullOrEmpty(id))
            {
                model = _context.EngineeringCase.Find(id);
                model.lookNumber++;
                _context.EngineeringCase.Update(model);
                _context.SaveChanges();
            }

            return View(model);
        }
    }
}

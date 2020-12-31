using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alcEntity;
using alcService;
using Microsoft.AspNetCore.Mvc;

namespace alcWeb.Controllers
{
    public class IntroductionController : Controller
    {
        private readonly MyContext _context;
        public IntroductionController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            List<Introduction> list = _context.Introduction.Where(p => p.isShow == 1).OrderBy(p => p.orderNumber).ToList();
            return View(list);
        }
    }
}

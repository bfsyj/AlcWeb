using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alcEntity;
using alcService;
using Microsoft.AspNetCore.Mvc;

namespace alcWeb.Controllers
{
    public class EnvironmentFilesController : Controller
    {
        private readonly MyContext _context;
        public EnvironmentFilesController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            List<EnvironmentFiles> list = _context.EnvironmentFiles.Where(p => p.isShow == 1).OrderByDescending(p => p.date).ToList();
            foreach(var item in list)
            {
                item.content = "";
            }
            return View(list);
        }

        public IActionResult Detail(string id)
        {
            EnvironmentFiles model = new EnvironmentFiles();
            if (!string.IsNullOrEmpty(id))
            {
                model = _context.EnvironmentFiles.Find(id);
                model.lookNumber++;
                _context.EnvironmentFiles.Update(model);
                _context.SaveChanges();
            }

            return View(model);
        }

        public void DownFile(string id)
        {
            EnvironmentFiles model = new EnvironmentFiles();
            if (!string.IsNullOrEmpty(id))
            {
                model = _context.EnvironmentFiles.Find(id);
                model.downNumber++;
                _context.EnvironmentFiles.Update(model);
                _context.SaveChanges();
            }
        }
    }
}

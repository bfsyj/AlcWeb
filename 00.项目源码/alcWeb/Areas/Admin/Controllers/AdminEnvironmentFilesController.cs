using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using alcCommon;
using alcEntity;
using alcService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace alcWeb.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminEnvironmentFilesController : Controller
    {
        private readonly MyContext _context;
        public AdminEnvironmentFilesController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            EnvironmentFiles model = new EnvironmentFiles();
            model.isShow = 1;
            if (!string.IsNullOrEmpty(id))
                model = _context.EnvironmentFiles.Find(id);
            return View(model);
        }

        public string Delete(string id)
        {
            var environmentFiles = _context.EnvironmentFiles.Find(id);
            _context.EnvironmentFiles.Remove(environmentFiles);
            if (_context.SaveChanges() > 0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }
        }

        public string Save(EnvironmentFiles environmentFiles)
        {
            if (string.IsNullOrEmpty(environmentFiles.id))
            {
                environmentFiles.id = Guid.NewGuid().ToString();
                environmentFiles.lookNumber = 0;
                environmentFiles.editUser = "管理员";
                environmentFiles.editTime = DateTime.Now;
                _context.Add(environmentFiles);
            }
            else
            {
                _context.Update(environmentFiles);
            }

            if (_context.SaveChanges() > 0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }

        }

        public string GetListByPage(string keyWord, int pageIndex, int pageSize)
        {
            keyWord = HttpUtility.UrlDecode(keyWord);

            int count;
            List<EnvironmentFiles> list;
            if (string.IsNullOrEmpty(keyWord))
            {
                count = _context.EnvironmentFiles.Count();
                list = _context.EnvironmentFiles.OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            else
            {
                count = _context.EnvironmentFiles.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).Count();
                list = _context.EnvironmentFiles.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            TableModel<EnvironmentFiles> response = new TableModel<EnvironmentFiles>(list, count);
            return JsonConvert.SerializeObject(response);
        }
    }
}

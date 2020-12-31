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
    public class AdminEngineeringCaseController : Controller
    {
        private readonly MyContext _context;
        public AdminEngineeringCaseController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            EngineeringCase model = new EngineeringCase();
            model.isShow = 1;
            if (!string.IsNullOrEmpty(id))
                model = _context.EngineeringCase.Find(id);
            return View(model);
        }

        public string Delete(string id)
        {
            var engineeringCase = _context.EngineeringCase.Find(id);
            _context.EngineeringCase.Remove(engineeringCase);
            if (_context.SaveChanges() > 0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }
        }

        public string Save(EngineeringCase engineeringCase)
        {
            if (string.IsNullOrEmpty(engineeringCase.id))
            {
                engineeringCase.id = Guid.NewGuid().ToString();
                engineeringCase.lookNumber = 0;
                engineeringCase.editUser = "管理员";
                engineeringCase.editTime = DateTime.Now;
                _context.Add(engineeringCase);
            }
            else
            {
                _context.Update(engineeringCase);
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
            List<EngineeringCase> list;
            if (string.IsNullOrEmpty(keyWord))
            {
                count = _context.EngineeringCase.Count();
                list = _context.EngineeringCase.OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            else
            {
                count = _context.EngineeringCase.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).Count();
                list = _context.EngineeringCase.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            TableModel<EngineeringCase> response = new TableModel<EngineeringCase>(list, count);
            return JsonConvert.SerializeObject(response);
        }
    }
}

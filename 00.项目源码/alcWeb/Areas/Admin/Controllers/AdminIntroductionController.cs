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
    public class AdminIntroductionController : Controller
    {
        private readonly MyContext _context;
        public AdminIntroductionController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            Introduction model = new Introduction();
            model.isShow = 1;
            if (!string.IsNullOrEmpty(id))
                model = _context.Introduction.Find(id);
            return View(model);
        }

        public string Delete(string id)
        {
            var introduction = _context.Introduction.Find(id);
            _context.Introduction.Remove(introduction);
            if (_context.SaveChanges() > 0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }
        }

        public string Save(Introduction introduction)
        {
            if (string.IsNullOrEmpty(introduction.id))
            {
                introduction.id = Guid.NewGuid().ToString();
                introduction.editUser = "管理员";
                introduction.editTime = DateTime.Now;
                _context.Add(introduction);
            }
            else
            {
                _context.Update(introduction);
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
            List<Introduction> list;
            if (string.IsNullOrEmpty(keyWord))
            {
                count = _context.Introduction.Count();
                list = _context.Introduction.OrderBy(p => p.orderNumber).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            else
            {
                count = _context.Introduction.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).Count();
                list = _context.Introduction.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).OrderBy(p => p.orderNumber).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            TableModel<Introduction> response = new TableModel<Introduction>(list, count);
            return JsonConvert.SerializeObject(response);
        }
    }
}

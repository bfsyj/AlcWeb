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
    public class AdminEnvironmentNewsController : Controller
    {
        private readonly MyContext _context;
        public AdminEnvironmentNewsController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            EnvironmentNews model = new EnvironmentNews();
            model.isShow = 1;
            model.imageUrl = "https://olc-wz.oss-cn-hangzhou.aliyuncs.com/upload/logoalcw.jpg";
            if (!string.IsNullOrEmpty(id))
                model = _context.EnvironmentNews.Find(id);
            return View(model);
        }

        public string Delete(string id)
        {
            var environmentNews = _context.EnvironmentNews.Find(id);
            _context.EnvironmentNews.Remove(environmentNews);
            if (_context.SaveChanges() > 0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }
        }

        public string Save(EnvironmentNews environmentNews)
        {
            if (string.IsNullOrEmpty(environmentNews.id))
            {
                environmentNews.id = Guid.NewGuid().ToString();
                environmentNews.lookNumber = 0;
                environmentNews.editUser = "管理员";
                environmentNews.editTime = DateTime.Now;
                _context.Add(environmentNews);
            }
            else
            {
                _context.Update(environmentNews);
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
            List<EnvironmentNews> list;
            if (string.IsNullOrEmpty(keyWord))
            {
                count = _context.EnvironmentNews.Count();
                list = _context.EnvironmentNews.OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            else
            {
                count = _context.EnvironmentNews.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).Count();
                list = _context.EnvironmentNews.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            TableModel<EnvironmentNews> response = new TableModel<EnvironmentNews>(list, count);
            return JsonConvert.SerializeObject(response);
        }
    }
}

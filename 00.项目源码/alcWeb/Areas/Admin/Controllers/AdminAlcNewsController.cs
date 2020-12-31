using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using alcService;
using alcEntity;
using alcCommon;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace alcWeb.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminAlcNewsController : Controller
    {
        private readonly MyContext _context;
        public AdminAlcNewsController(MyContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            AlcNews model = new AlcNews();
            model.isShow = 1;
            model.imageUrl = "https://olc-wz.oss-cn-hangzhou.aliyuncs.com/upload/logoalcw.jpg";
            if (!string.IsNullOrEmpty(id))
                model = _context.AlcNews.Find(id);
            return View(model);
        }

        public string Delete(string id)
        {
            var alcNews = _context.AlcNews.Find(id);
            _context.AlcNews.Remove(alcNews);
            if (_context.SaveChanges() > 0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }
        }

        public string Save(AlcNews alcNews)
        {
            if(string.IsNullOrEmpty(alcNews.id))
            {
                alcNews.id = Guid.NewGuid().ToString();
                alcNews.lookNumber = 0;
                alcNews.editUser = "管理员";
                alcNews.editTime = DateTime.Now;
                _context.Add(alcNews);
            }
            else
            {
                _context.Update(alcNews);
            }
            
            if( _context.SaveChanges()>0)
            {
                return "success";
            }
            else
            {
                return "failure";
            }

        }

        public string GetListByPage(string keyWord,int pageIndex, int pageSize)
        {
            keyWord = HttpUtility.UrlDecode(keyWord);

            int count;
            List<AlcNews> list;
            if (string.IsNullOrEmpty(keyWord))
            {
                count = _context.AlcNews.Count();
                list = _context.AlcNews.OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            else
            {
                count = _context.AlcNews.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).Count();
                list = _context.AlcNews.Where(p => EF.Functions.Like(p.title, "%" + keyWord + "%")).OrderByDescending(p => p.date).Skip((pageIndex - 1) * pageSize).Take(10).ToList();
            }
            TableModel<AlcNews> response = new TableModel<AlcNews>(list, count);
            return JsonConvert.SerializeObject(response);
        }


    }
}

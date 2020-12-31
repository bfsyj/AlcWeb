using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using alcCommon;
using Newtonsoft.Json;

namespace alcWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMenuController : Controller
    {
        public string GetMenu()
        {
            List<MenuModel> Menu= new List<MenuModel>();

            MenuModel M1 = new MenuModel() { Title = "新闻资讯", Href = "", Icon = "&#xe62a", Spread = true };
            MenuModel m1_1 = new MenuModel() { Title = "艾力辰动态", Href = "/Admin/AdminAlcNews/List", Icon = "&#xe60a", Spread = false };
            MenuModel m1_2 = new MenuModel() { Title = "环境新闻", Href = "/Admin/AdminEnvironmentNews/List", Icon = "&#xe705", Spread = false };
            MenuModel m1_3 = new MenuModel() { Title = "环保公示", Href = "/Admin/AdminEnvironmentFiles/List", Icon = "&#xe645", Spread = false};
            List<MenuModel> children1 = new List<MenuModel>();
            children1.Add(m1_1);
            children1.Add(m1_2);
            children1.Add(m1_3);
            M1.Children = children1.ToArray();

            MenuModel M2 = new MenuModel() { Title = "工程案例", Href = "", Icon = "&#xe630", Spread = false};
            MenuModel m2_1 = new MenuModel() { Title = "工程案例", Href = "/Admin/AdminEngineeringCase/List", Icon = "&#xe63c", Spread = false };
            List<MenuModel> children2 = new List<MenuModel>();
            children2.Add(m2_1);
            M2.Children = children2.ToArray();

            MenuModel M3 = new MenuModel() { Title = "公司业务", Href = "", Icon = "&#xe62a", Spread = false };
            MenuModel m3_1 = new MenuModel() { Title = "公司业务", Href = "/Admin/AdminIntroduction/List", Icon = "&#xe630", Spread = false };
            List<MenuModel> children3 = new List<MenuModel>();
            children3.Add(m3_1);
            M3.Children = children3.ToArray();

            Menu.Add(M1);
            Menu.Add(M2);
            Menu.Add(M3);
            string kk= JsonConvert.SerializeObject(Menu.ToArray(), new JsonSerializerSettings() { ContractResolver=new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()});
            return kk;
        }
    }
}

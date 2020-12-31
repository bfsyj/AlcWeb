using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using alcCommon;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace alcWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLoginController : Controller
    {
        public IActionResult Login()
        {
            LoginModel model = new LoginModel();
            model.result = "";
            return View(model);
        }



        [HttpPost]

        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {

            if (userName.Equals("admin") && password.Equals("123456"))
            {
                var claims = new List<Claim>(){new Claim(ClaimTypes.Name,userName),new Claim("password",password)};

                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties

                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(120),

                    IsPersistent = false,

                    AllowRefresh = false

                });

                if (returnUrl == null)
                {
                    return Redirect("/Admin/AdminHome/Index");
                }
                else
                {
                    return Redirect(returnUrl);
                }

                
            }
            LoginModel model = new LoginModel();
            model.result = "用户名或密码错误，请重新输入！";
            return View(model);

        }

        public async Task<IActionResult> Logout()

        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Admin/AdminLogin/Index");
        }

    }
}

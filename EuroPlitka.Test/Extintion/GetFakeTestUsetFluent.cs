using EuroPlitka.Controllers;
using EuroPlitka_Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EuroPlitka.Test.Extintion
{
    public static class GetFakeTestUsetFluent
    {



        public static T WithTestUser<T>(this T controller) where T : Controller
        {
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, WebConstanta.TestUserName),
                        new Claim(ClaimTypes.NameIdentifier, WebConstanta.TestIdUser)
                    }))
                }
            };
            return controller;
        }




    }
}

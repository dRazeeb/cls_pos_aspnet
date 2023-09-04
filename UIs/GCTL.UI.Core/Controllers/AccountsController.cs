using GCTL.Data.Models;
using GCTL.UI.Core.Extensions;
using GCTL.UI.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core;
using System.Net.NetworkInformation;
using GCTL.Core.ViewModels.Accounts;
using GCTL.Core.Helpers;

namespace GCTL.UI.Core.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext context;
        public AccountsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Route("")]
        public ActionResult Login()
        {
            return View();

        }

        [Route("")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set<UserInfoViewModel>(nameof(ApplicationConstants.LoginSessionKey), default);

                var user = context.CoreUserInfo
                    .Where(a => a.Username.Equals(model.Username)).FirstOrDefault();
                if (user != null)
                {
                    string password = "";
                    try
                    {
                        new PXLibrary.PXlibrary().PXDEcode(ref password, user.UserPassword);
                    }
                    catch (Exception)
                    {
                        password = user.UserPassword;
                    }

                    if (model.Password.Equals(password))
                    {
                        HttpContext.Session.Set(nameof(ApplicationConstants.LoginSessionKey), new UserInfoViewModel
                        {                            
                            EmployeeId = user.EmployeeId,
                            CompanyCode = user.CompanyCode,
                            Role = (DefaultRoles)Enum.Parse(typeof(DefaultRoles), user.Role, true),
                            Username = user.Username,
                            AccessCode = user.AccessCode,
                            IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            MacAddress = GetMACAddress()
                        });

                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        TempData["errorMessage"] = "Invalid User Name or Password";
                        //ViewBag.errorMessage = "Error: " + ex.Message + " - " + ex.InnerException;
                        return View(model);
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Invalid User Name or Password";
                    //ViewBag.errorMessage = "Error: " + ex.Message + " - " + ex.InnerException;
                    return View(model);
                }
            }
            return View(model);

        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Set<UserInfoViewModel>(nameof(ApplicationConstants.LoginSessionKey), default);
            HttpContext.Session.Clear(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Accounts");
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }
}
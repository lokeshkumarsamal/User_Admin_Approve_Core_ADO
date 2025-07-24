using Microsoft.AspNetCore.Mvc;
using User_Admin_Approve_Core_ADO.Models;
using User_Admin_Approve_Core_ADO.Services;

namespace User_Admin_Approve_Core_ADO.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminModel user)
        {
            await _userServices.AddUserAsync(user);
            return View();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userloginid, string userpass)
        {
            try
            {
                var x = await _userServices.UserLoginAsync(userloginid, userpass);
                if (x != null && x.userloginid != null)
                {
                    if (x.userloginid == userloginid && x.userpass == userpass)
                    {
                        HttpContext.Session.SetString("isapproved", x.isapproved.ToString().ToLower());
                        if (x.isapproved)
                        {
                            HttpContext.Session.SetString("userloginid", x.userloginid);
                            HttpContext.Session.SetString("username", x.username);
                            HttpContext.Session.SetString("userphone", x.userphone);
                            HttpContext.Session.SetString("useremail", x.useremail);
                            HttpContext.Session.SetString("useradd", x.useradd);
                            HttpContext.Session.SetString("isapproved", x.isapproved.ToString().ToLower());
                            HttpContext.Session.SetString("isadmin", x.isadmin.ToString().ToLower());

                            Console.WriteLine($"Session Values Set: userloginid={x.userloginid}, isadmin={x.isadmin.ToString().ToLower()}");


                            if (x.isadmin)
                            {
                                return RedirectToAction("AdminDashboard", "User");
                            }
                            else
                            {
                                return RedirectToAction("Dashboard", "User");
                            }
                        }
                        else
                        {
                            ViewBag.Msg = "You are not approved. Please contact the administrator.";
                            return View();
                        }
                    }
                }

                ViewBag.Msg = "Check Your Credentials";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "An error occurred. Please try again.";
                return View();
            }
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
                ViewBag.userphone = HttpContext.Session.GetString("userphone");
                ViewBag.useremail = HttpContext.Session.GetString("useremail");
                ViewBag.useradd = HttpContext.Session.GetString("useradd");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        public async Task<IActionResult> AdminDashboard()
        {
            var username = HttpContext.Session.GetString("username");
            var isadmin = HttpContext.Session.GetString("isadmin");

           //for testing session value in console
           Console.WriteLine($"Session Values Retrieved: username={username}, isadmin={isadmin}");

            if (username != null && isadmin == "true")
            {
                var pendingUsers = await _userServices.GetAllUsersAsync();
                ViewBag.PendingUsers = pendingUsers;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ApproveUser(string userloginid)
        {
            if (HttpContext.Session.GetString("username") != null && HttpContext.Session.GetString("isadmin") == "true")
            {
                var result = await _userServices.ApproveUserAsync(userloginid);
                if (result == "Success")
                {
                    var pendingUsers = await _userServices.GetAllUsersAsync(); 
                    ViewBag.PendingUsers = pendingUsers; 
                    return View("AdminDashboard"); 
                }
                else
                {
                    ViewBag.Msg = "Error approving user.";
                    return RedirectToAction("AdminDashboard");
                }
            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

    }

}

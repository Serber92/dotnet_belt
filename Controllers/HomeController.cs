using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt.Models;
using Microsoft.AspNetCore.Http;
using ContextNamespace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Belt.Controllers
{
    public class HomeController : Controller
    {
         private MyContext dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Registration_Login()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("Logged", "False");
            return View();
        }
        public IActionResult RegistrationProcess(Wrapper modelData)
        {
            User newUser = modelData.User;
            if(ModelState.IsValid)
            {

                                    var hasNumber = new Regex(@"[0-9]+");
                                    var hasUpperChar = new Regex(@"[A-Z]+");
                                    var hasMiniMaxChars = new Regex(@".{8,15}");
                                    var hasLowerChar = new Regex(@"[a-z]+");
                                    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                                    if (!hasLowerChar.IsMatch(newUser.Password))
                                    {
                                        ModelState.AddModelError("User.Password", "Password should contain At least one lower case letter");
                                        return View("Registration_Login");
                                    }
                                     if (!hasUpperChar.IsMatch(newUser.Password))
                                    {
                                        ModelState.AddModelError("User.Password", "Password should contain At least one upper case letter");
                                        return View("Registration_Login");
                                    }
                                     if (!hasNumber.IsMatch(newUser.Password))
                                    {
                                        ModelState.AddModelError("User.Password", "Password should contain At least one numeric value");
                                        return View("Registration_Login");
                                    }

                                     if (!hasSymbols.IsMatch(newUser.Password))
                                    {
                                        ModelState.AddModelError("User.Password", "Password should contain At least one special case characters");
                                        return View("Registration_Login");
                                    }
                 if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("User.Email", "Email already registered!");
                    return View("Registration_Login");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                User retrievedUser = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                HttpContext.Session.SetString("Logged", "True");
                HttpContext.Session.SetInt32("UserId", retrievedUser.UserId);
                return RedirectToAction("Main");
            }
            else
                return View("Registration_Login");
        }
        public IActionResult LoginProcess(Wrapper modelData)
        {
            Login newLogin = modelData.Login;
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == newLogin.Email))
                {
                    User retrievedUser = dbContext.Users.FirstOrDefault(u => u.Email == newLogin.Email);
                    var hasher = new PasswordHasher<Login>();
                    var result = hasher.VerifyHashedPassword(newLogin, retrievedUser.Password, newLogin.Password);
                    if(result != 0)
                    {
                        HttpContext.Session.SetString("Logged", "True");
                        HttpContext.Session.SetInt32("UserId", retrievedUser.UserId);
                        return RedirectToAction("Main");
                    }
                    else
                    {
                        ModelState.AddModelError("Login.Password", "Wrong Password!");
                        return View("Registration_Login");
                    }
                }
                else
                {
                    ModelState.AddModelError("Login.Email", "No such email registered!");
                    return View("Registration_Login");
                }
            }
            else
                return View("Registration_Login");
        }

        public IActionResult Main()
        {
            if(HttpContext.Session.GetString("Logged") == "True")
            {
                int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
                User retrievedUser = dbContext.Users
                .Include(u => u.ActivitiesJoined)
                .FirstOrDefault(u => u.UserId == (int)CurrentUserId);

                DateTime Now = DateTime.Now;
                

                List<_Activity> Allactivities = dbContext.Activities
                .Include(a => a.Creator)
                .Include(a => a.Participants)
                .ThenInclude(sub => sub.User)
                .OrderBy(a => a.DateConverted)
                .ToList();

               

                Wrapper modelData = new Wrapper()
                {
                    User = retrievedUser,
                    Activities = Allactivities,
                };
                return View(modelData);
            }
            else
            {
                ModelState.AddModelError("Login.Email", "Not Logged in!");
                ModelState.AddModelError("User.Name", "Not Logged in!");
                return View("Registration_Login");
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("Registration_Login");
        }
        public IActionResult AddNewActivity()
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
            User retrievedUser = dbContext.Users.FirstOrDefault(u => u.UserId == (int)CurrentUserId);

               

                Wrapper modelData = new Wrapper()
                {
                    User = retrievedUser,

                };
            return View(modelData);
        }
        public IActionResult AddNewActivityProcess(Wrapper modelData)
        {
            _Activity newActivity = modelData.Activity;
            if(ModelState.IsValid)
            {
                var DateCheck = new Regex(@"^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$");
                if (!DateCheck.IsMatch(newActivity.Date))
                {
                    ModelState.AddModelError("Activity.Date", "Date should be in format 'mm/dd/yyyy'");
                     int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
                    User retrievedUser = dbContext.Users.FirstOrDefault(u => u.UserId == (int)CurrentUserId);
               
                    Wrapper NewmodelData = new Wrapper()
                    {
                        User = retrievedUser,

                    };
                    return View("AddNewActivity", NewmodelData);
                }
                var MilitaryTime = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
                if (!MilitaryTime.IsMatch(newActivity.Time))
                {
                    ModelState.AddModelError("Activity.Time", "Time should be in format 'hh:mm'");
                     int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
                    User retrievedUser = dbContext.Users.FirstOrDefault(u => u.UserId == (int)CurrentUserId);
               
                    Wrapper NewmodelData = new Wrapper()
                    {
                        User = retrievedUser,

                    };
                    return View("AddNewActivity", NewmodelData);
                }


                DateTime Now = DateTime.Now;
                DateTime thisDate = DateTime.Parse(newActivity.Date);

                TimeSpan Time = TimeSpan.Parse(newActivity.Time);

                thisDate = thisDate.Date + Time;


                if(Now >= thisDate)
                {
                     ModelState.AddModelError("Activity.Date", "Date must be in future");
                     int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
                    User retrievedUser = dbContext.Users.FirstOrDefault(u => u.UserId == (int)CurrentUserId);
               
                    Wrapper NewmodelData = new Wrapper()
                    {
                        User = retrievedUser,

                    };
                    return View("AddNewActivity", NewmodelData);
                }

                newActivity.DateConverted = thisDate;
                dbContext.Activities.Add(newActivity);
                dbContext.SaveChanges();
                _Activity retrievedActivity = dbContext.Activities.FirstOrDefault(u => u.Title == newActivity.Title);
                return Redirect("ActivityInfo/" + retrievedActivity.ActivityId);
            }
            else
            {
                int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
                User retrievedUser = dbContext.Users.FirstOrDefault(u => u.UserId == (int)CurrentUserId);

               

                Wrapper NewmodelData = new Wrapper()
                {
                    User = retrievedUser,

                };
                return View("AddNewActivity", NewmodelData);
            }
        }
        [HttpGet("home/ActivityInfo/{ActivityId}")]
        public IActionResult ActivityInfo(int ActivityId)
        {
            _Activity retrievedActivity = dbContext.Activities
            .Include(a => a.Creator)
            .Include(a => a.Participants)
            .ThenInclude(sub => sub.User)
            .FirstOrDefault(a => a.ActivityId == ActivityId);

            int? CurrentUserId = HttpContext.Session.GetInt32("UserId");
            User retrievedUser = dbContext.Users
            .Include(u => u.ActivitiesJoined)
            .FirstOrDefault(u => u.UserId == (int)CurrentUserId);

            Wrapper modelData = new Wrapper()
                {
                    Activity = retrievedActivity,
                    User = retrievedUser,

                };
            return View(modelData);
        }
        [HttpGet("home/ActivityInfo/JoinActivity/{ActivityId}")]
        public IActionResult JoinActivity(int ActivityId)
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("UserId");

            Participant newParticipant = new Participant()
            {
                UserId = (int)CurrentUserId,
                ActivityId = ActivityId
            };

            dbContext.Participants.Add(newParticipant);
            dbContext.SaveChanges();
            return RedirectToAction("Main");
        }
        [HttpGet("home/ActivityInfo/LeaveActivity/{ActivityId}")]
        public IActionResult LeaveActivity(int ActivityId)
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("UserId");

            Participant ParticipantToDelete = dbContext.Participants.FirstOrDefault(p => p.UserId == (int)CurrentUserId && p.ActivityId == ActivityId);

            dbContext.Participants.Remove(ParticipantToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Main");
        }

        [HttpGet("home/ActivityInfo/Delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
            _Activity activityToDelete = dbContext.Activities.FirstOrDefault(a => a.ActivityId == ActivityId);
            dbContext.Activities.Remove(activityToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Main");
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

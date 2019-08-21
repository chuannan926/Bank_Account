using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Login.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]  
        public IActionResult Index()
        {
            
            return View();
        }


        [HttpPost("processregister")]
        public IActionResult ProcessRegister(User New_User)
        {   
            System.Console.WriteLine(New_User.Email);
            System.Console.WriteLine(New_User.First_Name);
            System.Console.WriteLine(New_User.Last_Name);
            System.Console.WriteLine(New_User.Password);
            System.Console.WriteLine(New_User.Confirm);
            System.Console.WriteLine(new string('*',80));


            if(ModelState.IsValid){
                if(dbContext.Users.Any(u => u.Email == New_User.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                New_User.Password = Hasher.HashPassword(New_User, New_User.Password);
                dbContext.Add(New_User);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", New_User.UserId);
                return RedirectToAction("Success");
            }
            else{
                return View("Index");
            }
        }

        [HttpGet("account")]
        public IActionResult Success()
        {
            if (HttpContext.Session.GetInt32("UserId") == null) // If user is not logged in, kick them out to Index
            {
                return RedirectToAction("Index");
            }
            else
            {
                //get UserID back from session, already in session
                int? logged_in_user_id = HttpContext.Session.GetInt32("UserId");
               
                System.Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
                //HomeController从DB里找出来User，Query the DB for the user whose id is in session, blue arrow

                User ThisUser = dbContext.Users
                .Include(u=>u.userTransaction)             
                .FirstOrDefault(u => u.UserId == logged_in_user_id);
                // In our return statement, we're passing that user object to the cshtml, red arrow,在account那个网页显示ThisUser；
                
                var transactions = ThisUser.userTransaction;
                decimal sum = 0;

                foreach(var trans in transactions)
                {
                    sum += trans.Amount;
                }
                
                ViewBag.Balance = sum;

                
                ViewBag.AllTransactions = transactions;
                ViewBag.Name = ThisUser.First_Name;
                
                    
                return View("account");
                
            

      
            }
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            int? user_id = HttpContext.Session.GetInt32("UserId");
            if(user_id != null)
            {
                return Redirect($"account/{user_id}");
            }
            return View("login");

        }

        [HttpPost("processlogin")]
        public IActionResult processlogin(LoginUser Login_User_From_Form)
        {   
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == Login_User_From_Form.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(Login_User_From_Form, userInDb.Password, Login_User_From_Form.Password);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("Password","Invalid Email/Password");
                    return View("login");
                }           
                HttpContext.Session.SetInt32("UserId",userInDb.UserId);
                // To retrieve a string from session we use ".GetString" 
                return RedirectToAction("Success");

            }

            return View("login");
        }

        [HttpPost("process_transaction")]
        public IActionResult processTransaction(Transaction Transaction_Amount)
        {   
            var UserId = HttpContext.Session.GetInt32("UserId");//find The UserID in session;
            User ThisUser = dbContext.Users//find ThisUser是UserID的，哪个User是UserID1.
            .Include(u=>u.userTransaction)   //找到User之后，找到这个User的Transaction；          
            .FirstOrDefault(u => u.UserId == UserId); //找到第一个的User是UserID的
            
            var transactions = ThisUser.userTransaction;//找到这个User的所有Transaction
            // decimal sum = ThisUser.userTransaction.Select(a=>a.Amount).Sum();
            
            decimal SUM = 0; 
            foreach (var Transaction in transactions)
            {
                SUM += Transaction.Amount; 
            }
            if (-Transaction_Amount.Amount > SUM)
            {
                return RedirectToAction("Success");
            }


            else
            {
                Transaction_Amount.UserId = (int)UserId; 
                dbContext.Add(Transaction_Amount);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
        }
        
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }



}


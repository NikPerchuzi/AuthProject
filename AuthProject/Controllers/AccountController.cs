﻿using AuthProject.Models;
using AuthProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthProject.Controllers
{
    public class AccountController : BaseController
    {
        private UserContext db;
        public AccountController(UserContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (Anonymous)
            {
                return View();
            }
            return RedirectToAction("Index", "Products");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email, user.Id); // аутентификация

                    return RedirectToAction("Index", "Products");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(bool redirectToMain = false)
        {
            if (redirectToMain)
            {
                return RedirectToAction("Index", "Products");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = new ResultVM();
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    var newUser = new User { Email = model.Email, Password = model.Password };
                    db.Users.Add(newUser);
                    await db.SaveChangesAsync();
                   // await Authenticate(model.Email, newUser.Id); // аутентификация
                    result.Success = true;
                    result.Message = "Вы успешно зарегистрировались";
                }
                else
                {
                    result.Message = "Такой пользователь уже существует";
                }
            }
            else
            {
                result.Message = "Проверьте правильность ввода логина или пароля!";
            }
            return Json(result);
        }

        private async Task Authenticate(string userName, int userId)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("UserId", userId.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AuthProject.Controllers
{
    public abstract class BaseController : Controller
    {
        public int? UserId => int.TryParse(HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value, out int id) ? (int?)id : null;
        public bool Anonymous => !UserId.HasValue;
    }
}

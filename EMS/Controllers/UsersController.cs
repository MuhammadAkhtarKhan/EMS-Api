using EMS.Models;
using EMS.Services;
//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;

namespace EMS.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("api/account/getuser/{id}")]
        public IHttpActionResult GetUFirstUser(int id, UserService _userService)
        {
            // Get user from dummy list
            var users = _userService.GetUserList();
            var user = users.Find(x => x.Id == id);
            return Ok(user);
        }


        //[HttpGet]
        //public IHttpActionResult GetAll(IUserService userService)
        //{
        //    _userService = userService;
        //    var users = userService.GetAll();
        //    return Ok(users);
        //}
    }
}

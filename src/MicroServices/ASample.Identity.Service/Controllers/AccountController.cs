﻿using ASample.Identity.Service.Infrasturatuce.Repository;
using ASample.Identity.Service.Models.User;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ASample.Identity.Service.Controllers
{
    [Authorize]

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private IdentityUserRepository _identityRepo;

        public AccountController()
        {
            _identityRepo = new IdentityUserRepository();

        }
        // GET: Account
        [AllowAnonymous]

        [Route("Register")]

        public async Task<IHttpActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await _identityRepo.Register(model);
            IHttpActionResult errorResult = GetError(result);
            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();

        }
        private IHttpActionResult GetError(IdentityResult result)

        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                foreach (string err in result.Errors)
                {
                    ModelState.AddModelError("", err);
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }
                return BadRequest(ModelState);

            }
            return null;

        }
    }
}
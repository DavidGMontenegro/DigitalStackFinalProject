﻿using FinalAPI.Models;
using FinalAPI.Services;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(String username, String email, String password)
        {
            try
            {
                User user = new User(username, email, password);
                var registeredUser = await this.userService.RegisterUser(user);
                return Ok(registeredUser);
            }
            catch (ArgumentException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(String username, String password)
        {
            try
            {
                var user = await this.userService.Authenticate(username, password);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("Get-user")]
        public async Task<IActionResult> GetUserData(String username)
        {
            try
            {
                var user = await this.userService.GetUserData(username);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("modify-user-data")]
        public async Task<IActionResult> ModifyUserData(String oldUsername, String newUsername, String newEmail)
        {
            try
            {
                var user = await this.userService.ModifyUserData(oldUsername, newUsername, newEmail);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("change-user-password")]
        public async Task<IActionResult> ChangeUserPassword(String username, String oldPassword, String newPassword)
        {
            try
            {
                var user = await this.userService.ChangeUserPassword(username, oldPassword, newPassword);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("change-user-picture")]
        public async Task<IActionResult> ChangeUserProfilePicture(String username, [FromBody] byte[] newPicture)
        {
            try
            {
                var user = await this.userService.ChangeUserProfilePic(username, newPicture);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

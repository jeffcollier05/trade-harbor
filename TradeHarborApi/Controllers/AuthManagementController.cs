﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradeHarborApi.Common;
using TradeHarborApi.Models.AuthDtos;
using TradeHarborApi.Repositories;
using TradeHarborApi.Services;

namespace TradeHarborApi.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication-related requests such as user registration and login.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthRepository _authRepository;
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthManagementController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager for managing identity users.</param>
        /// <param name="authRepository">The repository for authentication-related database operations.</param>
        /// <param name="authService">The service for generating JWT tokens and managing authentication.</param>
        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            AuthRepository authRepository,
            AuthService authService)
        {
            _userManager = userManager;
            _authRepository = authRepository;
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user with the provided registration information.
        /// </summary>
        /// <param name="requestDto">The registration request data transfer object.</param>
        /// <returns>An action result indicating the success or failure of the registration process.</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);
                if (emailExist != null)
                {
                    return BadRequest(error: RequestResponse.EMAIL_ALREADY_EXISTS);
                }
                else
                {
                    var newUser = new IdentityUser()
                    {
                        Email = requestDto.Email,
                        UserName = requestDto.Username
                    };

                    var usernameExist = await _userManager.FindByNameAsync(requestDto.Username);
                    if (usernameExist == null)
                    {
                        var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
                        if (isCreated.Succeeded)
                        {
                            var linkedRequestDto = new LinkedAccountDto()
                            {
                                FirstName = requestDto.FirstName,
                                LastName = requestDto.LastName,
                                ProfilePictureUrl = requestDto.ProfilePictureUrl,
                                UserId = newUser.Id
                            };
                            await _authRepository.InsertLinkedAccountInformation(linkedRequestDto);

                            var token = await _authService.GenerateJwtToken(newUser);
                            return Ok(new RegistrationRequestResponse()
                            {
                                Result = true,
                                Token = token
                            });
                        }
                        else
                        {
                            return BadRequest(error: RequestResponse.REQUEST_FAILED);
                        }
                    }
                    else
                    {
                        return BadRequest(error: RequestResponse.USERNAME_ALREADY_EXISTS);
                    }
                }
            }
            else
            {
                return BadRequest(error: RequestResponse.INCOMPLETE_REQUEST);
            }
        }

        /// <summary>
        /// Logs in a user with the provided login information.
        /// </summary>
        /// <param name="requestDto">The login request data transfer object.</param>
        /// <returns>An action result indicating the success or failure of the login process.</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);

                if (existingUser != null)
                {
                    var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requestDto.Password);
                    if (isPasswordValid)
                    {
                        var token = await _authService.GenerateJwtToken(existingUser);
                        return Ok(new LoginRequestResponse()
                        {
                            Token = token,
                            Result = true
                        });
                    }
                    else
                    {
                        return BadRequest(error: RequestResponse.INVALID_AUTHENTICATION);
                    }
                }
                else
                {
                    return BadRequest(error: RequestResponse.INVALID_AUTHENTICATION);
                }
            }
            else
            {
                return BadRequest(error: RequestResponse.INCOMPLETE_REQUEST);
            }
        }
    }
}
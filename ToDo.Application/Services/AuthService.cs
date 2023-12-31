﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using ToDo.Application.Contracts;
using ToDo.Application.DTOs.Auth;
using ToDo.Application.DTOs.User;
using ToDo.Application.Notification;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Models;

namespace ToDo.Application.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IMapper mapper, INotificator notificator, IConfiguration configuration, IUserRepository userRepository, IPasswordHasher<User> passwordHasher) : base(mapper, notificator)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<TokenDto?> Login(LoginDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user != null && _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password) == PasswordVerificationResult.Success)
            return GenerateToken(user);

        Notificator.Handle("E-mail ou senha estão incorretos.");
        return null;
    }

    public async Task<UserDto?> Register(RegisterDto dto)
    {
        var user = Mapper.Map<User>(dto);

        if (!dto.Validate(out var validationResult))
        {
            Notificator.Handle(validationResult.Errors);
            return null;
        }

        var getUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (getUser != null)
        {
            Notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return null;
        }

        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        _userRepository.Create(user);

        if (await _userRepository.UnityOfWork.Commit())
            return Mapper.Map<UserDto>(user);

        Notificator.Handle("Não foi possível cadastrar o usuário");
        return null;
    }
    
    private TokenDto GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"] ?? string.Empty);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["AppSettings:ExpirationHours"] ?? string.Empty)),
            Issuer = _configuration["AppSettings:Issuer"],
            Audience = _configuration["AppSettings:ValidOn"]
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            AccessToken = encodedToken
        };
    }
}
global using BrainHope.Services.DTO.Authentication.SingUp;
global using FluentValidation;
global using BrainHope.Services.DTO.Authentication.SignIn;
global using CleanArchitecture.DataAccess.Models;
global using CleanArchitecture.Services.DTOs.Responses;
global using MimeKit;
global using BrainHope.Services.DTO.Email;

global using CleanArchitecture.DataAccess.Contexts;
global using CleanArchitecture.Services.Interfaces;
global using CleanArchitecture.Utilities;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using MailKit.Net.Smtp;
global using MailKit.Security;
global using Microsoft.EntityFrameworkCore;
global using CleanArchitecture.Services.DTOs.Categories;
global using CleanArchitecture.Services.DTOs.Orders;
global using CleanArchitecture.Services.DTOs.Products;
global using CleanArchitecture.Services.DTOs.ShoppingCarts;

global using CleanArchitecture.Services.Services;
global using CleanArchitecture.Services.Validachans;
global using FluentValidation.AspNetCore;
global using Microsoft.Extensions.DependencyInjection;








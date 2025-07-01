
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using Microsoft.EntityFrameworkCore;
global using System.Security.Claims;
global using Utilites;

global using Microsoft.AspNetCore.Identity;
global using CleanArchitecture.DataAccess.Models;
global using CleanArchitecture.Services.Interfaces;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using CleanArchitecture.DataAccess.IRepository;
global using CleanArchitecture.DataAccess.IUnitOfWorks;
global using Mapster;
global using FluentValidation;
global using CleanArchitecture.Services.DTOs.Orders;
global using CleanArchitecture.Services.DTOs.Products;
global using CleanArchitecture.Services.DTOs.Categories;
global using CleanArchitecture.Services.DTOs.Responses;
global using CleanArchitecture.Services.DTOs.ShoppingCarts;
global using BrainHope.Services.DTO.Email;
global using BrainHope.Services.DTO.Authentication.SingUp;
global using BrainHope.Services.DTO.Authentication.SignIn;

global using CleanArchitecture.Services.Services;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using CleanArchitecture.DataAccess;
global using CleanArchitecture.DataAccess.Contexts;
global using CleanArchitecture.Services;



// System
global using System;
global using System.Collections.Generic;
global using System.Data;
global using System.Text;
global using System.ComponentModel.DataAnnotations;

// XUnit
global using Xunit;
global using FluentAssertions;
global using Moq;
global using Frontend.Tests.Mocks;

// Backend
global using Backend;
global using Backend.Models;
global using Backend.Enums;
global using Backend.DataAccess.Interfaces;
global using Backend.DataAccess;
global using Backend.Services;

// frontend
global using Frontend;
global using Frontend.Models;
global using RazorMusic.Pages;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Logging;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.Session;
global using Microsoft.Extensions.DependencyInjection;
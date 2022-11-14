﻿using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Other;

namespace PhlegmaticOne.InnoGotchi.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error(string errorMessage) =>
        View(new ErrorViewModel
        {
            ErrorMessage = errorMessage
        });
}
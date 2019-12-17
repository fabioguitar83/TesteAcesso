using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteAcesso.API.Controllers
{
    public abstract class ControllerBaseValidator: ControllerBase
    {
        protected List<string> ReturnValidations(ValidationException ex) 
        {
            return ex.Errors.Select(p => p.ErrorMessage).ToList();
        }

    }
}

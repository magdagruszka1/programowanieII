using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Programowanie2.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string formula)
        {
            if (string.IsNullOrEmpty(formula))
            {
                var data = new
                {
                    status = "error",
                    message = "not found formula"
                };
                return NotFound(data);
            }

            Programowanie2.ReversePolishNotation rpn = new Programowanie2.ReversePolishNotation();

            try
            {
                rpn.Parse(formula);
                var data = new
                {
                    status = "ok",
                    result = new
                    {
                        infix = rpn.TransitionExpression,
                        rpn = rpn.PostfixExpression
                    }
                };
                return Ok(data);
            }
            catch (System.Exception e)
            {
                var data = new
                {
                    status = "error",
                    message = e.Message
                };
                return BadRequest(data);
            }
        }
    }
}
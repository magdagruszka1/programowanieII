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
    public class CalculateController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string formula, double x)
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
                double result = 0.0;
                rpn.VariableX = Convert.ToDouble(x);
                rpn.Parse(formula);

                result = rpn.Evaluate();
                var data = new
                {
                    status = "ok",
                    result = result
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
        [HttpGet]
        [Route("xy")]
        public IActionResult Get(string formula, double from, double to, int n)
        {
            if (formula == null)
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
                List<PointType> list = rpn.CalculateRange(from, to, n);
                var data = new
                {
                    status = "ok",
                    result = list.Select(i => new
                    {
                        x = i.x,
                        y = i.y
                    })
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

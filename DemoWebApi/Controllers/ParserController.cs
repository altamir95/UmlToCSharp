using Microsoft.AspNetCore.Mvc;
using UmlToCSharp;
using UmlToCSharp.Validators;

namespace DemoWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ParserController : ControllerBase
{
    private readonly ILogger<ParserController> _logger;

    public ParserController(ILogger<ParserController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get(string uml)
    {
        var umlObj = new Uml(uml);
        foreach (var item in umlObj.EntityUml)
        {
            var a = new EntityValidator(item);
            if (!a.IsValid())
                return a.Error;
        }

        foreach (var item in umlObj.EnumUml)
        {
            var a = new EnumValidator(item);
            if (!a.IsValid())
                return a.Error;
        }
        return new Uml(uml).ToString();
    }
}
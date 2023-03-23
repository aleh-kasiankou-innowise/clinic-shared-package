using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Shared.BaseClasses;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
}
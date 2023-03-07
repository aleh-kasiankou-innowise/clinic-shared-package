using Microsoft.AspNetCore.Mvc;

namespace Innowise.Clinic.Shared.ControllersAbstractions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
}
using System;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController] //attribute to make this class a controller
[Route("api/[controller]")] //route to the controller
public class BaseApiController :ControllerBase //base controller
{

}

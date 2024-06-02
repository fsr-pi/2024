using EFModel;
using Microsoft.Extensions.Logging;

namespace ODataApi.Controllers
{
  public class PeopleController : GenericController<Person>
  {  
    public PeopleController(FirmContext ctx, ILogger<PeopleController> logger) : base(ctx, logger)
    {

    }
  }
}
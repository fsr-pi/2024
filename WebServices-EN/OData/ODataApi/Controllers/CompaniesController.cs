using EFModel;
using Microsoft.Extensions.Logging;

namespace ODataApi.Controllers
{
  public class CompaniesController : GenericController<Company>
  {  
    public CompaniesController(FirmContext ctx, ILogger<CompaniesController> logger) : base(ctx, logger)
    {

    }
  }
}
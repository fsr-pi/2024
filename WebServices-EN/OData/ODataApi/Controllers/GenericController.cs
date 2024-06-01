using EFModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ODataApi.Controllers
{
  public abstract class GenericController<T> : ODataController where T:class
  {
    protected readonly FirmContext ctx;
    protected readonly ILogger logger;
    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="ctx">FirmContext za pristup bazi podataka</param>
    /// <param name="logger">Logger za evidentiranje pogrešaka</param>
    protected GenericController(FirmContext ctx, ILogger<GenericController<T>> logger)
    {
      this.logger = logger;
      this.ctx = ctx;
    }

    // GET: odata/T
    /// <summary>
    /// Postupak za dohvat svih T-ova prema kriterijima unutar OData zahtjeva. 
    /// </summary>
    /// <returns>Popis odabranih zapisa. Vraća IQueryable i omogućava upite koristeći OData</returns>
    [EnableQuery(PageSize = 50)]
    public IQueryable<T> Get()
    {
      logger.LogTrace($"Get {Request.QueryString.Value}");
      var query = ctx.Set<T>().AsNoTracking();
      return query;
    }

    // GET: odata/T(key)
    /// <summary>
    /// Postupak za dohvat nekog zapisa po cjelobrojnom primarnom ključu. 
    /// </summary>
    /// <returns>podatak</returns>
    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
      logger.LogTrace($"Get + key = {key} {Request.QueryString.Value}");
      var item = await ctx.Set<T>()
                          .FindAsync(key);                             
      if (item != null)
      {
        return Ok(item);        
      }
      else
      {
        return NotFound("Traženi element ne postoji");
      }
    }
  }
}
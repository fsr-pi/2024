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
  public class PartnersController : ODataController
  {
    private readonly FirmContext ctx;
    private readonly ILogger logger;
    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="ctx">FirmContext za pristup bazi podataka</param>
    /// <param name="logger">Logger za evidentiranje pogrešaka</param>
    public PartnersController(FirmContext ctx, ILogger<PartnersController> logger)
    {
      this.logger = logger;
      this.ctx = ctx;
    }

    // GET: odata/Partner
    /// <summary>
    /// Postupak za dohvat svih partnera prema kriterijima unutar OData zahtjeva. 
    /// </summary>
    /// <returns>Popis odabranih dokumenata. Vraća IQueryable i omogućava upite koristeći OData</returns>
    [EnableQuery(PageSize = 50)]
    public IQueryable<Partner> Get()
    {
      logger.LogTrace($"Get {Request.QueryString.Value}");
      var query = ctx.Partners.AsNoTracking();
      return query;
    }

    // GET: odata/Partner(key)
    /// <summary>
    /// Postupak za dohvat nekog partnera. 
    /// </summary>
    /// <returns>Podatak o partneru</returns>
    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
      logger.LogTrace($"Get + key = {key} {Request.QueryString.Value}");
      var query = ctx.Partners
                      .AsNoTracking()
                      .Where(d => d.PartnerId == key);                              
      if (await query.AnyAsync())
      {
        return Ok(query);        
      }
      else
      {
        return NotFound("Traženi partner ne postoji");
      }
    }
  }
}
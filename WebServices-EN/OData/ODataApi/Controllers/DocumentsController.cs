using EFModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ODataApi.Controllers
{
  public class DocumentsController : ODataController
  {
    private readonly FirmContext ctx;
    private readonly ILogger logger;
    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="ctx">FirmContext za pristup bazi podataka</param>
    /// <param name="logger">Logger za evidentiranje pogrešaka</param>
    public DocumentsController(FirmContext ctx, ILogger<DocumentsController> logger)
    {
      this.logger = logger;
      this.ctx = ctx;
    }

    // GET: odata/Documents
    /// <summary>
    /// Postupak za dohvat svih dokument prema kriterijima unutar OData zahtjeva. 
    /// </summary>
    /// <returns>Popis odabranih dokumenata. Vraća IQueryable i omogućava upite koristeći OData</returns>
    [EnableQuery(PageSize = 50)]
    public IQueryable<Document> Get()
    {
      logger.LogTrace($"Get {Request.QueryString.Value}");
      var query = ctx.Documents.AsNoTracking();
      return query;
    }

    // GET: odata/Documents(key)
    /// <summary>
    /// Postupak za dohvat nekog dokumenta. 
    /// </summary>
    /// <returns>Podatak o dokumentu</returns>
    [EnableQuery]
    public async Task<IActionResult> Get(int key)
    {
      logger.LogTrace($"Get + key = {key} {Request.QueryString.Value}");
      var query = ctx.Documents
                      .AsNoTracking()
                      .Where(d => d.DocumentId == key);                              
      if (await query.AnyAsync())
      {
        return Ok(query);        
      }
      else
      {
        return NotFound("Traženi dokument ne postoji");
      }
    }
  }
}
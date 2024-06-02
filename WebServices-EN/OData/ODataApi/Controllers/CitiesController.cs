using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using ODataApi.Contract;
using System.Text;
using System.Text.Json;

namespace ODataApi.Controllers
{
  public class CitiesController : ODataController
  {
    private readonly EFModel.FirmContext ctx;
    private readonly ILogger logger;
    /// <summary>
    /// Konstruktor
    /// </summary>
    /// <param name="ctx">FirmContext za pristup bazi podataka</param>
    /// <param name="logger">Logger za evidentiranje pogrešaka</param>
    public CitiesController(EFModel.FirmContext ctx, ILogger<CitiesController> logger)
    {
      this.logger = logger;
      this.ctx = ctx;
    }

    // GET: odata/Cities
    /// <summary>
    /// Postupak za dohvat svih mjesta (vraća maksimalno 50 mjesta u jednom upitu). 
    /// </summary>
    /// <returns>Popis svih mjesta. Vraća IQueryable i omogućava upite koristeći OData</returns>    
    [EnableQuery(PageSize = 50)]
    public IQueryable<Mjesto> Get()
    {
      logger.LogTrace("Get: " + Request.QueryString.Value);
      var query = ctx.Cities
                     .Select(m => new Mjesto
                     {
                       IdMjesta = m.CityId,
                       NazivMjesta = m.CityName,
                       PostBrojMjesta = m.PostalCode,
                       PostNazivMjesta = m.PostalName,
                       OznDrzave = m.CountryCode,
                       NazivDrzave = m.CountryCodeNavigation.CountryName
                     });
      return query;
    }

    // GET: odata/Cities(key)
    /// <summary>
    /// Postupak za dohvat podataka nekog mjesta. 
    /// </summary>
    /// <returns>Podatak o mjestu</returns>
    public async Task<IActionResult> Get(int key)
    {
      logger.LogTrace($"Get + key = {key} {Request.QueryString.Value}");
      var mjesto = await ctx.Cities
                            .Where(m => m.CityId == key)
                            .Select(m => new Mjesto
                            {
                              IdMjesta = m.CityId,
                              NazivMjesta = m.CityName,
                              PostBrojMjesta = m.PostalCode,
                              PostNazivMjesta = m.PostalName,
                              OznDrzave = m.CountryCode,
                              NazivDrzave = m.CountryCodeNavigation.CountryName
                            })
                            .FirstOrDefaultAsync();
      if (mjesto == null)
      {
        return NotFound($"Mjesto s ključem {key} ne postoji");
      }
      else
      {
        return Ok(mjesto);
      }
    }

    // POST /odata/Cities
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Mjesto model)
    {     
      logger.LogTrace(JsonSerializer.Serialize(model));

      if (model != null && ModelState.IsValid)
      {
        var mjesto = new EFModel.City
        {
          CityName = model.NazivMjesta,
          PostalCode = model.PostBrojMjesta,
          PostalName = model.PostNazivMjesta,
          CountryCode = model.OznDrzave
        };
       
        ctx.Add(mjesto);
        await ctx.SaveChangesAsync();
        model.IdMjesta = mjesto.CityId;

        return Created(model);
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    // PUT /odata/Cities(key)
    [HttpPut]
    public async Task<IActionResult> Put(int key, [FromBody] Mjesto model)
    {    
      logger.LogTrace(JsonSerializer.Serialize(model));

      if (model == null || model.IdMjesta != key || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      else
      {
        var mjesto = await ctx.Cities.FindAsync(key);
        if (mjesto == null)
        {
          return NotFound("Traženo mjesto ne postoji");
        }
        else
        {
          mjesto.CityName = model.NazivMjesta;
          mjesto.PostalCode = model.PostBrojMjesta;
          mjesto.PostalName = model.PostNazivMjesta;
          mjesto.CountryCode = model.OznDrzave;

          await ctx.SaveChangesAsync();
          return Updated(model);
        };
      }
    }

    // PATCH /odata/Cities(key)
    [HttpPatch]
    public async Task<IActionResult> Patch(int key, [FromBody] Delta<Mjesto> model)
    {
      foreach (var changedProp in model.GetChangedPropertyNames())
      {
        if (model.TryGetPropertyValue(changedProp, out var value))
        {
          logger.LogTrace($"Changing {changedProp} to {value}");
        }
      }

      if (model == null || !ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      else
      {
        var mjesto = await ctx.Cities.FindAsync(key);
        if (mjesto == null)
        {
          return NotFound("Traženo mjesto ne postoji");
        }
        else
        {                    
          var viewmodel = new Mjesto
          {
            IdMjesta = mjesto.CityId,
            NazivMjesta = mjesto.CityName,
            PostBrojMjesta = mjesto.PostalCode,
            PostNazivMjesta = mjesto.PostalName,
            OznDrzave = mjesto.CountryCode,
          };

          model.Patch(viewmodel);

          mjesto.CityName = viewmodel.NazivMjesta;
          mjesto.PostalCode = viewmodel.PostBrojMjesta;
          mjesto.PostalName = viewmodel.PostNazivMjesta;
          mjesto.CountryCode = viewmodel.OznDrzave;

          await ctx.SaveChangesAsync();
          return Updated(viewmodel);
        };
      }
    }

    // DELETE /odata/Cities(key)   
    [HttpDelete]
    public async Task<IActionResult> Delete(int key)
    {
      var mjesto = await ctx.Cities.FindAsync(key);
      if (mjesto == null)
      {
        return NotFound("Traženo mjesto ne postoji");
      }
      else
      {
        ctx.Remove(mjesto);
        await ctx.SaveChangesAsync();
        return NoContent();
      }
    }
  }
}

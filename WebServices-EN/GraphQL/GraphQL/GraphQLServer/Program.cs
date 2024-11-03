using EFModel;
using GraphQL.Server.Ui.Voyager;
using GraphQLServer.SetupGraphQL;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Configure services

// add/configure services

//Note: Cannot run in parallel, see https://chillicream.com/docs/hotchocolate/integrations/entity-framework for workaround
//builder.Services.AddDbContext<FirmContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Firm")));
//builder.Services.AddGraphQLServer()
//                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
//                .SetPagingOptions(new PagingOptions
//                {
//                  DefaultPageSize = 20,
//                  MaxPageSize = 1000,
//                  IncludeTotalCount = true
//                })
//                .AddProjections()
//                .AddFiltering()
//                .AddSorting()
//                .AddQueryType<Queries>()
//                .AddMutationType<Mutations>();
#endregion

var app = builder.Build();

#region Configure middleware pipeline.
//// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#middleware-order-1

//if (app.Environment.IsDevelopment())
//{
//  app.UseDeveloperExceptionPage();
//}

//app.UseStaticFiles();

//app.UseRouting();

//app.MapGraphQL();

//app.UseGraphQLVoyager("/voyager", new VoyagerOptions() { GraphQLEndPoint = "graphql" });
//app.UseGraphQLPlayground(
//    "/",                               // url to host Playground at
//    new GraphQL.Server.Ui.Playground.PlaygroundOptions
//    {
//      GraphQLEndPoint = "/graphql",         // url of GraphQL endpoint
//      SubscriptionsEndPoint = "/graphql",   // url of GraphQL endpoint
//    }); 
#endregion

app.Run();
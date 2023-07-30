namespace WebApplication1

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.OpenApi.Models
open WebApplication1.Data
open Microsoft.EntityFrameworkCore

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddDbContext<AppDbContext>(fun options ->
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            |> ignore)

        builder.Services.AddMemoryCache()

        builder.Services.AddControllers()

        builder.Services.AddApiVersioning()

        builder.Services.AddSwaggerGen(fun c -> c.SwaggerDoc("v1", OpenApiInfo(Title = "My API", Version = "v1")))

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.UseSwagger()
        app.UseSwaggerUI(fun c -> c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"))

        app.Run()

        exitCode

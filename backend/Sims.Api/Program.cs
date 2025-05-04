using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Helper;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithJwt();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDependencyInjections();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

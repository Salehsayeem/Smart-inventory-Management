using Dapper;
using Microsoft.EntityFrameworkCore;
using Sims.Api.Context;
using Sims.Api.Helper;
using System.Text.Json.Serialization;
using static Sims.Api.Helper.CommonHelper;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;


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
    options.UseNpgsql(connectionString));

builder.Services.AddSingleton<AppSettings>(sp =>
{
    var settings = new AppSettings
    {
        ConnectionString = connectionString
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found")
    };
    return settings;
});

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
SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
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

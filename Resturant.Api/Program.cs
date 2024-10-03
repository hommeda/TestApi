using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Resturant.Core;
using Resturant.Core.Utilities;
using Resturant.Data;
using Resturant.Service;
using Serilog;
using System.Globalization;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ResturantDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ResturantDbDapperContext>();

builder.Services.AddMvc().AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResource));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;
});

builder.Services.AddCors(p => p.AddPolicy("CorsPolicy", options =>
{
    options.WithOrigins(builder.Configuration.GetSection("AppSettings:UIURL").Value).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name:"CorsPolicy", policy => policy.WithOrigins(builder.Configuration.GetSection("AppSettings:UIURL").Value).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
//});

//Log data By Serilog
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
//end Log

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IUnitOfWorkDb, UnitOfWorkDb>();

#region Bs
builder.Services.AddTransient<ILookUpService, LookUpService>();
#endregion


//services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();

//services.AddHostedService<TimedHostedService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                    new CultureInfo("ar-EG"),
                    new CultureInfo("en-US")
                };
    options.DefaultRequestCulture = new RequestCulture("ar-EG", "ar-EG");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    var defaultCookieRequestProvider =
    options.RequestCultureProviders.FirstOrDefault(rcp =>
         rcp.GetType() == typeof(CookieRequestCultureProvider));

    if (defaultCookieRequestProvider != null)
    {
        options.RequestCultureProviders.Remove(defaultCookieRequestProvider);
    }

    options.RequestCultureProviders.Insert(0,
        new CookieRequestCultureProvider()
        {
            CookieName = CookieRequestCultureProvider.DefaultCookieName,
            Options = options
        });
});

builder.Services.Configure<IISOptions>(options =>
{
    options.ForwardClientCertificate = false;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PointSales.Api", Version = "v1" });
    //options.DocInclusionPredicate((docName, apiDesc) =>
    //{
    //    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
    //    // Exclude all DevExpress reporting controllers
    //    return !methodInfo.DeclaringType.AssemblyQualifiedName.StartsWith("DevExpress", StringComparison.OrdinalIgnoreCase);
    //});
});

var app = builder.Build();

#region Configure
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{

    app.UseDeveloperExceptionPage();
    //app.UseHsts();

    app.UseExceptionHandler(builder =>
    {
        builder.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
                context.Response.AddApplicationError(error.Error.Message);
                await context.Response.WriteAsync(error.Error.Message);
            }
        });
    });
    app.UseRequestLocalization(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

   

    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Contents")),
        RequestPath = new PathString("/Contents")
    });

    app.UseStaticFiles();

    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PointSales.Api v1");
    });

}


app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());
app.UseRouting();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();
//app.MapHub<DepartmentStatusHub>("/status");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //endpoints.MapHub<MyHub>("/notify");
});

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Contents")),
    RequestPath = new PathString("/Contents")
});

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
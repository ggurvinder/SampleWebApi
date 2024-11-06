using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Data;
using SampleWebApi.Data.Repositories;
using SampleWebApi.Mappings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SampleWebApi.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using Microsoft.OpenApi.Models;
using Serilog;
using SampleWebApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
// add services to the controller
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();


// Add services to the container.
var logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .WriteTo.File("Logs/Nzwalks_log.txt",rollingInterval:RollingInterval.Day)
                 .MinimumLevel.Warning()
                 .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1",new OpenApiInfo { Title="NZ Walk API",Version="v1"});
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme { 
        Name="Authorization",
        In=ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey,
        Scheme=JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference=new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id=JwtBearerDefaults.AuthenticationScheme

            },
            Scheme="Oauth2",
            Name=JwtBearerDefaults.AuthenticationScheme,
            In=ParameterLocation.Header
        },
        new List<string>()
    }
    });

});

builder.Services.AddDbContext<NZWalksDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SampleWebApiConnection"));
    //options.UseSqlite(builder.Configuration.GetConnectionString("SampleWebApiConnectionSqlite"));

});

//builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<NZWalksAuthDbContext>();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options => { 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});



builder.Services.AddDbContext<NZWalksAuthDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SampleAuthWebApiConnection"));

});


builder.Services.AddDbContext<NZWalksSqliteDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("SampleWebApiConnectionSqlite"));
});

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<NZWalksAuthSqliteDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("SampleWebApiConnectionSqlite"));
});

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddScoped<IImageRepository, LocalImageRepository>();



builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        AuthenticationType="Jwt",
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //ValidAudience = builder.Configuration["Jwl:Audience"],
        ValidAudiences = new[] { builder.Configuration["Jwt:Audience"] },
        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        

    });
    



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<SampleWebApi.Middlewares.ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath="/Images"
});


app.MapControllers();

app.Run();

using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using API_Dinamis;
using System.Text;
using API_Dinamis.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddTransient<Seed>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<RepositoryUtils>();
builder.Services.AddScoped<RegionUtils>();
builder.Services.AddScoped<DtoUtils>();

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IKeyRepository, KeyRepository>();
builder.Services.AddScoped<IFirstInitializeRepository, FirstInitializeRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IZipCodeRepository, ZipCodeRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IStandardValueRepository, StandardValueRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<KeyRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddDbContext<DataContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
});



//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowOrigin",
//        builder => builder.WithOrigins("https://192.168.100.32:5671"));
//    options.AddPolicy("NewPolicy", builder =>
//    builder.AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader());
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("https://localhost:65285"));
    options.AddPolicy("AllowAll", builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Content-Disposition"));
});


//builder => builder.WithOrigins("https://localhost:44398", "https://192.168.100.29:44398", "http://192.168.100.29:44398"));

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(o =>
//{
//    o.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = false,
//        ValidateIssuerSigningKey = true
//    };
//});
//builder.Services.AddAuthorization();
//// Add configuration from appsettings.json
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddEnvironmentVariables();


var app = builder.Build();
//app.UseCors("AllowOrigin");
app.UseCors("AllowAll");

//var dbContext = app.Services.GetRequiredService<DataContext>();

//// Example: Retrieve all entities from the database
//var entities = dbContext.Keys.ToList();

//if (args.Length == 1 && args[0].ToLower() == "seeddata")
//    SeedData(app);

//void SeedData(IHost app)
//{
//    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
//    using (var scope = scopedFactory.CreateScope())
//    {
//        var service = scope.ServiceProvider.GetService<Seed>();
//        service.SeedDataContext();
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseAuthorization();
//IConfiguration configuration = app.Configuration;
//IWebHostEnvironment environment = app.Environment;


app.MapControllers();

app.Run();

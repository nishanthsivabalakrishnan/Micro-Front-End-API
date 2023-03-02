using MicroFrontendDal.BusinessRules.AppDbContext;
using MicroFrontendDal.BusinessRules.Authentication;
using MicroFrontendDal.BusinessRules.Logger;
using MicroFrontendDal.BusinessRules.Management;
using MicroFrontendDal.BusinessRules.Products;
using MicroFrontendDal.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


#region Program.cs Region
Log logger = new();
try
{
    var builder = WebApplication.CreateBuilder(args);
    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    //services cors
    builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));
    //Area to add code
    //Retrive from AppSettings
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    string jwtSecret = configuration["JWT:IssuerSignInKey"];
    //Initialize Database
    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
    builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(48));
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
    builder.Services.AddDbContext<MicroFrontEndDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
    //CORS Policy
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(x =>
        {
            x.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true);
        });
    });
    //JWT Authentication
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
    //Add Services
    builder.Services.AddScoped<IAuthentication, Authentication>();
    builder.Services.AddScoped<IProducts, Products>();
    builder.Services.AddScoped<IManagement,Management>();
    builder.Services.AddSingleton<Log>();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseAuthentication();

    app.UseCors("corsapp");

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseCors();

    app.MapControllers();

    logger.InfoLog("Program.cs", "Application Started Successfully");

    app.Run();
}
catch (Exception ex)
{
    logger.ErrorLog("Program.cs", "Program.cs", ex);
}
#endregion
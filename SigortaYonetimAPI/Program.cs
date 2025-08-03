using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SigortaYonetimAPI.Models;
using SigortaYonetimAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 32;
    });

// CORS ayarları ekle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// DbContext'i ekle
builder.Services.AddDbContext<SigortaYonetimDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity yapılandırması
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Şifre politikaları
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    
    // Kullanıcı politikaları
    options.User.RequireUniqueEmail = true;
    
    // E-posta doğrulama (şimdilik kapalı)
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<SigortaYonetimDbContext>()
.AddDefaultTokenProviders();

// JWT Ayarları
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secretKey = jwtSettings["SecretKey"];
var key = Encoding.UTF8.GetBytes(secretKey!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Servis kayıtları
builder.Services.AddScoped<ITokenService, TokenService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirect'i devre dışı bırak (sadece HTTP kullan)
// app.UseHttpsRedirection();

// CORS middleware'ini ekle
app.UseCors("AllowReactApp");

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Veritabanı seed verilerini oluştur
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SigortaYonetimDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    try
    {
        // Veritabanı yoksa oluştur
        if (!await context.Database.CanConnectAsync())
        {
            await context.Database.EnsureCreatedAsync();
        }
        
        // Seed verilerini oluştur
        await SeedData.InitializeAsync(app.Services);
        
        // Rolleri oluştur (eğer yoksa)
        var roles = new[] { "ADMIN", "ACENTE", "KULLANICI" };
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole 
                { 
                    Name = role,
                    Aciklama = role switch
                    {
                        "ADMIN" => "Sistem Yöneticisi",
                        "ACENTE" => "Acente Kullanıcısı", 
                        "KULLANICI" => "Normal Kullanıcı",
                        _ => ""
                    }
                });
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veritabanı hazırlanırken hata: {ex.Message}");
    }
}

app.Run();
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QlNhanSu_Backend.Models;
using QlNhanSu_Backend.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseInMemoryStorage());

builder.Services.AddHangfireServer();


var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<QlNhanSuContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["access"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IDiemDanhService, DiemDanhService>();
builder.Services.AddScoped<ICaLamService, CaLamService>();
builder.Services.AddScoped<IAutoCallService, AutoCallService>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();
builder.Services.AddScoped<IQuyenService, QuyenService>();
builder.Services.AddScoped<IVaiTroService, VaiTroService>();
builder.Services.AddScoped<IChucVuService, ChucVuService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowCredentials()
                  .AllowAnyMethod();
            // policy.WithOrigins("http://192.168.1.46:5173") 
            // .AllowAnyHeader()
            // .AllowCredentials()
            // .AllowAnyMethod();
        });
});





var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
app.UseHangfireDashboard();


using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IAutoCallService>();
    RecurringJob.AddOrUpdate("call-api-job",
            () => service.CallApi(),
            Cron.Daily,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

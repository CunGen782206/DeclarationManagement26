using DeclarationManagement.Api.Data;
using DeclarationManagement.Api.Mapping;
using DeclarationManagement.Api.Repositories;
using DeclarationManagement.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controller 注入
builder.Services.AddControllers();

// Swagger（便于接口联调）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext 注册（连接字符串由 appsettings.json 提供）
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper 注册
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repository 与 Service 注册
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDeclarationService, DeclarationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

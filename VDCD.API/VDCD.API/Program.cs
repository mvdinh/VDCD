using Microsoft.EntityFrameworkCore;
using VDCD.BL.Services;
using VDCD.DL;
using VDCD.DL.Base;
using VDCD.DL.Interface;
using VDCD.DL.Repo;
using VDCD.DL.ConnectDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure EF Core with PostgreSQL
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("VDCD.DL"))); // Migrations are in VDCD.DL project

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Cửa Hàng Gốm Sứ VDCD API",
        Version = "v1",
        Description = "Hệ thống API quản lý bán hàng cho cửa hàng gốm sứ"
    });
});

// Register Repositories/DLs
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped<IProductDL, ProductDL>();
builder.Services.AddScoped<IOrderDL, OrderDL>();

// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cửa Hàng Gốm Sứ VDCD API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Serilog;
using WalletLibrary.GoogleWallet.ServiceCollectionExtensions;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

// 設定 Serilog 日誌
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // 設定最低日誌級別
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    ) // 控制台日誌格式
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // 每日生成新的 log 檔案
    .CreateLogger();

builder.Host.UseSerilog(); // 註冊 Serilog 到 ASP.NET Core

builder.Services.AddGoogleWalletServices(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

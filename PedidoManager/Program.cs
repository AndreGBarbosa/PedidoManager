using PedidoManager.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using PedidoManager.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Serviços
builder.Services.AddControllersWithViews();

// 💾 Conexão com o banco e repositórios
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<PedidoRepository>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddTransient<IProdutoRepository, ProdutoRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();

var app = builder.Build();

// 🌐 Configuração de pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 🧭 Rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
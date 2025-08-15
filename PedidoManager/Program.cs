using Microsoft.Data.SqlClient;
using System.Data;
using PedidoManager.Repositories;
using PedidoManager.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Serviços MVC
builder.Services.AddControllersWithViews();

// 💾 Conexão com Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// 💾 Repositórios
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
builder.Services.AddTransient<IProdutoRepository, ProdutoRepository>();
builder.Services.AddTransient<IItemPedidoRepository, ItemPedidoRepository>();

builder.Services.AddSingleton<DbConnectionFactory>();

var app = builder.Build();

// 🌐 Pipeline de requisição
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
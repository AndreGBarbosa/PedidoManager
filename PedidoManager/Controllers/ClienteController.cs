using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;

namespace PedidoManager.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var clientes = await _clienteRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(busca))
                clientes = clientes.Where(c => c.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase)
                                            || c.Email.Contains(busca, StringComparison.OrdinalIgnoreCase));

            ViewBag.Busca = busca;
            return View(clientes);
        }

        public IActionResult Criar() => View();

        [HttpPost]
        public async Task<IActionResult> Criar(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            cliente.DataCadastro = DateTime.Now;
            await _clienteRepository.CreateAsync(cliente);

            TempData["Mensagem"] = "Cliente criado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            await _clienteRepository.UpdateAsync(cliente);
            TempData["Mensagem"] = "Cliente atualizado!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            await _clienteRepository.DeleteAsync(id);
            TempData["Mensagem"] = "Cliente excluído!";
            return RedirectToAction("Index");
        }
    }
}
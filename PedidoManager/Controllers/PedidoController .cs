using Microsoft.AspNetCore.Mvc;
using PedidoManager.Models;
using PedidoManager.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace PedidoManager.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;

        public PedidoController(IPedidoRepository pedidoRepo, IClienteRepository clienteRepo)
        {
            _pedidoRepository = pedidoRepo;
            _clienteRepository = clienteRepo;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return View(pedidos);
        }

        public async Task<IActionResult> Criar()
        {
            ViewBag.Clientes = await _clienteRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _clienteRepository.GetAllAsync();
                return View(pedido);
            }

            pedido.DataPedido = DateTime.Now;
            pedido.Status = "Novo";
            pedido.ValorTotal = 0; // Valor será calculado ao adicionar itens

            var pedidoId = await _pedidoRepository.CreateAsync(pedido);
            TempData["Mensagem"] = "Pedido criado com sucesso!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
                return NotFound();

            ViewBag.Clientes = await _clienteRepository.GetAllAsync();
            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Pedido pedido)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _clienteRepository.GetAllAsync();
                return View(pedido);
            }

            // Atualização de status ou cliente, sem alterar itens
            var atualizado = await _pedidoRepository.UpdateStatusAsync(pedido.Id, pedido.Status);
            if (atualizado)
                TempData["Mensagem"] = "Pedido atualizado!";
            else
                TempData["Mensagem"] = "Erro ao atualizar pedido.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            await _pedidoRepository.DeleteAsync(id);
            TempData["Mensagem"] = "Pedido excluído!";
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellnessNutricion.Api.Data;
using WellnessNutricion.Api.Models;

namespace WellnessNutricion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
    {
        return await _context.Clientes.ToListAsync();
    }

    // GET: api/Clientes/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }

        return cliente;
    }

    // POST: api/Clientes
    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCliente),
            new { id = cliente.Id },
            cliente
        );
    }

    // PUT: api/Clientes/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarCliente(
        int id,
        Cliente cliente)
    {
        var clienteExistente = await _context.Clientes.FindAsync(id);

        if (clienteExistente == null)
        {
            return NotFound();
        }

        clienteExistente.Nombre = cliente.Nombre;
        clienteExistente.Email = cliente.Email;
        clienteExistente.Telefono = cliente.Telefono;
        clienteExistente.Edad = cliente.Edad;
        clienteExistente.Objetivo = cliente.Objetivo;
        clienteExistente.Estado = cliente.Estado;
        clienteExistente.FenotipoCodigo = cliente.FenotipoCodigo;
        clienteExistente.PlanId = cliente.PlanId;
        clienteExistente.FechaInscripcion = cliente.FechaInscripcion;
        clienteExistente.FechaContacto = cliente.FechaContacto;
        clienteExistente.Notas = cliente.Notas;

        clienteExistente.FlagPagoRecibido = cliente.FlagPagoRecibido;
        clienteExistente.FlagEmailEnviado = cliente.FlagEmailEnviado;
        clienteExistente.FlagEfcaCompletado = cliente.FlagEfcaCompletado;
        clienteExistente.FlagConsentimiento = cliente.FlagConsentimiento;
        clienteExistente.FlagContactado = cliente.FlagContactado;
        clienteExistente.FlagWhatsapp = cliente.FlagWhatsapp;

        clienteExistente.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Clientes/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
        {
            return NotFound();
        }

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

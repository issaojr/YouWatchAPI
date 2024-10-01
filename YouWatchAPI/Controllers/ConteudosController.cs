/* 
 * Aluno: Issao Hanaoka Junior
 * Matrícula: 1885063
 * Disciplina: Projeto Integrador Multidisciplinar VIII
 * Graduação: Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas
 * Universidade Paulista - UNIP
 * 2024
 *
 * Descrição:
 * Este controlador gerencia as operações CRUD (Create, Read, Update, Delete) 
 * para a entidade "Conteudo" na aplicação YouWatchAPI. Ele permite listar 
 * conteúdos, visualizar detalhes, criar novos, editar e excluir conteúdos, 
 * utilizando o Entity Framework para interagir com o banco de dados.
 * O controlador também lida com relacionamentos, como a associação de um conteúdo a um criador, 
 * e realiza validações para garantir a integridade dos dados durante essas operações.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouWatchAPI.Data;
using YouWatchAPI.Models;

namespace YouWatchAPI.Controllers
{
    public class ConteudosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConteudosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Conteudoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Conteudos.Include(c => c.Criador);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Conteudoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conteudo = await _context.Conteudos
                .Include(c => c.Criador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conteudo == null)
            {
                return NotFound();
            }

            return View(conteudo);
        }

        // GET: Conteudoes/Create
        public IActionResult Create()
        {
            ViewData["CriadorId"] = new SelectList(_context.Criadores, "Id", "Id");
            return View();
        }

        // POST: Conteudoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Tipo,CriadorId")] Conteudo conteudo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conteudo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CriadorId"] = new SelectList(_context.Criadores, "Id", "Id", conteudo.CriadorId);
            return View(conteudo);
        }

        // GET: Conteudoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conteudo = await _context.Conteudos.FindAsync(id);
            if (conteudo == null)
            {
                return NotFound();
            }
            ViewData["CriadorId"] = new SelectList(_context.Criadores, "Id", "Id", conteudo.CriadorId);
            return View(conteudo);
        }

        // POST: Conteudoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Tipo,CriadorId")] Conteudo conteudo)
        {
            if (id != conteudo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conteudo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConteudoExists(conteudo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CriadorId"] = new SelectList(_context.Criadores, "Id", "Id", conteudo.CriadorId);
            return View(conteudo);
        }

        // GET: Conteudoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conteudo = await _context.Conteudos
                .Include(c => c.Criador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conteudo == null)
            {
                return NotFound();
            }

            return View(conteudo);
        }

        // POST: Conteudoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conteudo = await _context.Conteudos.FindAsync(id);
            if (conteudo != null)
            {
                _context.Conteudos.Remove(conteudo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConteudoExists(int id)
        {
            return _context.Conteudos.Any(e => e.Id == id);
        }
    }
}

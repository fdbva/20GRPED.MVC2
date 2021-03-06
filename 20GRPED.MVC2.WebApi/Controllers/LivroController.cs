﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _20GRPED.MVC2.Domain.Model.Entities;
using _20GRPED.MVC2.Domain.Model.Exceptions;
using _20GRPED.MVC2.Domain.Model.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace _20GRPED.MVC2.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivroController(
            ILivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroEntity>>> GetLivroEntity()
        {
            var livros = await _livroService.GetAllAsync();
            return livros.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LivroEntity>> GetLivroEntity(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var livroEntity = await _livroService.GetByIdAsync(id);

            if (livroEntity == null)
            {
                return NotFound();
            }

            return livroEntity;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivroEntity(int id, LivroEntity livroEntity)
        {
            if (id != livroEntity.Id)
            {
                return BadRequest();
            }

            try
            {
                await _livroService.UpdateAsync(livroEntity);
            }
            catch (EntityValidationException e)
            {
                ModelState.AddModelError(e.PropertyName, e.Message);
                return BadRequest(ModelState);
            }
            catch (RepositoryException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<LivroEntity>> PostLivroEntity(LivroEntity livroEntity)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                await _livroService.InsertAsync(livroEntity);

                return CreatedAtAction(
                    "GetLivroEntity", 
                    new { id = livroEntity.Id }, livroEntity);
            }
            catch (EntityValidationException e)
            {
                ModelState.AddModelError(e.PropertyName, e.Message);
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Livro/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LivroEntity>> DeleteLivroEntity(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var livroEntity = await _livroService.GetByIdAsync(id);
            if (livroEntity == null)
            {
                return NotFound();
            }

            await _livroService.DeleteAsync(id);

            return livroEntity;
        }
    }
}

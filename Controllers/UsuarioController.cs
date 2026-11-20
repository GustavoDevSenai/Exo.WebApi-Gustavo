using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exo.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public UsuariosController(
            UsuarioRepository usuarioRepository,
            IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        // GET: api/usuarios
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_usuarioRepository.Listar());
        }

        // POST: api/usuarios
        [HttpPost]
        public IActionResult Post(Usuario usuario)
        {
            Usuario usuarioBuscado = _usuarioRepository.Login(usuario.Email, usuario.Senha);

            if (usuarioBuscado == null)
            {
                return Unauthorized("E-mail ou senha inválidos!");
            }

            // Claims do usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuarioBuscado.Id.ToString())
            };

            // Chave secreta
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            // Credenciais
            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            // Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Usuario usuario = _usuarioRepository.BuscaPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/usuarios/{id}
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            _usuarioRepository.Atualizar(id, usuario);
            return NoContent();
        }

        // DELETE: api/usuarios/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _usuarioRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
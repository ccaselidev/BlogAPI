using AutoMapper;
using BlogAmaggiAPI.Data;
using BlogAmaggiAPI.Models;
using BlogAmaggiAPI.Services;
using BlogAmaggiAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAmaggiAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
    }

        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login([FromBody] UsuarioViewModel usuario)
        {            
            var token = AuthService.GenerateToken();
            return Ok(new { accessToken = token });            
        }

        [AllowAnonymous]
        [HttpPost("/registro")]
        public IActionResult CriarUsuario([FromBody] UsuarioViewModel usuario)
        {
            var usr = _mapper.Map<Usuario>(usuario);

            _context.Usuario.Add(usr);
            var usuarioSalvo = _context.SaveChanges();
            return usuarioSalvo > 0 ? Ok() : BadRequest("Nao foi possivel criar o usuario."); ;
        }
    }
}

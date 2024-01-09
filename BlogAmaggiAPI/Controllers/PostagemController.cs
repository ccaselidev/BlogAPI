using AutoMapper;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostagemController : ControllerBase
    {
        private HttpContext context;
        private readonly BaseContext _context;
        //private readonly WebSocket _webSocket;
        private readonly IMapper _mapper;

        public PostagemController(BaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("/postagem")]
        public IActionResult GetAll()
        {
            var postagens = _context.Postagem.ToList();
            return postagens.Count() > 0 ? Ok() : NotFound();
        }

        [Authorize]
        [HttpGet("/postagem/{id}")]
        public IActionResult GetById(int id)
        {
            var postagem = _context.Postagem.Find(id);
            return postagem == null ? NotFound() : Ok();
        }
       
        [Authorize]
        [HttpPost("/postagem")]
        public async Task<IActionResult> CriarPostagem([FromBody] PostagemViewModel body)
        {
            var postagem = _mapper.Map<Postagem>(body);

            _context.Postagem.Add(postagem);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                var mensagem = Encoding.ASCII.GetBytes("Sua postagem foi criada com sucesso!");
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                await webSocket.SendAsync(mensagem, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
            }

            return Created("", body);
        }
        
        [Authorize]
        [HttpPut("/postagem/{id}")]
        public IActionResult AlterarPostagem(int id, [FromBody] PostagemViewModel body)
        {
            var postagemSalva = _context.Postagem.Find(id);
            if (postagemSalva == null) return NotFound();

            var postagem = _mapper.Map<Postagem>(body);

            _context.Postagem.Update(postagem);
            var result = _context.SaveChanges();
            return result > 0 ? NoContent() : BadRequest("Nao foi possivel atualziar a postagem.");            
        }
        
        [Authorize]
        [HttpDelete("/postagem/{id}")]
        public IActionResult DeletarPostagem(int id)
        {
            var postagemSalva = _context.Postagem.Find(id);
            if (postagemSalva == null) return NotFound();

            _context.Postagem.Remove(postagemSalva);
            var result = _context.SaveChanges();
            return result > 0 ? NoContent() : BadRequest("Nao foi possivel remover a postagem.");
        }        
    }
}
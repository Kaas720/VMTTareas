using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using VMTTareas.Data;
using VMTTareas.Helpers;
using VMTTareas.Models;

namespace VMTTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredencialController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _accessor;
        public CredencialController(DataContext context, IConfiguration configuration, IHttpContextAccessor accessor)
        {
            _context = context;
            _configuration = configuration;
            _accessor = accessor;
        }

        // GET: api/Credencial
        [HttpPost("Login")]
        public async Task<ActionResult> LoginCliente(Credenciales dataUser)
        {
            var uset_temp = await _context.usuario.FirstOrDefaultAsync(x=> x.usuario.ToLower().Equals(dataUser.usuario));
            if (uset_temp == null)
            {
                return BadRequest("Usuario no encontrado");
            }
            else
            {
                if (uset_temp.passwordUser.Equals(dataUser.password))
                {
                    return Ok(JsonConvert.SerializeObject(CearToken(uset_temp)));
                }
                else{
                    return BadRequest("Contraseña incorrecta");
                }
            }
        }
        [Authorize]
        // GET: api/Personas/5
        [HttpGet("/BuscarUser")]
        public async Task<ActionResult<Usuario>> BuscarUsuario()
        {
            try
            {
                int id = DesifrarToken.JwtToPayloadUserData(_accessor.HttpContext);
                if (id != 0)
                {
                    var user = await _context.usuario.FindAsync(id);
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Token invalido");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        private string CearToken(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.idUsuario.ToString()), new Claim(ClaimTypes.Name, user.usuario)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
     
        private bool CredencialExists(int id)
        {
            return _context.usuario.Any(e => e.idUsuario == id);
        }
    }
}

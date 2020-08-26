using System.Linq;
using System.Threading.Tasks;
using CancelamentoIdentity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CancelamentoIdentity.Controllers
{
    [Route("v1/tipos")]
    public class TiposController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public TiposController(ApplicationDbContext context)
        {
            _ctx = context;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetList()
        {
            var data3 = from db in _ctx.Tipos
                        group db.Tipo by db.Tipo into g
                        select new { Id = g.Key};
                        
            return Ok(await data3.ToListAsync());
        }


        [HttpGet("bytipo/{filter}")]
        public IActionResult GetListTipos([FromRoute] string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                var matches = from m in _ctx.Tipos
                              where m.Tipo.Contains(filter)
                              select m;

                var ret = matches.ToList();
                return PartialView("_TiposCancelamentos", ret);
            }
            return BadRequest();
        }


        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
         {
            var ret = await _ctx.Tipos.FirstOrDefaultAsync(m => m.Id == id);
            return Json(ret);
        }
    }
}
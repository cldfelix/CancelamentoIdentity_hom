using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CancelamentoIdentity.Data;
using CancelamentoIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using log4net;
using System.Reflection;

namespace CancelamentoIdentity.Controllers
{

    [Route("v1/anexos")]


    public class AnexosController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ApplicationDbContext _ctx;
        IHostingEnvironment _appEnvironment;
        private readonly string _path;
        

        //private readonly string _path = @"C:\inetpub\wwwroot\cancelamentos\wwwroot\Resources\Images";

        public AnexosController(
            IHostingEnvironment env,
                 ApplicationDbContext ctx

                )
        {
            _ctx = ctx;
            _appEnvironment = env;
            _path = _appEnvironment.WebRootPath + "\\Resources\\";

        }

        [HttpPost("addFile")]
        [Authorize(Roles = "Criador,Editor")]
        public async Task<ActionResult> AddAnexosToCancelamento(FormAddAnexo formAnexo)
        {
            if (ModelState.IsValid)
            {
                var c = await _ctx.Cancelamentos.FindAsync(formAnexo.Idcancelamento);
                var a = await _ctx.Anexos.FindAsync(formAnexo.IdAnexo);

                c.Anexos.Add(a);

                _ctx.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }




        // GET: api/Anexos/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Criador,Editor,Leitor")]
        public async Task<ActionResult> GetAnexo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anexo = await _ctx.Anexos.FindAsync(id);

            if (anexo == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_path, anexo.NomeCriado);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            new FileExtensionContentTypeProvider().TryGetContentType(Path.GetFileName(filePath), out var contentType);
            return File(fileBytes, contentType ?? "application/octet-stream", anexo.NomeOriginal);

            //var memory = new MemoryStream();
            //    using (var stream = new FileStream(filePath, FileMode.Open))
            //    {

            //        await stream.CopyToAsync(memory);
            //    }

            //    memory.Position = 0;
            //    return File(memory, anexo.NomeOriginal);


        }





        // POST: api/Anexos
        [HttpPost]
        [DisableRequestSizeLimit]
        [Authorize(Roles = "Criador,Editor")]
        public async Task<ActionResult<dynamic>> PostAnexo()
        {
            Logger.Info("linha 117");

            var ret = new Anexo();
            try
            {
                var files = Request.Form.Files;




                var fileName = "";
                if (!Directory.Exists(_path))
                {
                    Logger.Info("diretorio não existe: " + _path);
                    Directory.CreateDirectory(_path);
                }

                var extPermitidas = new string[] { "doc", "pdf", "txt", "docx", "csv", "xls" };
                foreach (var file in files)
                {
                    var extencao = file.FileName.Split(".")[1];
                    //if (!extPermitidas.Contains(extencao))
                    //    return BadRequest("Arquivos com extensão" + extencao + " não sao permitidos");

                    fileName = Guid.NewGuid().ToString("N");

                    var filePath = Path.Combine(_path, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    ret.NomeOriginal = file.FileName;
                    ret.NomeCriado = fileName;
                    Logger.Info("diretorio não existe: " + ret.NomeCriado);

                    var dados = Request.Form["idCancelamento"];
                    if (!string.IsNullOrEmpty(dados))
                    {
                        var cancel = _ctx.Cancelamentos.Find(int.Parse(dados));
                        ret.Cancelamento = cancel;
                        _ctx.Anexos.Add(ret);
                        ret.Id = _ctx.SaveChangesAsync().Id;

                    }

                }
                return Ok(ret);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }



            //return CreatedAtAction("GetAnexo", new { id = anexo.Id }, anexo);
        }

        // POST: api/Anexoes
        [HttpDelete("{id}")]
        [Authorize(Roles = "Criador,Editor")]
        public async Task<ActionResult<dynamic>> AnexoDelete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var a = _ctx.Anexos.FirstOrDefault(x => x.Id == id);

            _ctx.Anexos.Attach(a);
            _ctx.Entry(a).State = EntityState.Deleted;
            await _ctx.SaveChangesAsync();


            var filePath = Path.Combine(_path, a.NomeCriado);

            if (System.IO.File.Exists(filePath))
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.Delete, 4096, true))
                {
                    await stream.FlushAsync();
                    System.IO.File.Delete(filePath);
                    return Ok();
                }
            }

            return NotFound();
        }

    }
}

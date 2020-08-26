using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CancelamentoIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using CancelamentoIdentity.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;
using log4net;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace CancelamentoIdentity.Controllers
{
   [Authorize]
    public class HomeController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AnexosController _anexoController;
        private readonly ApplicationDbContext _ctx;
        private readonly RoleManager<IdentityRole> _roleManager;

        public int TamPagina = 2;
        private readonly string[] _opTipos = new string[] {
               "Aeroportos",
                "Informática",
                "Diversos",
                "Tráfego Aéreo",
                "Safety/Security",
                "Infraestrutura",
                "Meteorologia",
                "Cargas",
                "Manutenção",
                    "Suprimentos"
            };

        public HomeController(ApplicationDbContext context, AnexosController anexoController, RoleManager<IdentityRole> rolesManager)
        {
            _ctx = context;
            _roleManager = rolesManager;
            _anexoController = anexoController;


        }

        public async Task<IActionResult> Index()
        {
            var c = await _ctx.Cancelamentos
               .Include("VooCancelado")
             .Include("VooAnterior")
             .Include("TipoCancelamento")
             .Include("Anexos")
             .Where(x=>  !x.Reativado)
              .OrderByDescending(p => p.DataCriacao).ToListAsync();

      

            return View(c);
        }

        [HttpGet("mostrar/{id}")]
        [Authorize(Roles = "Criador, Editor, Leitor")]
        public async Task<IActionResult> Mostrar([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancelamento = await _ctx.Cancelamentos
                 .Include("VooCancelado")
                .Include("VooAnterior")
                 .Include("TipoCancelamento")
                .Include("Anexos")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancelamento == null)
            {
                return NotFound();
            }

            return View(cancelamento);
        }


        [HttpGet("editar/{id}")]
        [Authorize(Roles = "Criador, Editor")]
        public async Task<IActionResult> Editar([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancelamento = await _ctx.Cancelamentos
                .Include("VooCancelado")
                .Include("VooAnterior")
                .Include("TipoCancelamento")
                .Include("Anexos")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancelamento == null)
            {
                return NotFound();
            }
            ViewBag.Tipos = _opTipos;

            var matches = from m in _ctx.Tipos
                          where m.Tipo.Contains(cancelamento.TipoCancelamento.Tipo)
                          select m;

            ViewBag.TiposSelecionados = matches.ToList();
            ViewBag.User = User.Identity.Name;

            return View(cancelamento);
        }


        [HttpPost("editar/{id}")]
        [Authorize(Roles = "Criador, Editor")]
        public async Task<IActionResult> Editar(int id, [Bind("Id,Observacao,VooCancelado,VooAnterior, Anexo,TipoCancelamento,Cancelado,AlvoDeProcesso,Observacao, CriadoPor,DataCriacao")] Cancelamento cancelamento)
        {
            if (id != cancelamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _ctx.Voos.Update(cancelamento.VooCancelado);
                    _ctx.SaveChanges();

                    cancelamento.TipoCancelamento = _ctx.Tipos.Where(x => x.Id == cancelamento.TipoCancelamento.Id).FirstOrDefault();

                    if (cancelamento.VooAnterior != null)
                    {
                        _ctx.Voos.Update(cancelamento.VooAnterior);
                        _ctx.SaveChanges();

                    }
                    else
                    {
                        cancelamento.VooAnterior = null;
                    }

                    cancelamento.DataAtualizacao = DateTime.Now;
                    cancelamento.AtualizadoPor = User.Identity.Name;

                    _ctx.Update(cancelamento);
                    await _ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancelamentoExists(cancelamento.Id))
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
            ViewBag.Tipos = _opTipos;

            var matches = from m in _ctx.Tipos
                          where m.Tipo.Contains(cancelamento.TipoCancelamento.Tipo)
                          select m;

            ViewBag.TiposSelecionados = matches.ToList();
            return View(cancelamento);
        }

        [HttpGet]
        [Authorize(Roles = "Criador")]
        public IActionResult Criar()
        {
            ViewBag.Tipos = _opTipos;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Criador")]
        public async Task<IActionResult> Criar([Bind("Observacao, VooCancelado,VooAnterior, EnviarEmail, EmailUsuario, SenhaUsuario, Anexo, TipoCancelamento, Cancelado, AlvoDeProcesso")] Cancelamento cancelamento)

        {
            if (ModelState.IsValid)
            {
                cancelamento.TipoCancelamento = _ctx.Tipos.Find(cancelamento.TipoCancelamento.Id);
                cancelamento.DataCriacao = DateTime.Now;
                cancelamento.CriadoPor = User.Identity.Name;
                cancelamento.DataAtualizacao = DateTime.Now;
                cancelamento.AtualizadoPor = User.Identity.Name;
                _ctx.Add(cancelamento);
                await _ctx.SaveChangesAsync();

                if (!string.IsNullOrEmpty(cancelamento.Anexo.NomeCriado))
                {
                    cancelamento.Anexo.Cancelamento = cancelamento;
                    _ctx.Anexos.Add(cancelamento.Anexo);

                    _ctx.SaveChanges();
                   
                }

                if (cancelamento.EnviarEmail)
                    sendEmail(cancelamento);


                return RedirectToAction(nameof(Index));
            }

            var t = new Filter
            {
                FilterName = _ctx.Tipos.Find(cancelamento.TipoCancelamento.Id).Tipo
            };
            ViewBag.Tipos = _opTipos;
            ViewBag.TiposSelect = GetListTipos(t);
            return View(cancelamento);
        }

        [HttpPost]
        [Authorize(Roles = "Criador")]
        public async Task<IActionResult> ReativarCancelamento([FromBody] Reativacao reat){

            var cancelamento = await _ctx.Cancelamentos
                     .Include("VooCancelado")
                    .Include("VooAnterior")
                     .Include("TipoCancelamento")
                    .Include("Anexos")
                    .FirstOrDefaultAsync(m => m.Id == reat.IdCancelamento);

            cancelamento.Reativado = true;

            cancelamento.EmailUsuario = reat.Email;
            cancelamento.SenhaUsuario = reat.SenhaEmail;

            sendEmail(cancelamento, true);
            _ctx.Cancelamentos.Update(cancelamento);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetListTipos([FromBody] Filter filter)
        {
            if (ModelState.IsValid)
            {
                var matches = from m in _ctx.Tipos
                              where m.Tipo.Contains(filter.FilterName)
                              select m;

                var ret = matches.ToList();
                if (User.Identity.Name == "cco@voegol.com.br")
                {
                    return PartialView("_TiposCancelamentos", ret);

                }
                else
                {
                    return PartialView("_TiposCancelamentosDisabled", ret);

                }
            }
            return BadRequest();
        }

        private bool CancelamentoExists(int id)
        {
            return _ctx.Cancelamentos.Any(e => e.Id == id);
        }

        private void sendEmail(Cancelamento can, bool vooReativado = false)
        {
            string to = "gr-gestao_slots@voegol.com.br, GR-ProgramacaoSAOOW@voegol.com.br"; //To address    
            //string to = "clfelix@voegol.com.br, macoliveira@voegol.com.br"; //To address    
            //string to = "clfelix@voegol.com.br"; //To address    
            string from =  can.EmailUsuario; //From address    
            MailMessage message = new MailMessage(from, to);
            var url = "http://10.4.213.90/Home/Mostrar/" + can.Id;

            var tipo = vooReativado ? "Reativamento do Voo: " : "Cancelamento do Voo: ";

            var body = string.Format($@"
            <h1>Cancelamento teste - Homologacao, desconsiderar cancelamento </h1>
       <h1>{ tipo + can.VooCancelado.NumeroDoVoo} Data: {can.VooCancelado.STD.ToString("dd/MM/yyyy HH:mm")}</h1>
            <table style=' 
            border: 2px solid #FFFFFF;
            width: 100%;
            text-align: left;
            border: 1px solid #000000;
            border-collapse: collapse;'>
                <tbody>
                    <tr>
                        <td style='width: 30%; border: 1px solid #000000;'>Motivo: </td>
                        <td style='width: 70%; border: 1px solid #000000;'>{can.TipoCancelamento.CodAdm}</td>                       
                    </tr>
                    <tr>
                        <td style='width: 30%; border: 1px solid #000000;'>Voo: </td>
                        <td style='width: 70%; border: 1px solid #000000;'>{can.VooCancelado.NumeroDoVoo}</td>                       
                    </tr>
                    <tr>
                        <td style='width: 30%; border: 1px solid #000000;'>Data: </td>
                        <td style='width: 70%; border: 1px solid #000000;'>{can.VooCancelado.STD.ToString("dd/MM/yyyy HH:mm")}</td>                       
                    </tr>
                    <tr>
                        <td style='width: 30%; border: 1px solid #000000;'>Etapa: </td>
                        <td style='width: 70%; border: 1px solid #000000;'>{can.VooCancelado.Origem + " | " + can.VooCancelado.Destino}</td>                       
                    </tr>
                    <tr>
                        <td style='width: 30%; border: 1px solid #000000;'>Obs: </td>
                        <td style='width: 70%; border: 1px solid #000000;'>{can.Observacao}</td>                       
                    </tr>
                    <tr>
                        <td style='width: 30%; border: 1px solid #000000;'>N.° de Passageiros: </td>
                        <td style='width: 70%; border: 1px solid #000000;'>{can.VooCancelado.QtdPassageiros}</td>                       
                    </tr>
                </tbody>
            </table> 
          

");

            if (vooReativado)
            {
                body = body + string.Format($@"<a href='{url}'>Detalhes do voo cancelado: <em>{can.VooCancelado.NumeroDoVoo} do dia {can.VooCancelado.STD.ToString("dd/MM/yyyy HH:mm")}</em> </a>");
            }




            message.Subject = string.Format($@"{tipo + can.VooCancelado.NumeroDoVoo} Data: {can.VooCancelado.STD.ToString("dd/MM/yyyy HH:mm")} ");
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(can.EmailUsuario, can.SenhaUsuario);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                RedirectToAction(nameof(Index));
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

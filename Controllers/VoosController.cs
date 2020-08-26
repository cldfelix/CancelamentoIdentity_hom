using System;
using System.Collections.Generic;
using System.IO;
using CancelamentoIdentity.InfraEstruture;
using CancelamentoIdentity.Models;
using CancelamentoIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CancelamentoIdentity.Controllers
{
    [Authorize(Roles = "Criador, Editor")]
    public class VoosController : Controller
    {
        IHostingEnvironment _appEnvironment;
        private readonly string _path;

        public VoosController(IHostingEnvironment env)
        {
            _appEnvironment = env;
            _path = _appEnvironment.WebRootPath + "\\Resources\\";
        }

        [HttpPost]
        public IActionResult BuscarVooCancelado([FromBody] VooForm v)
        {
            try
            {
                    var listaVoosPortalEscala = new List<Cancelamento>();
                var listaVoos = new List<Voo>();
                var pe = PortalEscalaService.GetDadosDoVoo(v.DataDoVoo, v.NumeroDoVoo);
                if (pe == null) return this.StatusCode(StatusCodes.Status404NotFound, "Voo não encontrado no portal escala");

                foreach (var p in pe)
                {
                    var c = new Cancelamento();
                    c.VooCancelado = new Voo();
                    c.VooCancelado.MetarOrigem = GerMetar(p.scheduledAirportDeparture, p.scheduledDeparture);
                    c.VooCancelado.MetarDestino = GerMetar(p.scheduledAirportArrival, p.scheduledArrival);
                    c.VooCancelado.Origem = p.scheduledAirportDeparture;
                    c.VooCancelado.Destino = p.scheduledAirportArrival;
                    c.VooCancelado.Matricula = p.aircraftRegistration;
                    c.VooCancelado.TipoVoo = p.legType;
                    c.VooCancelado.STA = p.scheduledArrival;
                    c.VooCancelado.STD = p.scheduledDeparture;
                    c.VooCancelado.NumeroDoVoo = p.flightNumber;
                    c.VooCancelado.DataVoo = p.scheduledDeparture;
                    listaVoos.Add(c.VooCancelado);
                    listaVoosPortalEscala.Add(c);
                };

                if (listaVoosPortalEscala.Count > 1)
                {
                    var filePath = Path.Combine(_path, "voosMultiplos.json");
                    string json = JsonConvert.SerializeObject(listaVoos);
                    System.IO.File.WriteAllText(filePath, json);
             
                }
                    return PartialView("_ComboVoosCancelados", listaVoosPortalEscala);
         
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno: " + e.Message);
            }


        }

        [HttpGet]
        public IActionResult BuscarVooAnterior()
        {
            var c = new Cancelamento();
            c.VooCancelado = new Voo();
            return PartialView("_VooAnterior", c);

        }


        [HttpPost]
        public IActionResult BuscarVooAnterior([FromBody] VooForm v = null)
        {
            try
            {
                var listaVoosPortalEscala = new List<Cancelamento>();
                var listVooAnteriores= new List<Voo>();

                var pe = PortalEscalaService.GetDadosDoVoo(v.DataDoVoo, v.NumeroDoVoo);
                if (pe == null) return this.StatusCode(StatusCodes.Status404NotFound, "Voo não encontrado no portal escala");

                foreach (var p in pe)
                {
                    var c = new Cancelamento();
                    c.VooAnterior = new Voo();
                    c.VooAnterior.MetarOrigem = GerMetar(p.scheduledAirportDeparture, p.scheduledDeparture);
                    c.VooAnterior.MetarDestino = GerMetar(p.scheduledAirportArrival, p.scheduledArrival);
                    c.VooAnterior.Origem = p.scheduledAirportDeparture;
                    c.VooAnterior.Destino = p.scheduledAirportArrival;
                    c.VooAnterior.Matricula = p.aircraftRegistration;
                    c.VooAnterior.TipoVoo = p.legType;
                    c.VooAnterior.STA = p.scheduledArrival;
                    c.VooAnterior.STD = p.scheduledDeparture;
                    c.VooAnterior.NumeroDoVoo = v.NumeroDoVoo;
                    c.VooAnterior.DataVoo = p.scheduledArrival;

                    listaVoosPortalEscala.Add(c);
                    listVooAnteriores.Add(c.VooAnterior);

                }

                if (listaVoosPortalEscala.Count > 1)
                {
                    var filePath = Path.Combine(_path, "voosMultiplosAnteriores.json");
                    string json = JsonConvert.SerializeObject(listVooAnteriores);
                    System.IO.File.WriteAllText(filePath, json);

                }

                    return PartialView("_ComboVoosAnteriores", listaVoosPortalEscala);


            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno: " + e.Message);
            }


        }


        private string GerMetar(string airport, DateTime dataVoo)
        {
            try
            {
                RedemetServices redemet = new RedemetServices();
                var metar = redemet.RetrieveMeteorologyData(airport, dataVoo, dataVoo);
                return metar;
            }
            catch (Exception)
            {
                return airport + " na data: " + dataVoo.ToString() + " nao encontrado";
            }
        }


    }
}
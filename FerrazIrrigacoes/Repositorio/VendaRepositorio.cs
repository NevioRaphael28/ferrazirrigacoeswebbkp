using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FerrazIrrigacoes.Models;


namespace FerrazIrrigacoes.Repositorio
{
    public class VendaRepositorio
    {
        private sakitadbEntities db = new sakitadbEntities();

        public int GerarNovaVenda()
        {
            Venda objinserir = new Venda();
            objinserir.DataVenda = DateTime.Now;

            db.Venda.Add(objinserir);
            db.SaveChanges();

            return objinserir.Id;
        }

    }
}
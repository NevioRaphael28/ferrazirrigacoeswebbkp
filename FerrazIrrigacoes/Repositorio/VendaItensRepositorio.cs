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
    public class VendaItensRepositorio
    {
        private sakitadbEntities db = new sakitadbEntities();

        public void InserirItem(ItensVenda objdados)
        {
            db.ItensVenda.Add(objdados);
            db.SaveChanges();
        }

    }
}
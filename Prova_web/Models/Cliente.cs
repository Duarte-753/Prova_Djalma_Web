﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prova_web.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string EMail { get; set; }
        public string DataNasc { get; set; }
        public string Vendedor { get; set; }


    }
}
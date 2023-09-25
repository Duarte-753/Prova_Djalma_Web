using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prova_web.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int EAN { get; set; }
        public string PreçoVenda { get; set; }
        public string Marca_P { get; set; }
        public int Estoque { get; set; }

    }
}
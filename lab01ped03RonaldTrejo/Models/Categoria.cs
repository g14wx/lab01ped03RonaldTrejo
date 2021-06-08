using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lab01ped03RonaldTrejo.Models
{
    public class Categoria
    {
        public int ID { get; set; }
        public string Cat { get; set; }
        public string ImagenCat { get; set; }
        public ICollection<Juego> Juegos { get; set; }
    }
}
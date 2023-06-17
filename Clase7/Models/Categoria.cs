using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Clase7.Models
{
    public class Categoria
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Debe ingresar el Id Categoría.")]
        public int IdCategoria { get; set; }

        [Display(Name = "Nombre Categoría")]
        [Required(ErrorMessage = "Debe ingresar el nombre de la categoría.")]
        public string NombreCategoria { get; set; }

        [Display(Name ="Descripción")]
        public string Descripcion { get; set; }
    }
}
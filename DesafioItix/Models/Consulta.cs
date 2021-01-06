using System;
using System.ComponentModel.DataAnnotations;

namespace DesafioItix.Models
{
    public class Consulta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public DateTime DataInicial { get; set; }

        [Required]
        public DateTime DataFinal { get; set; }

        public string Observacoes { get; set; }
    }
}

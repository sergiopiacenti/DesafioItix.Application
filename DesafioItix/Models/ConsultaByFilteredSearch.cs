using System;

namespace DesafioItix.Models
{
    public class ConsultaByFilteredSearch
    {

        public int? Id { get; set; }

        public string Nome { get; set; }

        public DateTime? DataNascimento { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public string Observacoes { get; set; }
    }
}

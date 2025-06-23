using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class TransaccionInterbancaria
    {
        public int Id { get; set; }

        public string NumeroTransaccion { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public string NumeroCuentaDestino { get; set; }
        public string BancoDestino { get; set; }

        public decimal Monto { get; set; }
        public string Moneda { get; set; } // "COP" o "USD"
        public decimal MontoConvertidoCOP { get; set; }

        public string Estado { get; set; } // "pendiente", "procesado"
        public DateTime Fecha { get; set; }

        public Guid CuentaOrigenId { get; set; }
        public Cuenta CuentaOrigen { get; set; }
    }

}

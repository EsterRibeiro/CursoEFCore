using System;
using System.Collections.Generic;
using System.Text;

namespace Curso.ValueObjects
{
    public enum TipoFrete
    {
        CIF, //remetente paga o frete
        FOB, // destinatário paga
        SemFrete
    }
}

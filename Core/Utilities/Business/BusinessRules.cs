using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public static class BusinessRules
    {
        // ACIKLAMALAR
        // params ile IResult turunde 1 veya 1den cok degiskeni alip tekbir array altinda toplayabiliyoruz.
        // params ile gonderilen is kuralini business layerinda ilgili yere bildiriyoruz.
        // kuralin kendisi geri donduruluyor.

        public static IResult Run(params IResult[] logics)
        {
            foreach (var logic in logics)
            {
                if (!logic.Success) return logic;
            }

            return null;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public interface ICoreModule
    {
        // Business icerinde yazilan DependencyResolvers sadece projeye ozel bagimliliklari cozer.
        // Bu asamada tum projelerde ortak olan bagimliliklari cozecegiz
        void Load(IServiceCollection serviceCollection);
    }
}

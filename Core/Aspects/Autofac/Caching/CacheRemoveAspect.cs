using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching
{
    // Data bozuldugu zaman -> data eklenir, silinir, guncellenir yani CRUD islemlerinde kullanilir
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation) //-> metot basarili olursa calisacak. Add islemi basarisiz ise cache'i temizlemesine gerek yok yani
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}

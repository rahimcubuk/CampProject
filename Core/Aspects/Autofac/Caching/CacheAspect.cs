using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception // MethodInterception --> attribute demek
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60) //-> sure vermezsen 60dk cache de tututlur
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            // invocation.Method.ReflectedType.FullName -> metodun namespace + interfaceAdi --> Business.Abstract.IProductService
            // invocation.Method.Name -> metodun adi --> .GetAll()
            // invocation.Arguments.ToList(); --> metodun parametreleri yoksa null gecer
            // key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})"; --> key = Business.Abstract.IProductService.GetAll() / Business.Abstract.IProductService.GetById(1)

            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})"; //--> '??' soldaki ifadeden deger gelirse onu gelmezsa sagdakini ekle
            if (_cacheManager.IsAdd(key))
            {
                //cache de varsa return olustur cache ten metodu dondur
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            // cache de yoksa veritabanindan al ve cache ekle
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);
        }
    }
}

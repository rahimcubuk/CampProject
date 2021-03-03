using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.CCS;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region readme
            // WebAPI icerisinde startup.cs de IoC container icin yaptigimizin karsiligi
            // Burada yapmamizin sebebi ise farkli bir API kullanmak istedigimizde her seferinde
            //  tekrar tekrar IoC kurmak zorunda kalmamak icin.
            // Bu modulu olusturduktan sonra WebAPI program.cs de 
            /*
                public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacBusinessModule());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
             */
            // kodunu ekledik.
            #endregion

            #region Product
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();
            #endregion

            #region Category
            builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
            builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();
            #endregion

            #region User
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();
            #endregion

            #region Auth
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            #endregion

            #region Logger
            builder.RegisterType<FileLogger>().As<ILogger>().SingleInstance();
            #endregion

            //builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

            #region Aspect
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
               .EnableInterfaceInterceptors(new ProxyGenerationOptions()
               {
                   Selector = new AspectInterceptorSelector()
               }).SingleInstance();
            #endregion
        }
    }
}

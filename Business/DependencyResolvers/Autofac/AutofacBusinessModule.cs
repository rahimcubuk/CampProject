using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
            #region Product IoC Container Code Block
            builder.RegisterType<ProductManager>()
                   .As<IProductService>()
                   .SingleInstance();
            builder.RegisterType<EfProductDal>()
                   .As<IProductDal>()
                   .SingleInstance();
            #endregion

            #region Category IoC Container Code Block
            builder.RegisterType<CategoryManager>()
                   .As<ICategoryService>()
                   .SingleInstance();
            builder.RegisterType<EfCategoryDal>()
                   .As<ICategoryDal>()
                   .SingleInstance();
            #endregion
        }
    }
}

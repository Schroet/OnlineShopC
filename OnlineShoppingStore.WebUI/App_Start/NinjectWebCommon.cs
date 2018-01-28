using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Moq;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using OnlineShoppingStore.Domain.Abstract;
using OnlineShoppingStore.Domain.Concrete;
using OnlineShoppingStore.Domain.Entities;
using OnlineShoppingStore.WebUI.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace OnlineShoppingStore.WebUI.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>();

            kernel.Bind<IAuthentication>().To<FormsAuthenticationProvider>();

            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product> 
            //{
            //    new Product { Name = "Footbal", Price = 23},
            //    new Product { Name = "Surf board", Price = 179},
            //    new Product { Name = "Running shoes", Price = 95}
            //});

            //kernel.Bind<IProductRepository>().ToConstant(mock.Object);
        }
    }
}
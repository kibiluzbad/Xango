using System.Reflection;
using Blog.Domain;
using Blog.Domain.Queries;
using Blog.Repository.Queries;
using Blog.UI.App_Start;
using Blog.UI.Infra.AutoMapper;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using NHibernate;
using Ninject;
using Ninject.Web.Mvc;
using NinjectAdapter;
using WebActivator;
using Xango.Data;
using Xango.Data.NHibernate;
using Xango.Data.NHibernate.Configuration;
using Xango.Data.NHibernate.Queries;
using Xango.Data.Query;

[assembly: PreApplicationStartMethod(typeof (NinjectMVC3), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectMVC3), "Stop")]

namespace Blog.UI.App_Start
{
    public static class NinjectMVC3
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            NHFluentConfiguration.Init();
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));
            RegisterServices(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var locator = new NinjectServiceLocator(kernel);
            kernel.Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));
            kernel.Bind<IServiceLocator>().ToConstant(locator).InSingletonScope();
            kernel.Bind<ISessionFactory>().ToConstant(NHFluentConfiguration.SessionFactory).InSingletonScope();
            kernel.Bind<IQueryFactory>().To<QueryFactory>();
            kernel.Bind<IPagedPostSearch>().To<PagedPostSearch>();
            kernel.Bind<IFindPost>().To<FindPost>();
            kernel.Bind<IFindPostBySlug>().To<FindPostBySlug>();
            kernel.Bind<ILastPosts>().To<LastPosts>();
            kernel.Bind<IAdvancedPostSearch>().To<AdvancedPostSearch>();
        }
    }
}
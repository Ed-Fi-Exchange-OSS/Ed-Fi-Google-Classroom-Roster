[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WISEroster.Mvc.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WISEroster.Mvc.App_Start.NinjectWebCommon), "Stop")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace WISEroster.Mvc.App_Start
{

    using log4net;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.Mvc.FilterBindingSyntax;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using WISEroster.Business;
    using WISEroster.Domain.Api;
    using WISEroster.Mvc.Filters;
    using WISEroster.Mvc.ImplementationSpecific;


    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(x => x.FromAssemblyContaining(
                     typeof(IOrganizationBusiness),
                    typeof(IV3ApiDbContext)
                )
                .SelectAllClasses()
                .BindDefaultInterface()
                .Configure(c => c.InRequestScope()));

            kernel.Rebind<ISessionInfo>().To<SessionInfo>().InRequestScope().WithConstructorArgument("session", x => HttpContext.Current == null ?
                null :
                new HttpSessionStateWrapper(HttpContext.Current.Session));

            kernel.Bind<ILog>().ToMethod(context =>
                LogManager.GetLogger(context.Request.ParentContext.Plan.Type));

            kernel.BindFilter<DebugWamsAuthenticationFilter>(FilterScope.First, 0);

            kernel.BindFilter<CurrentAgencyFilter>(FilterScope.Global, 1)//.WhenActionMethodHasNo<NotFilteredAttribute>()
                .InRequestScope();

            kernel.Bind<System.Web.Http.Filters.IExceptionFilter>().To<Log4NetExceptionFilter>().InSingletonScope();
        }

    }
}
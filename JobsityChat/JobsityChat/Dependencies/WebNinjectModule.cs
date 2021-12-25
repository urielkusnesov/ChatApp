using System.Data.Entity;
using Model;
using Ninject.Modules;
using Ninject.Web.Common;
using Repository;
using Service.UserService;
using Service.PostService;

namespace JobsityChat.Dependencies
{
    public class WebNinjectModule : NinjectModule
    {
        public override void Load()
        {
            // Comunicacion Directa
            // Cuando se solicite una implementacion de IConfiguracionProvider o ConfiguracionProvider, ninject devolverá ConfiguracionProvider
            Bind<DbContext>().To<Context>().InRequestScope();
            Bind<IRepositoryService, RepositoryService>().To<RepositoryService>().InRequestScope();
            Bind<IUserService, UserService>().To<UserService>().InRequestScope();
            Bind<IPostService, PostService>().To<PostService>().InRequestScope();
        }

    }
}
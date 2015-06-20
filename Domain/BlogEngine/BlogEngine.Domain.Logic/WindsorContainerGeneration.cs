using System;
using System.Collections.Generic;
using System.Linq;
using BlogEngine.Domain.Implementations;
using BlogEngine.Domain.Models;
using BlogEngine.Repository;
using BlogEngine.Repository.Implementations;
using BlogEngine.Repository.Models;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Releasers;
using Castle.Windsor;
using UserManagementRepository.Implementations;
using UserManagementRepository.Interfaces;
using UserManagementService.Implementations;

namespace BlogEngine.Domain
{
    /// <summary>
    /// Used to inject all the required components for the blog engine application.
    /// </summary>
    public class WindsorContainerGeneration
    {
        public IWindsorContainer SetupWithWebRequest()
        {
            return Generate(c => c.LifestylePerWebRequest());
        }

        public WindsorContainer SetupWithThread()
        {
            return Generate(c => c.LifestylePerThread());
        }


        private WindsorContainer Generate(Action<ComponentRegistration> reg)
        {
            var container = new WindsorContainer();
            
            var types = new List<FromAssemblyDescriptor>();
            types.Add(Types.FromAssembly(typeof(GenericUserService<BlogUser, BlogUserEntity, int>).Assembly));
            types.Add(Types.FromAssembly(typeof(UserSettingsService).Assembly));
            types.Add(Types.FromAssembly(typeof(BlogRepository).Assembly));
            types.Add(Types.FromAssembly(typeof(BlogEntryService).Assembly));

            foreach (var type in types)
            {
                container.Register(type.Where(t => t.GetInterfaces().Any()).WithServiceAllInterfaces().Configure(reg));
            }
            
            //ensure the db context is passed in.
            container.Register(Component.For<IUserRepository<BlogUserEntity, int>>().LifeStyle.PerWebRequest.UsingFactoryMethod(() => new GenericUserRepository<BlogUserEntity, int>(new BlogEngineContext())));

            return container;
        }
    }
}

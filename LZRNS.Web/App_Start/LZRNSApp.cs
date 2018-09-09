using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Repository.Implementations;
using LZRNS.DomainModels.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Web;

namespace LZRNS.Web
{
    public class LZRNSApp : UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(LZRNSApp).Assembly);
            builder.RegisterApiControllers(typeof(LZRNSApp).Assembly);

            builder.RegisterControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);
            builder.Register(c => new UmbracoHelper(UmbracoContext.Current));

            //context
            builder.RegisterType<BasketballDbContext>();

            //register repositories
            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
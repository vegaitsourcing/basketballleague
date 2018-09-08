using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
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
        protected override void OnApplicationEnd(object sender, EventArgs e)
        {
            base.OnApplicationEnd(sender, e);

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(LZRNSApp).Assembly);
            builder.RegisterApiControllers(typeof(LZRNSApp).Assembly);

            builder.RegisterControllers(typeof(UmbracoApplication).Assembly);
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);
            builder.Register(c => new UmbracoHelper(UmbracoContext.Current));

            //register repositories



            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
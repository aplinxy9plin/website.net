﻿using ActiveCitizenWeb.StaticContentCMS.Configuration;
using Autofac;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ActiveCitizenWeb.StaticContentCMS.Startup))]

namespace ActiveCitizenWeb.StaticContentCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = RegisterDependencies(app);
            ConfigureAuth(app);

            using (var scope = container.BeginLifetimeScope())
            {
                var appSettings = scope.Resolve<IAppSettings>();
                if (appSettings.CreateDefaultCredentialsOnStart)
                {
                    CreateDefaultUsersAndRoles();
                }
            }
        }
    }
}

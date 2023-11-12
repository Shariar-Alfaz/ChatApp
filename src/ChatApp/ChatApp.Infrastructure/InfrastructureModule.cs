using Autofac;
using ChatApp.Application.Feature.Services;
using ChatApp.Infrastructure.Feature.Services.Email;

namespace ChatApp.Infrastructure
{
    public class InfrastructureModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}

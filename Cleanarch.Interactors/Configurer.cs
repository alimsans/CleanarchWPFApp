using System.Configuration;
using Autofac;
using Cleanarch.DomainLayer.Repositories;
using Cleanarch.Repository.DataProviders;

namespace Cleanarch.DomainLayer
{
    internal static class Configurer
    {
        public static IContainer Container { get; }

        static Configurer()
        {
            var builder = new ContainerBuilder();

            IDataProvider repository = null;

            if (ConfigurationManager.AppSettings["DataSource"] == "Local")
            {
                repository = new LocalDataProvider(ConfigurationManager.AppSettings["LocalStoragePath"]);
            }

            builder.RegisterInstance<IDataProvider>(repository);
            builder.RegisterType<Repositories.Repository>().As<IRepository>();

            Container = builder.Build();
        }
    }
}

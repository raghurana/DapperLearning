using DapperLearning.ConsoleApp.Data;
using DapperLearning.ConsoleApp.Data.Queries;
using DapperLearning.ConsoleApp.Utils;
using Ninject.Modules;

namespace DapperLearning.ConsoleApp
{
    public class DependencyMap : NinjectModule
    {
        public override void Load()
        {
            Bind<IConnectionStringProvider>().To<AppSettingConnectionStringProvider>().InSingletonScope();
            Bind<IDbConnectionProvider>().To<DefaultDbConnectionProvider>().InSingletonScope();
            Bind<IResourceReader>().ToConstant(new AssemblyResourceReader(GetType()));
            Bind<ICustomQueryProvider>().To<EmbeddedCustomQueryProvider>().InSingletonScope();
        }
    }
}
using DapperLearning.ConsoleApp.Data;
using Ninject.Modules;

namespace DapperLearning.ConsoleApp
{
    public class DependencyMap : NinjectModule
    {
        public override void Load()
        {
            Bind<IConnectionStringProvider>().To<AppSettingConnectionStringProvider>().InSingletonScope();
            Bind<IDbConnectionProvider>().To<DefaultDbConnectionProvider>().InSingletonScope();
        }
    }
}
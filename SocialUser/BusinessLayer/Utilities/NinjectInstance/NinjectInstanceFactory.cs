using BusinessLayer.DependencyResolvers.Ninject;
using Ninject;

namespace BusinessLayer.Utilities.NinjectInstance
{
    public static class NinjectInstanceFactory
    {
        public static T GetInstance<T>()
        {
            var kernel = new StandardKernel(new BusinessModule());
            return kernel.Get<T>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Feeder.Common.Factory
{
    public static class InstanceFactory
    {
        private static readonly UnityContainer sContainer = new UnityContainer();

        public static T GetInstance<T>()
        {
            return (T)sContainer.Resolve(typeof(T), new ResolverOverride[0]);
        }

        public static void Register<I, T>() where T : I
        {
            sContainer.RegisterType(typeof(I), typeof(T));
        }

        public static void RegisterSingleton<I, T>() where T : I
        {
            sContainer.RegisterType(typeof(I), typeof(T), new ContainerControlledLifetimeManager());
        }
    }
}

using System;
using Autofac;
using Delbert.Infrastructure.Abstract;

namespace Delbert.Infrastructure
{
    public class IoC : IIoC
    {
        private static IContainer _container;

        public void RegisterContainer(IContainer container)
        {
            _container = container;
        }

        public static IoC Instance
        {
            get { return _container.Resolve<IoC>(); }
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public T Resolve<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }
    }
}

using System;
using Autofac;

namespace Delbert.Infrastructure.Abstract
{
    public interface IIoC
    {
        void RegisterContainer(IContainer container);
        T Resolve<T>();
        object Resolve(Type type);
        T Resolve<T>(string name);
    }
}
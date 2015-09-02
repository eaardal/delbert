using System;
using Delbert.Infrastructure.Abstract;

namespace Delbert.Infrastructure
{
    public class EnvironmentAdapter : IEnvironmentAdapter
    {
        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
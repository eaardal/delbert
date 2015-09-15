using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delbert.Infrastructure.Abstract;

namespace Delbert.Infrastructure
{
    public class ProcessAdapter : IProcessAdapter
    {
        public void Start(string fileName)
        {
            Process.Start(fileName);
        }
    }
}

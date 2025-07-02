using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionProfiler.Core.Interfaces
{
    public interface ISecretResolver
    {
        Task<string> ResolveAsync(string secretKey);
    }
}

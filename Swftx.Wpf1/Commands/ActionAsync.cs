using System.Threading.Tasks;

namespace Swftx.Wpf.Commands
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}

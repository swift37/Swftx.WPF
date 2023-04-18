using System.Threading.Tasks;

namespace Swftx.Wpf.Commands
{
    public delegate Task ActionAsync();

    public delegate Task ActionAsync<in T>(T? parameter);
}

using System.Reflection;
using System.Threading.Tasks;

namespace DotNetCli.Extensions
{
    public static class MethodInfoExtensions
    {
        public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;

            //If the task has a generic type then get the result. Otherwise just return null as it is void...
            if (@this.ReturnType.IsGenericType)
                return awaitable.GetAwaiter().GetResult();

            return null;
        }
    }
}

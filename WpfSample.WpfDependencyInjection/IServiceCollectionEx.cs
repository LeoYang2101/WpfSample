using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample.WpfDependencyInjection
{
    public static class IServiceCollectionEx
    {
        public static void RegisterServiceFromAssembly(this IServiceCollection services)
        {

            var assembly = Assembly.GetAssembly(typeof(App));
            if (assembly == null) return;


            var types = assembly.GetTypes().ToList().Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && t.GetCustomAttributes(typeof(AutoRegisterService), false).Length > 0);

            types.ToList().ForEach(t =>
            {
                var attribute = t.GetCustomAttribute<AutoRegisterService>();
                if (attribute != null)
                {
                    var descriptor = new ServiceDescriptor(attribute.ServiceType, t, attribute.Lifetime);
                    if (descriptor != null)
                        services.Add(descriptor);
                }
            });
        }

    }



    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRegisterService : Attribute
    {
        public readonly Type ServiceType;
        public readonly ServiceLifetime Lifetime;

        public AutoRegisterService(Type ServiceType, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            this.ServiceType = ServiceType;
            Lifetime = lifetime;
        }
    }

}

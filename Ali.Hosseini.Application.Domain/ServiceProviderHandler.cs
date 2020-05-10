using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ali.Hosseini.Application.Domain
{
    public class ServiceProviderHandler
    {

        private static IServiceProvider CastleServiceContainer { get; set; }
        public static void Initialize(IServiceProvider castleServiceContainer)
        {
            CastleServiceContainer = castleServiceContainer;
        }
        public static TService GetService<TService>()
        {
            if (CastleServiceContainer == null) return default;
            return CastleServiceContainer.GetService<TService>();

        }
    }
}

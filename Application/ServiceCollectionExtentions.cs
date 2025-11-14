using Application.Factories;
using Application.Interfaces.Factories;
using Application.Interfaces.Managers;
using Application.Interfaces.Services;
using Application.Managers;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Application.Enums;

namespace Application
{
    public static class ServiceCollectionExtentions
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IWebPlaylistManager, WebPlaylistManager>();

            services.AddScoped<IGetWebPlaylistService, GetWebPlaylistService>();

            services.AddFactories(config);
        }
        private static void AddFactories(this IServiceCollection services, IConfiguration config)
        {
            var domens = GetDomens(config);

            services.AddScoped<IHtmlInstrumentsFactory>(k => new SimpleHtmlInstrumentsFactory(domens));
        }
        private static Dictionary<string, Domens> GetDomens(IConfiguration config)
        {
            var section = config.GetRequiredSection(nameof(Domens));

            return new Dictionary<string, Domens>()
            {
                { section.GetValue<string>(nameof(Domens.Amazon)), Domens.Amazon}
                
            };
        }
    }
}

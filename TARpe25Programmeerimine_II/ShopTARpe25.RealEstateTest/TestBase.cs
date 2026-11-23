using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.RealEstateTest.Macros;
using ShopTARpe25.RealEstateTest.Mock;

namespace ShopTARpe25.RealEstateTest
{
    // Baasklass testidele, mis paneb paika teenused ja testandmebaasi
    public abstract class TestBase
    {
        protected IServiceProvider ServiceProvider { get; set; }

        protected TestBase()
        {
            // Loob uue teenuste nimekirja
            var services = new ServiceCollection();
            // Lisab vajalikud teenused ja andmebaasi nimekirja
            SetupServices(services);
            // Ehitab teenuste pakkuja valmis, et teenuseid saaks kätte
            ServiceProvider = services.BuildServiceProvider();
        }

        // Meetod, mis seadistab teenused ja testandmebaasi
        public virtual void SetupServices(IServiceCollection services)
        {
            // Ühendab IRealEstateServices liidese RealEstateServices klassiga
            services.AddScoped<IRealEstateServices, RealEstateServices>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddSingleton<IHostEnvironment, MockIHostEnvironment>();

            // Seadistab testandmebaasi
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                // Kasutab päris andmebaasi asemel mälusisest andmebaasi "TEST"
                x.UseInMemoryDatabase("TEST");
                // Keelab transaktsioonide hoiatuse, sest mälusisene andmebaas ei toeta neid
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Registreerib kõik makrod
            RegisterMacros(services);
        }

        // Meetod ressursside puhastamiseks pärast testi (hetkel tühi)
        public void Dispose()
        {

        }

        // Abimeetod, millega saab testides lihtsalt teenuseid kätte
        protected T Svc<T>()
        {
            // Otsib ja tagastab küsitud teenuse
            return ServiceProvider.GetService<T>();
        }

        // Meetod, mis otsib ja lisab automaatselt kõik makrod
        private void RegisterMacros(IServiceCollection services)
        {
            // Võtab aluseks IMacros liidese
            var macroBaseType = typeof(IMacros);

            // Otsib projektist kõik tavalised klassid, mis kasutavad IMacros liidest
            var macros = macroBaseType.Assembly.GetTypes()
                 .Where(t => macroBaseType.IsAssignableFrom(t)
                 && !t.IsInterface && !t.IsAbstract);

            // Käib kõik leitud makrod ükshaaval läbi
            foreach (var macro in macros)
            {
                // Lisab makro teenuste hulka
                services.AddSingleton(macro);
            }
        }
    }
}

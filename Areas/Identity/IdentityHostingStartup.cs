using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CancelamentoIdentity.Areas.Identity.IdentityHostingStartup))]
namespace CancelamentoIdentity.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
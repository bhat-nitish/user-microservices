using Microsoft.Extensions.DependencyInjection;

namespace AdminService.Extensions
{
    public static class CorsExtensions
    {
        public static void RegisterCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "adminCorsPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200").AllowAnyHeader().WithMethods("GET", "POST","PUT","DELETE");
                                  });
            });
        }
    }
}

using ApplicantTracking.Infrastructure.Data;
using ApplicantTracking.Application.Abstractions;
using ApplicantTracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicantTracking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicantTrackingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ApplicantTrackingDb")));

        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<ITimelineRepository, TimelineRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}


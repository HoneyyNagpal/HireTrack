using HireTrack.Core.Interfaces;
using HireTrack.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HireTrack.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IInterviewRepository, InterviewRepository>();
            return services;
        }
    }
}

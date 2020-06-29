using Microsoft.Extensions.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAdvert.API.Services;

namespace WebAdvert.API.HealthChecks
{
    public class StoreageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService advertStorageService;

        public StoreageHealthCheck(IAdvertStorageService advertStorageService)
        {
            this.advertStorageService = advertStorageService;
        }

        public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            var isStorageOk = await advertStorageService.CheckHealthAsync();
            return HealthCheckResult.FromStatus(isStorageOk ? CheckStatus.Healthy : CheckStatus.Unhealthy, "");
        }

     
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUnityOfWork.Scheduler
{
    public class ScheduledJob : IJob
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ScheduledJob> logger;


        public ScheduledJob(IConfiguration configuration, ILogger<ScheduledJob> logger)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            this.logger.LogWarning($"Hello from scheduled task {DateTime.Now.ToLongTimeString()}");

            await Task.CompletedTask;

        }
    }
}

using Hangfire.Common;
using System;
using System.Linq;
using Newtonsoft.Json;
using Hangfire.Client;
using Hangfire.Server;

namespace HetsData.Hangfire
{
    public sealed class SkipSameJobAttribute : JobFilterAttribute, IClientFilter, IServerFilter
    {
        private readonly int _timeoutInSeconds = 1;

        public void OnCreated(CreatedContext filterContext)
        {
        }

        public void OnCreating(CreatingContext context)
        {
            var job = context.Job;
            var jobFingerprint = GetJobFingerprint(job);

            var monitor = context.Storage.GetMonitoringApi();
            var fingerprints = monitor.ProcessingJobs(0, 10000)
                .Select(x => GetJobFingerprint(x.Value.Job))
                .ToList();

            fingerprints.AddRange(
                monitor.EnqueuedJobs("default", 0, 10000)
                .Select(x => GetJobFingerprint(x.Value.Job))
            );

            foreach (var fingerprint in fingerprints)
            {
                if (jobFingerprint != fingerprint)
                    continue;

                context.Canceled = true;

                return;
            }
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var resource = GetJobFingerprint(filterContext.BackgroundJob.Job);

            var timeout = TimeSpan.FromSeconds(_timeoutInSeconds);

            var distributedLock = filterContext.Connection.AcquireDistributedLock(resource, timeout);
            filterContext.Items["DistributedLock"] = distributedLock;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            if (!filterContext.Items.ContainsKey("DistributedLock"))
            {
                throw new InvalidOperationException("Can not release a distributed lock: it was not acquired.");
            }

            var distributedLock = (IDisposable)filterContext.Items["DistributedLock"];
            distributedLock.Dispose();
        }

        private string GetJobFingerprint(Job job)
        {
            var args = "";

            if (job.Args.Count > 0)
            {
                args = "-" + JsonConvert.SerializeObject(job.Args);
            }

            return $"{job.Type.FullName}-{job.Method.Name}{args}";
        }
    }
}

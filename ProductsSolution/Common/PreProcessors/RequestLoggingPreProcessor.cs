using FastEndpoints;

namespace ProductsSolution.Common.PreProcessors
{
    public class RequestLoggingPreProcessor<TRequest> : IPreProcessor<TRequest>
    {
        public Task PreProcessAsync(IPreProcessorContext<TRequest> ctx, CancellationToken ct)
        {
            var logger = ctx.HttpContext.Resolve<ILogger<TRequest>>(); //.Resolve<T>() is a FastEndpoints helper method -> Get service from DI container.... "Give me an ILogger<TRequest> that is already registered in DI"k
            var request = ctx.HttpContext.Request;

            // Store start time
            ctx.HttpContext.Items["RequestStartTime"] = DateTime.UtcNow;

            var queryString = request.QueryString.HasValue ? request.QueryString.Value : "";

            logger.LogInformation(
                "THIS IS A LOG DATA FROM PRE PROCESSOR: [{Timestamp}] {Method} {Path}{Query}",
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                request.Method,
                request.Path,
                queryString);

            return Task.CompletedTask;
        }
    }
}

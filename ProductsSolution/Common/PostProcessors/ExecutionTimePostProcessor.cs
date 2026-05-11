using FastEndpoints;
using Microsoft.Extensions.Logging;

public class ExecutionTimePostProcessor<TRequest, TResponse>
    : IPostProcessor<TRequest, TResponse>
    where TRequest : notnull
{
    public Task PostProcessAsync(IPostProcessorContext<TRequest, TResponse> ctx, CancellationToken ct)
    {
        var http = ctx.HttpContext;
        var logger = http.Resolve<ILogger<ExecutionTimePostProcessor<TRequest, TResponse>>>();
        var req = http.Request;

        var now = DateTime.UtcNow;
        logger.LogInformation("THIS IS FROM POST PROCESSOR: ");
        if (http.Items.TryGetValue("RequestStartTime", out var startObj) && startObj is DateTime start) //If 'RequestStartTime' exists AND it is a DateTime, then store it in variable 'start'
        {
            var elapsedMs = (now - start).TotalMilliseconds;
            
            if (elapsedMs > 1000)
            {
                logger.LogWarning(
                    "SLOW [{Timestamp}] {Method} {Path} -> {StatusCode} ({ElapsedMs} ms)",
                    now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    req.Method,
                    req.Path,
                    http.Response.StatusCode,
                    elapsedMs.ToString("F2"));
            }
            else
            {
                logger.LogInformation(
                    "[{Timestamp}] {Method} {Path} -> {StatusCode} ({ElapsedMs} ms)",
                    now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    req.Method,
                    req.Path,
                    http.Response.StatusCode,
                    elapsedMs.ToString("F2"));
            }
        }
        else
        {
            // Fallback if PreProcessor didn't set start time
            logger.LogWarning(
                "[{Timestamp}] {Method} {Path} -> {StatusCode}",
                now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                req.Method,
                req.Path,
                http.Response.StatusCode);
        }

        return Task.CompletedTask;
    }
}
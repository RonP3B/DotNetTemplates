using Evently.Gateway.Authentication;
using Evently.Gateway.Middleware;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("Evently.Gateway"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("Yarp.ReverseProxy");

        tracing.AddOtlpExporter();
    });

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.ConfigureOptions<JwtBearerConfigureOptions>();
// Add services to the container.

var app = builder.Build();

app.UseLogContextTraceLogging();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();
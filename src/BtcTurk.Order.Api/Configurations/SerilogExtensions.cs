using System.Diagnostics;
using Serilog;
using Serilog.Exceptions;

namespace BtcTurk.Order.Api.Configurations;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilog(
        this WebApplicationBuilder builder,
        string sectionName = "Serilog")
    {
        var serilogOptions = new SerilogOptions();
        builder.Configuration.GetSection(sectionName).Bind(serilogOptions);

        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration, sectionName: sectionName);

            loggerConfiguration
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
                .Enrich.WithProperty("TraceId", Activity.Current?.TraceId.ToString())
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails();
            
            if (serilogOptions.UseConsole)
            {
                loggerConfiguration.WriteTo.Async(writeTo =>
                    writeTo.Console(outputTemplate: serilogOptions.LogTemplate));
            }
            
            if (!string.IsNullOrEmpty(serilogOptions.SeqUrl))
            {
                loggerConfiguration.WriteTo.Seq(serilogOptions.SeqUrl);
            }

        });

        return builder;
    }
    
    private sealed class SerilogOptions
    {
        public bool UseConsole { get; set; } = true;
        public string? SeqUrl { get; set; } 

        public string LogTemplate { get; set; } =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level} - {Message:lj}{NewLine}{Exception}";
    }
}

using App.WindowsService;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

Trace.AutoFlush = true;
using (var textWriterTraceListener = new TextWriterTraceListener(@"C:\logs\trace-logs.log"))
{
    IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET Joke Service";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<WindowsBackgroundService>();
        services.AddHttpClient<JokeService>();
        services.AddLogging(builder =>
        {
            builder.AddTraceSource(new SourceSwitch("HouseKeeping") { Level = SourceLevels.All }, textWriterTraceListener);
        });
    })
    .Build();
    await host.RunAsync();

    var ts = new TraceSource("HouseKeeping", SourceLevels.All);
    ts.Listeners.Add(textWriterTraceListener);
}


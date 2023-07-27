using dialogs_api;

var builder =Host.CreateDefaultBuilder(args).ConfigureLogging(x =>
{
    x.ClearProviders();
    x.AddConsole();
}).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
    webBuilder.UseKestrel();
    webBuilder.UseUrls("http://*:7289");
});

builder.Build().Run();
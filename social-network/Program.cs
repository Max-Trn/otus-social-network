using social_network;

var builder =Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
    webBuilder.UseKestrel();
    webBuilder.UseUrls("http://*:7285");
});

builder.Build().Run();



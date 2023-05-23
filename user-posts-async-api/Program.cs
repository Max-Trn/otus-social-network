using user_posts_async_api;

var builder =Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
    webBuilder.UseKestrel();
    webBuilder.UseUrls("http://*:7286");
});

builder.Build().Run();


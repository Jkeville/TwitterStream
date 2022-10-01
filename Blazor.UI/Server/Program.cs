using Blazor.UI.Server.Extensions;

using Blazor.UI.Server.Hubs;
using Blazor.UI.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json.Converters;
using TwitterStream.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRedisDB(builder.Configuration);
builder.Services.ConfigureBrokerServices();
builder.Services.ConfigureTwitterApiServices(builder.Configuration.GetSection("Api").GetSection("TwitterApi"));
builder.Services.ConfigureTwitterQueues(builder.Configuration.GetSection("Queues"));
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddCors(policy =>
    {
        policy.AddPolicy("CorsPolicy", opt => opt
            .WithOrigins("https://localhost:5001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
    });

builder.Services.AddSingleton<TimerManager>();
builder.Services.AddSingleton<IDataManager,DataManager>();
builder.Services.AddSignalR();//.AddJsonProtocol(options =>
//{
//    options.PayloadSerializerOptions.Converters.Add()
//});
    



var app = builder.Build();
app.UseResponseCompression();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<TwitterHub>("/twitterhub");
});


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();



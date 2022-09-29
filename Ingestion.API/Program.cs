
using Microsoft.AspNetCore.HttpOverrides;
using Ingestion.API.Extensions;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

//logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

// Add services to the container.
builder.Services.ConfigureSwagger();
builder.Services.ConfigureRedisDB(builder.Configuration);
builder.Services.ConfigureBrokerServices();
builder.Services.ConfigureTwitterApiServices(builder.Configuration.GetSection("Api").GetSection("TwitterApi"));
builder.Services.ConfigureTwitterQueues(builder.Configuration.GetSection("Queues"));
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.AddControllers();
builder.Host.UseSerilog();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Twitter Service API v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Twitter Service API v2");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();



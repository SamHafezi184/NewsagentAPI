
using AssessmentAPI.Contracts;
using AssessmentAPI.Middleware;
using AssessmentAPI.Services;
using Serilog;
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "log", "requests-responses.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                fileSizeLimitBytes: 10_000_000)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "NewsagentAPI")
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient();


        builder.Services.AddScoped<INewsagentDatasetService, NewsagentDatasetService>();
        builder.Services.AddScoped<INewsagentMatcher, NewsagentMatcher>();
        builder.Services.AddScoped<IChainValidator, SuperNewsValidator>();
        builder.Services.AddScoped<IChainValidator, AdventureNewsValidator>();
        builder.Services.AddScoped<IChainValidator, NewsInWordsValidator>();
        builder.Services.AddScoped<ChainValidatorFactory>();




        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseMiddleware<RequestResponseLoggingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}


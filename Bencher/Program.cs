// See https://aka.ms/new-console-template for more information

using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;

BenchmarkRunner.Run<LoggerPerformance>();

[MemoryDiagnoser(displayGenColumns: true)]
public class LoggerPerformance
{
    private readonly ILogger<LoggerPerformance> _logger = 
        new LoggerFactory().CreateLogger<LoggerPerformance>();
    
    [Benchmark]
    public void LogInformationSimple()
    {
        _logger.LogInformation("Information for MyString");
    }

    [Benchmark]
    public void LogInformationInterpolation()
    {
        var str = "MyString";
        var preped = $"{str}{str.Reverse()}";
        _logger.LogInformation($"Information for {str}");
    }
    
    [Benchmark]
    public void LogInformationStructured()
    {
        var str = "MyString";
        _logger.LogInformation("Information for {str}", str);
    }
    
    [Benchmark]
    public void LogInformationCustomSimple()
    {
        _logger.LogInformationCustom(str: "MyString");
    }

    [Benchmark]
    public void LogInformationCustomInterpolation()
    {
        var str = "MyString";
        var preped = $"{str}{str.Reverse()}";
        _logger.LogInformationCustom(preped);
    }
    
    [Benchmark]
    public void LogInformationCustomStructuredLogDataVariabled()
    {
        var ld = new LogData("MyString");
        _logger.LogInformationLogData(ld);
    }
    
    [Benchmark]
    public void LogInformationCustomStructuredLogDataInlined()
    {
        _logger.LogInformationLogData(new LogData(null));
    }
    
    [Benchmark]
    public void LogInformationCustomStructuredTopFunction()
    {
        _logger.LogInformationCustomLogData(new LogData("MyString"));
    }
}

public static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Information for {str}")]
    public static partial void LogInformationCustom(this ILogger logger, string str = "MyString");

    public static void LogInformationCustomLogData(this ILogger logger, LogData logData)
    {
        logger.LogInformationLogData(logData);
    }
    
    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Information for MyString")]
    public static partial void LogInformationLogData(this ILogger logger,[LogProperties(SkipNullProperties = true,OmitReferenceName = true)] LogData logData);
}

public record LogData(string? Str, string Type = "", string Bank = "Capitec");
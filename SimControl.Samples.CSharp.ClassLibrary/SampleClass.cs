using Microsoft.Extensions.Logging;

namespace SimControl.Samples.CSharp.ClassLibrary
{
    public class SampleClass
    {
        public SampleClass(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SampleClass>();

            _logger.LogInformation("SampleClass instantiated");
        }

        private readonly ILogger _logger;
    }
}

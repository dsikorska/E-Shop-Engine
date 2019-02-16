using NLog;
using NLog.Config;
using NLog.Targets;

namespace E_Shop_Engine.Services
{
    public static class NLogConfig
    {
        /// <summary>
        /// Set configuration for logging service.
        /// </summary>
        public static void RegisterConfig()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            FileTarget target = new FileTarget("target2")
            {
                FileName = "${basedir}/App_Data/logs/app.log",
                Layout = "${longdate} ${level:uppercase=true} ${logger} ${callsite} ${newline} ${aspnet-request-form:OutputFormat=JSON} ${newline} " +
                "${aspnet-request:header=Referer} ${newline} ${message} ${newline} ${exception:format=toString,Data} ${newline}",
                ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveAboveSize = 10 * 1024 * 1024
            };
            config.AddTarget(target);

            config.AddRuleForAllLevels(target);

            LogManager.Configuration = config;
        }
    }
}

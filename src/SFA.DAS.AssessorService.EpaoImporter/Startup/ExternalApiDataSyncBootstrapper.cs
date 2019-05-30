﻿using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.AssessorService.EpaoImporter.Const;
using SFA.DAS.AssessorService.EpaoImporter.InfrastructureServices;
using SFA.DAS.AssessorService.EpaoImporter.Logger;
using SFA.DAS.AssessorService.EpaoImporter.Startup.DependencyResolution;
using SFA.DAS.AssessorService.Settings;
using StructureMap;
using System.Globalization;

namespace SFA.DAS.AssessorService.EpaoImporter.Startup
{
    public class ExternalApiDataSyncBootstrapper
    {
        private readonly Container _container;

        public ExternalApiDataSyncBootstrapper(TraceWriter functionLogger, ExecutionContext context)
        {
            IAggregateLogger logger = new AggregateLogger(FunctionName.ExternalApiDataSync, functionLogger, context);

            var configuration = ConfigurationHelper.GetConfiguration();

            logger.LogInfo("Initialising bootstrapper ....");
            logger.LogInfo("Config Received");

            _container = new Container(configure =>
            {
                configure.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.WithDefaultConventions();
                });

                configure.For<IAggregateLogger>().Use(logger).Singleton();
                configure.For<IWebConfiguration>().Use(configuration).Singleton();

                logger.LogInfo("Calling http registry and getting the token ....");
                configure.AddRegistry<HttpRegistry>();
            });

            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }        
    }
}

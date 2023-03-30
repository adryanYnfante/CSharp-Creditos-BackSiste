using AutoMapper.Data;
using Creditos.AppServices.Messaging.Automapper;
using Domain.UseCase.Common;
using DrivenAdapters.Mongo;
using EntryPoints.ServiceBus;
using EntryPoints.ServiceBus.Dtos;
using org.reactivecommons.api;
using org.reactivecommons.api.impl;
using SC.Configuration.Provider.Mongo;

namespace Creditos.AppServices.Messaging.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Método para registrar AutoMapper
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(cfg =>
            {
                cfg.AddDataReaderMapping();
            }, typeof(ConfigurationProfile));

        /// <summary>
        /// Método para registrar Mongo
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="connectionString">connection string.</param>
        /// <param name="db">database.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterMongo(this IServiceCollection services, string connectionString, string db) =>
                                    services.AddSingleton<IContext>(provider => new Context(connectionString, db));

        /// <summary>
        /// Método para registrar los servicios
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            #region UseCases

            services.AddTransient<IManageEventsUseCase, ManageEventsUseCase>();

            #endregion UseCases

            return services;
        }

        /// <summary>
        /// RegisterSubscriptions
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection RegisterSubscriptions(this IServiceCollection services)
        {
            services.AddTransient<ICreditoCommandSubscription, CreditoCommandSubscription>();
            services.AddTransient<ICreditoEventSubscription, CreditoEventSubscription>();

            return services;
        }

        /// <summary>
        /// RegisterAsyncGateways
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceBusConn"></param>
        public static IServiceCollection RegisterAsyncGateways(this IServiceCollection services,
                string serviceBusConn)
        {
            services.RegisterAsyncGateway<CreditoRequest>(serviceBusConn);
            return services;
        }

        /// <summary>
        /// Register Gateway
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceBusConn"></param>
        private static void RegisterAsyncGateway<TEntity>(this IServiceCollection services, string serviceBusConn) =>
                services.AddSingleton<IDirectAsyncGateway<TEntity>>(new DirectAsyncGatewayServiceBus<TEntity>(serviceBusConn));

        public static IConfigurationBuilder AddMongoProvider(this ConfigurationManager configuration,
            string sectionName, string connectionString, string suffix)
        {
            MongoAppsettingsConfiguration settings = new MongoAppsettingsConfiguration();
            configuration.GetSection(sectionName).Bind(settings);
            settings.ConnectionString = connectionString;
            configuration.AddMongoConfiguration(options =>
            {
                options.ConnectionString = settings.ConnectionString;
                options.CollectionName = settings.CollectionName;
                options.DatabaseName = $"{settings.DatabaseName}_{suffix}";
                options.ReloadOnChange = settings.ReloadOnChange;
            });

            return configuration;
        }

        public static IServiceCollection RegisterCors(this IServiceCollection services, string policyName) =>
            services.AddCors(o => o.AddPolicy(policyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

        public static IConfigurationBuilder AddJsonProvider(this IConfigurationBuilder configuration)
                => configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
    }
}
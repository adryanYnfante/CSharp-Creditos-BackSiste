using AutoMapper.Data;
using credinet.comun.api;
using Creditos.AppServices.Automapper;
using Domain.Model.Entities.Gateway;
using Domain.UseCase.Clientes;
using Domain.UseCase.Common;
using Domain.UseCase.Creditos;
using DrivenAdapters.Mongo;
using DrivenAdapters.ServiceBus;
using DrivenAdapters.ServiceBus.Entities;
using EntryPoints.ReactiveWeb.Dtos.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using org.reactivecommons.api;
using org.reactivecommons.api.impl;
using StackExchange.Redis;
using System;

namespace Creditos.AppServices.Extensions
{
    /// <summary>
    /// Service Extensions
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers the cors.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterCors(this IServiceCollection services, string policyName) =>
            services.AddCors(o => o.AddPolicy(policyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

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
            #region Helpers

            services.AddSingleton<IMensajesHelper, MensajesApiHelper>();

            #endregion Helpers

            #region UseCases

            services.AddScoped<IManageEventsUseCase, ManageEventsUseCase>();
            services.AddScoped<IClienteUseCase, ClienteUseCase>();
            services.AddScoped<ICreditoUseCase, CreditoUseCase>();

            #endregion UseCases

            #region Repositories

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICreditoRepository, CreditoRepository>();

            #endregion Repositories

            services.AddGrpc();

            return services;
        }

        private static void RegisterAsyncGateway<TEntity>(this IServiceCollection services, string serviceBusConn) =>
            services.AddSingleton<IDirectAsyncGateway<TEntity>>(new DirectAsyncGatewayServiceBus<TEntity>(serviceBusConn));

        public static IServiceCollection RegisterAsyncGateways(this IServiceCollection services,
            string serviceBusConn)
        {
            services.RegisterAsyncGateway<PagoEntity>(serviceBusConn);
            return services;
        }

        public static IServiceCollection RegisterFluentValidations(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CrearClienteValidation>();
            services.AddValidatorsFromAssemblyContaining<ModificarClienteValidation>();
            services.AddValidatorsFromAssemblyContaining<CrearCreditoValidation>();
            return services;
        }

        /// <summary>
        ///   Lazies the connection.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        /// <returns></returns>
        private static Lazy<ConnectionMultiplexer> LazyConnection(string connectionString) =>
            new(() => ConnectionMultiplexer.Connect(connectionString));
    }
}
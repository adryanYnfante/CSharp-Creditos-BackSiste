using Helpers.ObjectsUtils.ApplicationSettings;

namespace Helpers.ObjectsUtils
{
    /// <summary>
    /// ConfiguradorAppSettings
    /// </summary>
    public class ConfiguradorAppSettings
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Nombre del dominio
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// MongoDBConnection
        /// </summary>
        public string MongoDBConnection { get; set; }

        /// <summary>
        /// Pais por defecto
        /// </summary>
        public string DefaultCountry { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// StorageConnection
        /// </summary>
        public string StorageConnection { get; set; }

        /// <summary>
        /// StorageContainerName
        /// </summary>
        public string StorageContainerName { get; set; }

        /// <summary>
        /// RedisCacheConnectionString
        /// </summary>
        public string RedisCacheConnectionString { get; set; }

        /// <summary>
        /// EndPoint de HealthChecks
        /// </summary>
        public string HealthChecksEndPoint { get; set; }

        public decimal TASA_EA { get; set; }

        public string QueueName { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string TopicNameSubscription { get; set; }

        /// <summary>
        /// Validation
        /// </summary>
        public ValidationSettings Validation { get; set; }

        /// <summary>
        /// Gets or sets the instancias redis.
        /// </summary>
        /// <value>
        /// The instancias redis.
        /// </value>
        public SettingInstanciaRedis InstanciasRedis { get; set; }
    }
}
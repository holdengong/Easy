namespace Identity.API
{
    public class IdentityApiConfig
    {
        /// <summary>
        /// sso站点host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// mysql连接字符串
        /// </summary>
        public string MySqlConnectionString { get; internal set; }

        /// <summary>
        /// redis连接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }
    }
}

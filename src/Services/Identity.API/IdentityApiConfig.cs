namespace Identity.API
{
    public class IdentityApiConfig
    {
        /// <summary>
        /// sso站点host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// mysql链接字符串
        /// </summary>
        public DatabaseConnectionStrings MySql { get; set; }
    }

    public class DatabaseConnectionStrings
    {
        public string IdentityDbContextConnectionString { get; set; }
        public string ConfigurationDbContextConnectionString { get; set; }
        public string PersistedGrantDbContextConnectionString { get; set; }
    }
}

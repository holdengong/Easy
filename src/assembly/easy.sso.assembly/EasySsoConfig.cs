namespace EasySso
{
    public class EasySsoConfig
    {
        /// <summary>
        /// 使用自定义登录页
        /// </summary>
        public bool UseCustomLoginPage { get; set; } = false;

        /// <summary>
        /// 自定义登录页地址
        /// </summary>
        public string CustomLoginPath { get; set; }
    }
}

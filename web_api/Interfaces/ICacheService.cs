namespace web_api.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);

        /// <summary>
        /// Deve ser passado o tempo de expiração em segundos
        /// </summary>
        /// <typeparam name="T">tipo do objeto do cache</typeparam>
        /// <param name="key">chave do cache</param>
        /// <param name="value">objeto do cache</param>
        /// <param name="cacheMinutes">tempo em segundos</param>
        void Set<T>(string key, T value, int cacheSeconds);
        //void Set<T>(string key, T value);
        ///// <summary>
        ///// Adicionar no cache com tempo de experição em 900 sgundos
        ///// </summary>
        ///// <typeparam name="T">tipo do objeto do cache</typeparam>
        ///// <param name="key">chave do cache</param>
        ///// <param name="value">objeto do cache</param>
        void Remove(string key);
    }
}

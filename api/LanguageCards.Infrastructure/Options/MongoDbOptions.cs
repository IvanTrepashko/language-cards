namespace MongoDb.Options
{
    public class MongoDbOptions
    {
        public string DatabaseName { get; set; }

        public string Port { get; set; }

        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                var auth = (Username != null && Password != null) ?
                    $"{Username}:{Password}@" :
                    string.Empty;

                return $"mongodb://{auth}{Url}:{Port}";
            }
        }
    }
}

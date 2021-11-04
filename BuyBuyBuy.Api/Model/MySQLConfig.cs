namespace BuyBuyBuy.Api.Model
{
    public class MySQLConfig
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public string ConnectionString =>
            $"Data Source={Address};Port={Port};User ID={User};Password={Password};Initial Catalog={Database};" +
            $"Charset=utf8;SslMode=none;Max pool size=10";
    }
}

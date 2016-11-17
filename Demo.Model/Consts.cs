namespace Demo.Model
{
    public static class Consts
    {
        public const string RabbitMqAddress = "rabbitmq://localhost:5672";
        public const string SagaQueue = "load.data";
        public const string User = "guest";
        public const string Pass = "guest";
        public const string Source = @"http://api.github.com/orgs/gopangea/repos";
    }
}
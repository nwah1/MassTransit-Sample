namespace Demo.Model
{
    public static class Consts
    {
        public const string RabbitMqAddress = "rabbitmq://machost:5672";
        public const string LoadDataQueue = "load.data";
        public const string ListReposQueue = "list.repos";
        public const string User = "worker";
        public const string Pass = "worker";
        public const string Source = @"http://api.github.com/orgs/gopangea/repos";
    }
}
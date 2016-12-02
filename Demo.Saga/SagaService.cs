using System;
using Automatonymous;
using Demo.Model;
using MassTransit;
using MassTransit.EntityFrameworkIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Saga;
using Topshelf;
using Topshelf.Logging;
using MassTransit.Util;

namespace Demo.Saga
{
    class SagaService : ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<SagaService>();

        IBusControl _busControl;
        LoadDataSaga _machine;
        ISagaRepository<LoadData> _repository;

        public SagaService()
        {
        }

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");

            _machine = new LoadDataSaga();

            SagaDbContextFactory sagaDbContextFactory = () => new SagaDbContext<LoadData, LoadDataMap>(DbContextFactoryProvider.ConnectionString);

            //_repository = new Lazy<ISagaRepository<LoadData>>(() => new EntityFrameworkSagaRepository<LoadData>(sagaDbContextFactory));
            _repository = new InMemorySagaRepository<LoadData>();

            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                IRabbitMqHost host = x.Host(new Uri(Consts.RabbitMqAddress), h =>
                {
                    h.Username(Consts.User);
                    h.Password(Consts.Pass);
                });

                x.ReceiveEndpoint(host, Consts.LoadDataQueue, e =>
                {
                    e.PrefetchCount = 8;
                    e.StateMachineSaga(_machine, _repository);
                });
                
            });

            _log.Info("Starting bus...");
            
            TaskUtil.Await(() => _busControl.StartAsync());

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _log.Info("Stopping bus...");

            if (_busControl != null)
                _busControl.Stop();

            return true;
        }
    }
}
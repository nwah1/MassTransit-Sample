using System;
using Automatonymous;
using Demo.Model;

namespace Demo.Saga
{
    public class LoadDataSaga : MassTransitStateMachine<LoadData>
    {
        public LoadDataSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => DataLoaded, x => x.CorrelateBy(data => data.Id, context => context.Message.Id).SelectId(context => Guid.NewGuid()));
            
            Initially(
                When(DataLoaded)
                    .Then(context =>
                    {
                        context.Instance.TimeOfRequest = DateTime.Now;
                        context.Publish(new SaveData { Repos = context.Data.Repos });
                    })
                    .ThenAsync(context => Console.Out.WriteLineAsync($"Data Loading..."))
                    .Finalize());

            SetCompletedWhenFinalized();
        }

        public State Active { get; private set; }

        public Event<ILoadData> DataLoaded { get; private set; }
    }
}
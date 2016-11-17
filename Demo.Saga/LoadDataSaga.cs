using System;
using System.Collections.Generic;
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
                        context.Instance.Id = Guid.NewGuid().ToString();
                        context.Instance.TimeOfRequest = DateTime.Now;
                    })
                    .ThenAsync(context => Console.Out.WriteLineAsync($"Data Loading..."))
                    .Publish(context => new SaveData { Repos = context.Data.Repos })
                    .Finalize());

            SetCompletedWhenFinalized();
        }

        public State Active { get; private set; }

        public Event<ILoadData> DataLoaded { get; private set; }
    }
}
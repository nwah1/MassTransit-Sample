#region Imports

using Automatonymous;
using System;

#endregion Imports

public class LoadData : SagaStateMachineInstance
{
    #region Properties of LoadData

    public Guid CorrelationId { get; set; }

    public State CurrentState { get; set; }

    public string Id { get; set; }

    public DateTime TimeOfRequest { get; set; }

    #endregion Properties of LoadData
}

#region Imports

using System;
using System.Collections.Generic;

#endregion Imports

namespace Demo.Model
{
    public interface ILoadData
    {
        Guid CorrelationId { get; }
        string Id { get; set; }
        List<Repo> Repos { get; set; }
        DateTime TimeOfRequest { get; set; }
    }
}

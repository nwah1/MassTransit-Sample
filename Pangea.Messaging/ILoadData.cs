using System;
using System.Collections.Generic;

namespace Pangea.Messaging
{
    public interface ILoadData
    {
        Guid Id { get; }
        List<Repo> Repos { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Demo.Model
{
    public interface ILoadData
    {
        Guid Id { get; }
        List<Repo> Repos { get; set; }
    }
}
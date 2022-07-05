using System;
using System.Collections.Generic;
using ParticipantsViewer.Commands;

namespace ParticipantsViewer.Repository
{
    public interface ICommandsRepository
    {
        Dictionary<String, Func<IParticipantsRepository, String, ICommand>> Commands { get; }
    }
}

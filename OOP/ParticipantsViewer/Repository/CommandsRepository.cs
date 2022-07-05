using System;
using System.Collections.Generic;
using ParticipantsViewer.Commands;

namespace ParticipantsViewer.Repository
{
    public class CommandsRepository : ICommandsRepository
    {
        public Dictionary<string, Func<IParticipantsRepository, String, ICommand>> Commands => new()
            {
                {"get-page", (participantRepos, argument) => new GetPageCommand(participantRepos, argument) },
                { "search", (participantRepos, argument) => new SearchCommand(participantRepos, argument) }
        };
    }
}

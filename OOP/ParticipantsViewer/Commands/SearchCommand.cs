using ParticipantsViewer.Extensions;
using ParticipantsViewer.Model;
using ParticipantsViewer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParticipantsViewer.Commands
{
    public class SearchCommand : ICommand
    {
        private readonly IParticipantsRepository _repository;
        private readonly string _argument;

        public SearchCommand(IParticipantsRepository repos, string arg)
        {
            _repository = repos;
            _argument = arg[1..^1];
        }

        public IEnumerable<Participant> Result { get; set; }

        public void Execute()
        {
            Result = _repository.Participants
                .OrderBy(p => p.RegistrationDate)
                .Distinct(new ParticipantComparer())
                .Search(_argument);
            Print();
        }

        private void Print()
        {
            Console.WriteLine("{0, 15} | {1, 15} | {2, 17} | {3, 10}", "Имя", "Фамилия", "Дата регистрации", "Сервис");
            Console.WriteLine("___________________________________________________________________");
            foreach (var participant in Result)
            {
                Console.WriteLine("{0, 15} | {1, 15} | {2, 17} | {3, 10}",
                    participant.FirstName,
                    participant.LastName,
                    participant.RegistrationDate.ToString("g"),
                    participant.ProviderName);
            }
        }
    }
}
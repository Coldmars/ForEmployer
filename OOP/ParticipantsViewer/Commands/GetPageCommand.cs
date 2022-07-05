using System;
using System.Collections.Generic;
using System.Linq;
using ParticipantsViewer.Model;
using ParticipantsViewer.Repository;
using ParticipantsViewer.Extensions;

namespace ParticipantsViewer.Commands
{
   public class GetPageCommand : ICommand
    {
        private readonly IParticipantsRepository _repository;
        private readonly string _argument;
        private const int PAGE_SIZE = 5;

        public GetPageCommand(IParticipantsRepository repos, string arg)
        {
            _repository = repos;
            _argument = arg;
        }

        public IEnumerable<Participant> Repository { get; set; }

        public void Execute()
        {
            Repository = _repository.Participants
                .OrderBy(p => p.RegistrationDate)
                .Distinct(new ParticipantComparer())
                .Page(Int32.Parse(_argument), PAGE_SIZE);
            Print();
        }

        private void Print()
        {
            Console.WriteLine("{0, 15} | {1, 15} | {2, 17} | {3, 10}", "Имя", "Фамилия", "Дата регистрации", "Сервис");
            Console.WriteLine("___________________________________________________________________");
            foreach (var participant in Repository)
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

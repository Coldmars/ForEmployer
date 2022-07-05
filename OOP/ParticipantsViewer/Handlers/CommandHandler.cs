using System;
using ParticipantsViewer.Commands;
using ParticipantsViewer.Repository;

namespace ParticipantsViewer.Handlers
{
    public class CommandHandler
    {
        private string _name;
        private string _argument;
        private readonly IParticipantsRepository _participantRepository;
        private readonly ICommandsRepository _commandRepository;
        private ICommand _command;

        public CommandHandler(ICommandsRepository commands, IParticipantsRepository participants)
        {
            _commandRepository = commands;
            _participantRepository = participants;
        }

        public void Run(string enterCommand)
        {
            ParseCommand(enterCommand);
            SetCommand();
            _command.Execute();
        }

        private void ParseCommand(string command)
        {
            try
            {
                var substrings = command.Split(" ");
                _name = substrings[0];
                _argument = substrings[1];
            }
            catch
            {
                throw new Exception("Failed to parse. \n Command's template: {Instuction} {Argument}");
            }
        }

        private ICommand RecognizeCommand() =>
            _commandRepository.Commands.ContainsKey(_name)
                ? _commandRepository.Commands[_name](_participantRepository, _argument)
                : throw new Exception("Instruction does not exist");

        private void SetCommand() =>
            _command = RecognizeCommand();
    }
}

using System;
using ParticipantsViewer.Import;
using ParticipantsViewer.Formatters;
using ParticipantsViewer.Repository;
using ParticipantsViewer.Handlers;
using System.Threading.Tasks;

namespace ParticipantsViewer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ParticipantsRepository participantsRepository = new();
            CommandsRepository commandsRepository = new();

            var jsonDeserialize = ImportJsonToRepositoryAsync(@"./participants.json", participantsRepository);
            var xmlDeserialize = ImportXmlToRepositoryAsync(@"./participants.xml", participantsRepository);
            var csvDeserialize = ImportCsvToRepositoryAsync(@"./participants.csv", participantsRepository);

            CommandHandler commandHandler = new(commandsRepository, participantsRepository);

            while (true)
            {
                string command = Console.ReadLine();
                await jsonDeserialize;
                await xmlDeserialize;
                await csvDeserialize;
                commandHandler.Run(command);
            }
        }

        private static async Task ImportJsonToRepositoryAsync(string pathToFile, IParticipantsRepository repos)
        {
            JsonFormatter json = new();
            Importer importer = new();

            await Task.Run(() => importer.ImportDataToRepository(repos.Participants, json, pathToFile));
        }
        private static async Task ImportXmlToRepositoryAsync(string pathToFile, IParticipantsRepository repos)
        {
            XmlFormatter xml = new();
            Importer importer = new();

            await Task.Run(() => importer.ImportDataToRepository(repos.Participants, xml, pathToFile));
        }
        private static async Task ImportCsvToRepositoryAsync(string pathToFile, IParticipantsRepository repos)
        {
            CsvFormatter csv = new();
            Importer importer = new();

            await Task.Run(() => importer.ImportDataToRepository(repos.Participants, csv, pathToFile));
        }
    }
}

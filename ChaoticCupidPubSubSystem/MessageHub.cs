using ChaoticCupidPubSubSystem.Interfaces;
using ChaoticCupidPubSubSystem.Model;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography;

namespace ChaoticCupidPubSubSystem
{
    public class MessageHub : Hub, ICupidHub, IPersonHub
    {
        private static readonly List<Person> _persons = new();

        public Task InitSinglePerson(string username, string city, int age, string phoneNumber)
        {
            Person person = new Person(username, city, age, phoneNumber);
            person.ConnectionId = Context.ConnectionId;

            if (!_persons.Any(x => x.Username == person.Username))
            {
                _persons.Add(person);
                Console.WriteLine($"[SERVER] Korisnik {person.Username} je sabskrajbovan!");
            }
            else
            {
                Console.WriteLine($"[SERVER] Korisnik {person.Username} vec postoji!");
            }

            return Task.CompletedTask;
        }

        public bool IsSimilarAge(int age1, int age2)
        {
            return Math.Abs(age1 - age2) <= 2;
        }

        public Task BlockPerson(string username, string personToBlock)
        {
            var person = _persons.FirstOrDefault(p => p.Username == username);

            if (person != null)
            {
                person.BlockedPersons.Add(personToBlock);
                Console.WriteLine($"[SERVER] {username} blokirao {personToBlock}");
            }

            return Task.CompletedTask;
        }

        public Task ConfirmReceived(string username)
        {
            var person = _persons.FirstOrDefault(p => p.Username == username);

            if (person != null)
            {
                person.ConfirmLetter = true;
                Console.WriteLine($"[SERVER] {username} potvrdio/la prijem pisma.");
            }

            return Task.CompletedTask;
        }

        public async Task SendLetter()
        {
            for (int i = 0; i < _persons.Count; i++)
            {
                var receiver = _persons[i];

                if (!receiver.ConfirmLetter)
                {
                    Console.WriteLine($"[SERVER] {receiver.Username} jos nije potvrdio/la prethodno pismo.");
                    continue;
                }

                int matchingScore = -1;
                Person? sender = null;

                for (int j = 0; j < _persons.Count; j++)
                {
                    var possibleSender = _persons[j];

                    if (receiver.Username == possibleSender.Username)
                        continue;

                    if (receiver.BlockedPersons.Contains(possibleSender.Username))
                        continue;

                    int senderScore = 0;

                    if (possibleSender.City == receiver.City)
                        senderScore += 30;

                    if (IsSimilarAge(possibleSender.Age, receiver.Age))
                        senderScore += 20;

                    senderScore += RandomNumberGenerator.GetInt32(0, 101);

                    if (senderScore > matchingScore)
                    {
                        matchingScore = senderScore;
                        sender = possibleSender;
                    }
                }

                if (sender == null)
                {
                    Console.WriteLine($"[SERVER] Posiljalac za {receiver.Username} ne postoji.");
                    continue;
                }

                string[] messages =
                {
                    "Radujem se nasem susretu!",
                    "Zelim da se upoznamo.",
                    "Nisam zainteresovan/a za upoznavanje"
                };

                int randomIndex = RandomNumberGenerator.GetInt32(0, messages.Length);
                string selectedMessage = messages[randomIndex];
                string message;
                if (selectedMessage == "Nisam zainteresovan/a za upoznavanje")
                
                    message = $"Posiljalac: {sender.Username}, {sender.Age}, {sender.City}\n" +
                              $"Pismo: {selectedMessage}";
                
                else
                    message = $"Posiljalac: {sender.Username}, {sender.Age}, {sender.City}, {sender.PhoneNumber}\n" +
                              $"Pismo: {selectedMessage}";


                Console.WriteLine($"[SERVER] Pismo poslato {receiver.Username} od {sender.Username}");

                //notify sub that the letter has arrived
                await Clients.Client(receiver.ConnectionId)
                    .SendAsync("LetterArrived", message);

                receiver.ConfirmLetter = false;
            }
        }
    }
}
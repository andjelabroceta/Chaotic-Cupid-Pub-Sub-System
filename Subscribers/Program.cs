using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;


namespace Subscriber
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string username, phoneNumber, city, age;
            int ageInt;
            while (true)
            {
                Console.WriteLine("[SUB] Unesi korisnicko ime : ");
                username = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("[SUB] Korisnicko ime mora biti uneseno!");
                    continue;
                }
                break;
            }
            while (true)
            {
                Console.WriteLine("[SUB] Unesi grad : ");
                city = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(city))
                {
                    Console.WriteLine("[SUB] Grad mora biti unesen!");
                    continue;
                }
                break;
            }

            while (true)
            {
                Console.WriteLine("[SUB] Unesi godine : ");
                age = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(age))
                {
                    Console.WriteLine("[SUB] Godine moraju biti unesene!");
                    continue;
                }
                if (!int.TryParse(age, out ageInt))
                {
                    Console.WriteLine("[SUB] Godine moraju biti broj!");
                    continue;
                }
                if(ageInt <= 0)
                {
                    Console.WriteLine("[SUB] Godine moraju biti >0 !");
                    continue;
                }
                break;
            
            }   
            while (true)
            {
                Console.WriteLine("[SUB] Unesi broj telefona : ");
                phoneNumber = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    Console.WriteLine("[SUB] Broj telefona mora biti unesen!");
                    continue;
                }
                break;
            }
            
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7207/messageHub")
                .Build();

            //handler registration
            connection.On<string>("LetterArrived", (letter) =>
            {
                Console.WriteLine($"[SUB] {letter}");
                Console.WriteLine("[SUB] Za potvrdu prijema pisma unesi x:");
            });
            //connection to massage hub
            await connection.StartAsync();

            await connection.InvokeAsync("InitSinglePerson", username, city, ageInt, phoneNumber);
            Console.WriteLine("[SUB] Uspjesno subskrajbovan");


            Console.WriteLine("[SUB] Komande:");
            Console.WriteLine("x - potvrda prijema pisma");
            Console.WriteLine("/block username - blokiranje korisnika");

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "x")
                {
                    await connection.InvokeAsync(
                        "ConfirmReceived",
                        username
                    );
                    Console.WriteLine("[SUB] Potvrdjen prijem pisma.");

                }
                else if (input.StartsWith("/block"))
                {
                    string blockedUsername = input.Replace("/block ", "").Trim();

                    if (string.IsNullOrWhiteSpace(blockedUsername))
                    {
                        Console.WriteLine("[SUB] Morate unijeti username osobe koju blokirate.");
                        continue;
                    }

                    await connection.InvokeAsync("BlockPerson", username, blockedUsername);
                    Console.WriteLine($"[SUB] Blokiran korisnik: {blockedUsername}");
                }
                else
                {
                    Console.WriteLine("[SUB] Nevalidna komanda. Pokusajte opet!");
                }

            }
        }
    }
}

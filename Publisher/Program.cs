using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Publisher
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7207/messageHub")
                .Build();
            await connection.StartAsync();
            Console.WriteLine("[CUPID] Kupid povezan na sistem");

             while (true)
             {
                await connection.InvokeAsync("SendLetter");
                Console.WriteLine("[CUPID] Metoda SendLetter je pozvana.");
                await Task.Delay(60000);
             }
        }
    }
}

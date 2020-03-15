using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace HamBusSig_
{
  public class SigRConnection
  {
    async Task StartConnection()
    {
      HubConnection connection;
      Console.WriteLine("Start of SignalR test client");

      connection = new HubConnectionBuilder()
          .WithUrl("http://localhost:53353/ChatHub")
          .Build();


      connection.Closed += async (error) =>
      {
        await Task.Delay(new Random().Next(0, 5) * 1000);
        await connection.StartAsync();
      };
    }
  }
}

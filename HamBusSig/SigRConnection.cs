using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace HamBusSig
{
  public class SigRConnection
  {
    HubConnection connection;
    public HubConnection StartConnection(string url)
    {

      connection = new HubConnectionBuilder()
          .WithUrl(url)
          .WithAutomaticReconnect()
          .Build();


      connection.Closed += async (error) =>
      {
        await Task.Delay(new Random().Next(0, 5) * 1000);
        await connection.StartAsync();
      };
      connection.Reconnecting += error =>
      {
        Console.WriteLine($"Connection Lost attempting to reconnect: {error.Message}");

        // Notify users the connection was lost and the client is reconnecting.
        // Start queuing or dropping messages.

        return Task.CompletedTask;
      };

      connection.On<string>("loginResponse", (message) =>
      {
        Console.WriteLine($"Got login message: {message}");
      });
      return connection;
    }

    public async void Login(string group, Action<string> cb = null)
    {
      try
      {
        connection.On<string>("loginResponse", cb);
        await connection.InvokeAsync("Login", group);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }
    }
  }
}

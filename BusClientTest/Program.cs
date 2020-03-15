using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BusClientTest
{
  public class Program
  {
    public static void Main(string[] args)
    {
      HubConnection connection;
      Console.WriteLine("Start of SignalR test client");

      connection = new HubConnectionBuilder()
          .WithUrl("http://localhost:7300/masterbus")
          .Build();

      #region snippet_ClosedRestart
      connection.Closed += async (error) =>
      {
        await Task.Delay(new Random().Next(0, 5) * 1000);
        await connection.StartAsync();
      };
      #endregion


      Console.ReadKey();


    }
  }
}

using HamBusSig;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace BusClientTest
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      HubConnection connection;
      
      var busConn = new SigRConnection();
      Console.WriteLine("Start of SignalR test client");

      connection = busConn.StartConnection("http://localhost:7300/masterbus");
      await connection.StartAsync();

      #region snippet_ClosedRestart
      connection.Closed += async (error) =>
      {
        await Task.Delay(new Random().Next(0, 5) * 1000);
        await connection.StartAsync();
      };
      #endregion

      busConn.Login("logging", (foo) =>
      {
        Console.WriteLine($"This is an action: {foo}");
      });

      Console.ReadKey();


    }
  }
}

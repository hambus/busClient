using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreHambusCommonLibrary;
using HambusCommonLibrary;
using Microsoft.AspNetCore.SignalR.Client;

namespace HamBusSig
{
  public class SigRConnection
  {
    public HubConnection? connection = null;
    public async Task<HubConnection> StartConnection(string url)
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



      try
      {
        await connection.StartAsync();
        Console.WriteLine($"connection id: {connection.ConnectionId}");
        //connection.On<string>("loginResponse", (message) =>
        //{
        //  Console.WriteLine($"Got login message: {message}");
        //});
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        Environment.Exit(-1);
      }
      // this needs to go else where
      List<string>? groupList = new List<string>();
      groupList.Add("radio");
      groupList.Add("logging");
      groupList.Add("virtual");
      Login("Flex300", groupList);

      return connection;
    }

    public async void Login(string name, List<string>? group, Action<string>? cb = null)
    {
      try
      {
        connection.On<string>("loginResponse", cb);
        await connection.InvokeAsync("Login", name, group);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }
    }
    private void loginRespCB(string message)
    {
      Console.WriteLine(message);
    }
    public async void sendRigState(RigState state, Action<string>? cb = null)
    {
      Console.WriteLine("Sending state");
      try
      {
        if (cb != null)
          connection.On<string>("loginResponse", cb);
        await connection.InvokeAsync("RadioStateChange", state);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error: {ex.Message}");
      }
    }
  }
}


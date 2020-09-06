using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;

namespace CSharpDuelsHubFunction.Functions
{
    public static class DuelsHub
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "duels", UserId = "{headers.x-ms-client-principal-id}")] SignalRConnectionInfo connectionInfo)
        {
            Console.WriteLine($"url: {connectionInfo.Url} token: {connectionInfo.AccessToken}");
            return connectionInfo;
        }
        [FunctionName("alerts")]
        public static Task SendAlert(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "alerts/{user}")] object message, string user,
            [SignalR(HubName = "duels")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            Console.WriteLine($"message: {JsonConvert.SerializeObject(message)}");
            var messageString = $"{user}::{message}";
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "getAlert",
                    Arguments = new object[] { messageString }
                });
        }
        [FunctionName("joinDuel")]
        public static Task JoinDuel(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "joinDuel/{groupName}/{userName}")] HttpRequest req, string groupName, string userName,
            [SignalR(HubName = "duels")] IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            Console.WriteLine($"{userName} joined group {groupName}");
            return signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = userName,
                    GroupName = groupName,
                    Action = GroupAction.Add
                });
        }

        [FunctionName("duelResult")]
        public static Task DuelResult([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "duelResult/{group}")]
            object message, string group,
            [SignalR(HubName = "duels")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            Console.WriteLine($"message: {JsonConvert.SerializeObject(message)}");
            var messageString = $"{group}::{message}";
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    GroupName = group,
                    Target = "resultAlert",
                    Arguments = new object[] { messageString }
                });
        }
        [FunctionName("create")]
        public static Task SendNewArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "create/{group}/{user}/{challenge}")] object message, string user, string group, string challenge, [SignalR(HubName = "duels")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            Console.WriteLine($"message: {user} created arena {group}");
            var messageString = $"{user}::{group}::{challenge}";
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "showArena",
                    Arguments = new object[] { messageString }
                });
        }
        [FunctionName("leaveDuel")]
        public static Task LeaveDuel([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "leaveDuel/{groupName}/{userName}")] HttpRequest req, string groupName, string userName,
            [SignalR(HubName = "duels")] IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            Console.WriteLine($"{userName} left group {groupName}");
            return signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    UserId = userName,
                    GroupName = groupName,
                    Action = GroupAction.Remove
                });
        }
        [FunctionName("leave")]
        public static Task LeaveDuelMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "leave/{group}/{user}")] object message, string user, string group, [SignalR(HubName = "duels")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            Console.WriteLine($"message: {user} created arena {group}");
            var messageString = $"{user} Removed Arena: {group}";
            return signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "leaveArena",
                    Arguments = new object[] { messageString }
                });
        }
    }
}
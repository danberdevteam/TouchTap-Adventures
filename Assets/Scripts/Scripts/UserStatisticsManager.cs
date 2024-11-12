// using PlayFab;
// using PlayFab.ClientModels;
// using System.Collections.Generic;
// using UnityEngine;

// public class UserStatisticsManager : MonoBehaviour
// {
//     public void GetOverallAccuracy()
//     {
//         PlayFabClientAPI.GetPlayersInSegment(new GetPlayersInSegmentRequest
//         {
//             SegmentId = "YourSegmentId" // You need to replace this with your actual segment ID if you are using segmentation
//         }, result =>
//         {
//             List<string> playerIds = new List<string>();
//             foreach (var playerInfo in result.PlayerProfiles)
//                 playerIds.Add(playerInfo.PlayerId);

//             GetStatisticsForPlayers(playerIds);
//         }, error => Debug.LogError(error.GenerateErrorReport()));
//     }

//     private void GetStatisticsForPlayers(List<string> playerIds)
//     {
//         foreach (string playerId in playerIds)
//         {
//             PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest
//             {
//                 PlayFabId = playerId
//             }, result =>
//             {
//                 foreach (var stat in result.Statistics)
//                 {
//                     if (stat.StatisticName == "Accuracy") // Assuming "Accuracy" is a statistic you store per user
//                         Debug.Log($"Player {playerId} Accuracy: {stat.Value}");
//                 }
//             }, error => Debug.LogError(error.GenerateErrorReport()));
//         }
//     }
// }

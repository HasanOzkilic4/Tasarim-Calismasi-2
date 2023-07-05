using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using System;
//using UnityEditor.PackageManager;
//using static UnityEditor.Progress;
//using UnityEditor.SearchService;
//using UnityEditor.Tilemaps;
//using UnityEditor.Search;

public class PhotonScript : MonoBehaviourPunCallbacks
{

    public static PhotonScript Instance;

    private void Awake()
    {



        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    string RoomName { get; set; }
    byte RoomPlayerCount { get; set; }
    bool CustomRoom { get; set; }

    public void ServerLogin(string roomName = "", bool customRoom = false , byte roomPlayerCount = 0)
    {
        RoomName = roomName;
        RoomPlayerCount = roomPlayerCount;
        CustomRoom = customRoom;
        if (roomName != ""  && roomPlayerCount != 0 && customRoom != false ) // silinebilir.
        {
            RoomName = roomName;
            RoomPlayerCount = roomPlayerCount;
            CustomRoom = customRoom;
        }
        else if(roomName != "" && customRoom != false)
        {
            RoomName = roomName;

        }

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (RoomName != "" && RoomPlayerCount !=0)
        {
          
            PhotonNetwork.CreateRoom(RoomName, new RoomOptions() { MaxPlayers = RoomPlayerCount, IsOpen = true, IsVisible = true }, TypedLobby.Default);
        }
        else if( RoomName != "" && RoomPlayerCount ==0)
        {
          
            PhotonNetwork.JoinRoom(RoomName);
        }
        else
        {
            if (PhotonNetwork.CountOfRooms == 0)
            {
                
                PhotonNetwork.JoinOrCreateRoom((UnityEngine.Random.Range(0f, int.MaxValue)).ToString() + ""
                + (UnityEngine.Random.Range(0f, 10_000f))
                , new RoomOptions() { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
            }
            else
            {
               
                PhotonNetwork.JoinRandomRoom();
            }
        }

       
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username");

    }
    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            var items = GetRandomValuesZeroBetweenNine();


            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();

            for (int i = 0; i < items.Count; i++)
            {
                props.Add($"{i}", items[i]);
            }



            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }



        PhotonNetwork.LoadLevel("Wait");

        List<int> GetRandomValuesZeroBetweenNine()
        {
            List<int> fromZeroToNine = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> result = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                int chooseÝtem = fromZeroToNine[UnityEngine.Random.Range(0, fromZeroToNine.Count)];
                result.Add(chooseÝtem);
                fromZeroToNine.Remove(chooseÝtem);


            }
            return result;
        }

    }








    public override void OnLeftLobby()
    {
       
    }

    public override void OnLeftRoom()
    {
     
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
      
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
      
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
      
    }



}

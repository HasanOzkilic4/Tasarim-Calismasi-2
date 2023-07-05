using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ATGHScript : MonoBehaviour
{
    // Start is called before the first frame update

    DatabaseReference reference;
    FirebaseDatabase database;
    private async void Awake()
    {
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
        reference = database.RootReference;

      
        var a = await IsThereSuchATheGameHistoryInTheSystem(PhotonNetwork.MasterClient.NickName);
        if ((!a.IsSuccess) && PhotonNetwork.IsMasterClient)
        {
            AddTheGameHistory(new TheGameHistory(PhotonNetwork.MasterClient.NickName) { GameOwner = PhotonNetwork.MasterClient.NickName });
        }
    }
     void Start()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            sETFORScoreCongig();

        }



    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Invoke("sETFORScoreCongig" , 3f);

        }

    }
    public TextMeshProUGUI mc;
    public TextMeshProUGUI nmc;

    public Player NoMaster()
    {
        Player player = null;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber != PhotonNetwork.MasterClient.ActorNumber)
            {
                player = PhotonNetwork.PlayerList[i];
                break;

            }



        }
        return player;
    }
    void Update()
    {
        
    }
    public async Task<TheGameHistory> Get(string gameOwner)
    {
        string gameOwnerName = String.Empty;
        int masterClientScore = 0;
        int noMasterClientScore = 0;
        int masterClient1Score = 0;
        int masterClient2Score = 0;
        int masterClient3Score = 0;
        int noMasterClient1Score = 0;
        int noMasterClient2Score = 0;
        int noMasterClient3Score = 0;
        TheGameHistory theGameHistory = new TheGameHistory(gameOwner);

        var result = await reference.Child("TheGameHistory").Child(gameOwner).GetValueAsync();

        foreach (var item in result.Children.ToList())
        {
            switch (item.Key)
            {
                case "GameOwner":
                    gameOwnerName = item.Value.ToString();
                    break;

                case "MasterClientScore":
                    masterClientScore = int.Parse(item.Value.ToString());
                    break;
                case "NoMasterClientScore":
                    noMasterClientScore = int.Parse(item.Value.ToString());
                    break;
                case "MasterClient1Score":
                    masterClient1Score = int.Parse(item.Value.ToString());
                    break;
                case "MasterClient2Score":
                    masterClient2Score = int.Parse(item.Value.ToString());
                    break;
                case "MasterClient3Score":
                    masterClient3Score = int.Parse(item.Value.ToString());
                    break;
                case "NoMasterClient1Score":
                    noMasterClient1Score = int.Parse(item.Value.ToString());
                    break;
                case "NoMasterClient2Score":
                    noMasterClient2Score = int.Parse(item.Value.ToString());
                    break;
                case "NoMasterClient3Score":
                    noMasterClient3Score = int.Parse(item.Value.ToString());
                    break;
            }
        }
        theGameHistory = new TheGameHistory(gameOwner)
        {
            GameOwner = gameOwner,
            MasterClientScore = masterClientScore,
            NoMasterClientScore = noMasterClientScore,
            MasterClient1Score = masterClient1Score,
            MasterClient2Score = masterClient2Score,
            MasterClient3Score = masterClient3Score,
            NoMasterClient1Score = noMasterClient1Score,
            NoMasterClient2Score = noMasterClient2Score,
            NoMasterClient3Score = noMasterClient3Score
        };
        return theGameHistory;
    }

    public async void sETFORScoreCongig()
    {

        //if (PhotonNetwork.MasterClient.NickName != PhotonNetwork.LocalPlayer.NickName)
        //{
        //    nmc.text = PhotonNetwork.LocalPlayer.NickName;
        //}

        var a = await Get(PhotonNetwork.MasterClient.NickName);
        if (a == null && PhotonNetwork.MasterClient.NickName == null)
        {
            return;
        }

       

        mc.text = PhotonNetwork.MasterClient.NickName +  " \n " + a.MasterClientScore.ToString();
        nmc.text = NoMaster().NickName+ " \n " + a.NoMasterClientScore.ToString();
    }
    public void AddTheGameHistory(TheGameHistory entity)
    {
        string userWithJson = JsonUtility.ToJson(entity);
        reference.Child("TheGameHistory").Child(entity.GameOwner).SetRawJsonValueAsync(userWithJson);

    }

    async Task<IResult> IsThereSuchATheGameHistoryInTheSystem(string gameOwner)
    {
        List<TheGameHistory> users = await GetAll();

        if (users.Any(u => u.GameOwner == gameOwner))
        {
            return new Result(true, "Böyle bir oyun ismi zaten mevcut");

        }

        return new Result(false);

    }



    public async Task<List<TheGameHistory>> GetAll(Func<TheGameHistory, bool> filter = null)
    {
        if (filter == null)
        {
            var users = await GetAllUsers();
            return users;
        }
        else
        {
            var users = await GetAllUsers();
            return users.Where(filter).ToList();

        }

        async Task<List<TheGameHistory>> GetAllUsers()
        {

            List<TheGameHistory> getAllTheGameHistory = new List<TheGameHistory>();
            var getTheGameHistory = await reference.Child("TheGameHistory").GetValueAsync();
            foreach (var user in getTheGameHistory.Children.ToList())
            {
                string gameOwner = String.Empty;


                TheGameHistory newUser = null;

                foreach (var item in user.Children.ToList())
                {


                    switch (item.Key)
                    {
                        case "GameOwner":
                            gameOwner = item.Value.ToString();
                            break;
                    }
                }
                newUser = new TheGameHistory(gameOwner)
                {
                    GameOwner = gameOwner
                };
                getAllTheGameHistory.Add(newUser);


            }

            return getAllTheGameHistory;
        }

    }

}

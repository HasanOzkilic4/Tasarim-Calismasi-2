using Assets.Concrete;
using Assets.Scripts.Entities;
using Firebase.Database;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI WÝNNER_THE_GAME;
    FirebaseDatabase database;
    DatabaseReference reference;
    DatabaseUserDal _databaseUserDal;
    public TextMeshProUGUI winOrLose;
    public GameObject winOrLosePanel;
    private void Awake()
    {
        clockscript.KACÝNCÝ_OYUN = 0;
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
        reference = database.RootReference;
        _databaseUserDal = new DatabaseUserDal();
        if (PhotonNetwork.IsMasterClient)
        {
            AddedScoreToWinnerPlayer();
            DeletedScoreToLoserPlayerManager();
        }


    }
    void Start()
    {
        //    gameObject.GetComponent<PhotonView>().RPC("WÝNNER_THE_GAME_MANAGER" , RpcTarget.AllBufferedViaServer);
        WÝNNER_THE_GAME_MANAGER();
  
    }


    //[PunRPC]
    public async void WÝNNER_THE_GAME_MANAGER()
    {

        var gameHistory = await Get(PhotonNetwork.MasterClient.NickName);


        
        if (gameHistory.MasterClientScore > gameHistory.NoMasterClientScore && PhotonNetwork.IsMasterClient)
        {
            winOrLose.text = "TEBRÝKLER "+ PhotonNetwork.LocalPlayer.NickName +  " KAZANDINIZ";
            winOrLosePanel.GetComponent<Image>().color = Color.blue;
            WÝNNER_THE_GAME.text = "+10 puan kazandýnýz";


        }
        else if (gameHistory.NoMasterClientScore > gameHistory.MasterClientScore && !PhotonNetwork.IsMasterClient)
        {
            winOrLose.text = "TEBRÝKLER " + PhotonNetwork.LocalPlayer.NickName + " KAZANDINIZ";
            winOrLosePanel.GetComponent<Image>().color = Color.blue;
            WÝNNER_THE_GAME.text = "+10 puan kazandýnýz";

        }
        if (gameHistory.MasterClientScore < gameHistory.NoMasterClientScore && PhotonNetwork.IsMasterClient)
        {
            winOrLose.text = "KAYBETTÝNÝZ " + PhotonNetwork.LocalPlayer.NickName;
            winOrLosePanel.GetComponent<Image>().color = Color.red;
            WÝNNER_THE_GAME.text = " 5 puan  kaybettiniz";

        }
        if (gameHistory.MasterClientScore > gameHistory.NoMasterClientScore && !PhotonNetwork.IsMasterClient)
        {
            winOrLose.text = "KAYBETTÝNÝZ " + PhotonNetwork.LocalPlayer.NickName;
            winOrLosePanel.GetComponent<Image>().color = Color.red;
            WÝNNER_THE_GAME.text = "5 puan kaybettiniz";

        }
      

   
    }


 
   

    public async void AddedScoreToWinnerPlayer()
    {
        var gameHistory = await Get(PhotonNetwork.MasterClient.NickName);
        if (gameHistory.MasterClientScore > gameHistory.NoMasterClientScore)
        {
            var c = await _databaseUserDal.Get(PhotonNetwork.MasterClient.NickName);
            c.Score += 10;
            _databaseUserDal.Update(PhotonNetwork.MasterClient.NickName, c);

        }
        else
        {
            var c = await _databaseUserDal.Get(NoMaster().NickName);
            c.Score += 10;
            _databaseUserDal.Update(NoMaster().NickName, c);
        }

    }

    public async void DeletedScoreToLoserPlayerManager()
    {
        var gameHistory = await Get(PhotonNetwork.MasterClient.NickName);
        if (gameHistory.MasterClientScore > gameHistory.NoMasterClientScore)
        {
            var c = await _databaseUserDal.Get(NoMaster().NickName);
            c.Score -= 5;
            if (c.Score<0)
            {
                c.Score = 0;
            }
            _databaseUserDal.Update(NoMaster().NickName, c);
        }
        else
        {
            var c = await _databaseUserDal.Get(PhotonNetwork.MasterClient.NickName);
            c.Score -= 5;
            if (c.Score < 0)
            {
                c.Score = 0;
            }
            _databaseUserDal.Update(PhotonNetwork.MasterClient.NickName, c);
        }

    }

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

    public void GameMenu()
    {
        SceneManager.LoadScene("UserPage");
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
        TheGameHistory theGameHistory;


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


}

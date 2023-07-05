using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;
using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using System.Linq;
using System.Threading.Tasks;
using System;
using Firebase.Database;
using Photon.Realtime;


public class WaitManager : MonoBehaviour, IPunObservable
{
    static int d = 0;
    float timer;
    public TextMeshProUGUI textTimer;



    DatabaseReference reference;
    FirebaseDatabase database;

    public GameObject ExitButton;
    //  PhotonView photonView;


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

 
    private  void Start()
    {
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
       reference = database.RootReference;

        d += 1;
        timer = 4f;
     
      
        PhotonNetwork.LocalPlayer.AddScore(0);
       
     
    }

    public TextMeshProUGUI playerIsWaiting;
    public void WaitingPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            playerIsWaiting.enabled = false;
            ExitButton.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        WaitingPlayer();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && d==1 )
        {
            timer= 0;
        }
        if (!PhotonNetwork.IsMasterClient)
        {
           
            return;
        }

        timer -= Time.fixedDeltaTime;
        textTimer.text = timer.ToString("F1");
        
        if (timer <= 0 && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
           PhotonNetwork.LoadLevel("GameScene");
            Destroy(this);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(timer);
        }
        else if (stream.IsReading)
        {
            timer = (float)stream.ReceiveNext();
            textTimer.text = timer.ToString("F1");
        }
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

    public void Exit()
    {

       
       
        SceneManager.LoadScene("UserPage");
    }
}

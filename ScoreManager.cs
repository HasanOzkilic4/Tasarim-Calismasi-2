using Assets.Scripts.Entities;
using Firebase.Database;
using JetBrains.Annotations;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI user1Score;
    public TextMeshProUGUI user2Score;
    static FirebaseDatabase database;
    static DatabaseReference reference;
    TheGameHistory a;

    private  void Awake()
    {

        
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
        reference = database.RootReference;
  






    }

    private   void Start()
    {
     //   a = await Get(PhotonNetwork.MasterClient.NickName);

        // gameObject.GetComponent<PhotonView>().RPC("ScoreAyarlama", RpcTarget.AllBufferedViaServer);
        ScoreAyarlama();
        

     

    }

    private void FixedUpdate()
    {
        ScoreAyarlamaByInvoke();
    }

    public void ScoreAyarlamaByInvoke()
    {
        Invoke("ScoreAyarlama", 3f);
    }



    // [PunRPC]
    public async void ScoreAyarlama()
    {
        a = await Get(PhotonNetwork.MasterClient.NickName);


        if (clockscript.KACÝNCÝ_OYUN == 1 || clockscript.KACÝNCÝ_OYUN == 2 || clockscript.KACÝNCÝ_OYUN == 3 )
        {
            switch (clockscript.KACÝNCÝ_OYUN)
            {
                case 1:
                    user1Score.text = a.MasterClientScore.ToString() ;
                    user2Score.text = a.NoMasterClientScore.ToString();
                    break;

                case 2:
                    user1Score.text = a.MasterClientScore.ToString() ;
                    user2Score.text = a.NoMasterClientScore.ToString();
                    break;

                case 3:
                    user1Score.text = a.MasterClientScore.ToString();
                    user2Score.text = a.NoMasterClientScore.ToString();
                    break;

            }



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










using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
   // public static int CLÝCK_COUNT;
    void Start()
    {
       // CLÝCK_COUNT = 10;
        if (PhotonNetwork.LocalPlayer.NickName != PhotonNetwork.MasterClient.NickName)
        {
            if (NoMaster().GetScore() < PhotonNetwork.MasterClient.GetScore())
            {
                NoMaster().SetScore(NoMaster().GetScore()+ 10);
            }
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


}

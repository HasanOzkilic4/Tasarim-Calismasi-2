using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using TMPro;

public class User2JokerScript : MonoBehaviour
{
    public GameObject TimeCounterText;
    public GameObject HealthText;

    public void CanClickJokerCard()
    {
        PhotonNetwork.MasterClient.CustomProperties.TryGetValue("healthCount", out object healthCount);
        var hash = PhotonNetwork.MasterClient.CustomProperties;
        int health = (int)healthCount;

        if (!PhotonNetwork.IsMasterClient && WhoPlayingPlayer().NickName != PhotonNetwork.MasterClient.NickName
            && health !=0)
        {
            TimeCounterText.GetComponent<PhotonView>().RPC("ClockStart", RpcTarget.AllBufferedViaServer, false);
            TimeCounterText.GetComponent<PhotonView>().RPC("ClockStart", RpcTarget.AllBufferedViaServer, true);
            health--;
            hash["healthCount"] = health;
            PhotonNetwork.MasterClient.SetCustomProperties(hash);
            gameObject.GetComponent<PhotonView>().RPC("HealthTextManager" , RpcTarget.AllBufferedViaServer , health);
     
        }
        else
        {
            playingPlayer = PhotonNetwork.LocalPlayer;
        }

    }
    [PunRPC]
    public void HealthTextManager(int health)

    {
        HealthText.GetComponent<TextMeshProUGUI>().text = health.ToString();
    }
    Player playingPlayer = PhotonNetwork.LocalPlayer;
    public Player WhoPlayingPlayer()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.PlayerList[i].NickName)
            {
                continue;
            }


            if (playingPlayer.GetScore() == PhotonNetwork.PlayerList[i].GetScore())
            {
                playingPlayer = PhotonNetwork.MasterClient;
            }
            else
            {
                playingPlayer = NoMaster();

            }
        }
        return playingPlayer;
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

using Photon.Realtime;
//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
//using System.Collections;

public class User1Process : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ArrowForUser1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Calculate()
    {
        PhotonNetwork.MasterClient.CustomProperties.TryGetValue("canClickToProcess" , out object canClickToProcess);
        if (PhotonNetwork.IsMasterClient && ((bool)canClickToProcess) && WhoPlayingPlayer().NickName == PhotonNetwork.MasterClient.NickName)
        {
            switch (gameObject.GetComponentInChildren<TextMeshProUGUI>().text)
            {
                case "+":
                    SetPlayerProperties();
                    break;


                case "-":
                    SetPlayerProperties();
                    break;


                case "*":
                    SetPlayerProperties();
                    break;


                case "/":
                    SetPlayerProperties();
                    break;


                default:
                    break;
            }
        }
        else
        {
            playingPlayer = PhotonNetwork.LocalPlayer;
        }
      
       
   
    }

    void SetPlayerProperties()
    {
        var hashtable = PhotonNetwork.MasterClient.CustomProperties;
        
        hashtable["process"] = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        hashtable["canClickForRightNumber"] = true;
        hashtable["canClickToProcess"] = false;
        PhotonNetwork.MasterClient.SetCustomProperties(hashtable);

      

        GetComponent<PhotonView>().RPC("ButtonColor", RpcTarget.AllBufferedViaServer);
        









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


    [PunRPC]
    public void ButtonColor()
    {
        gameObject.GetComponent<Button>().colors = ColorBlock.defaultColorBlock;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        ArrowForUser1.SetActive(false);
        gameObject.GetComponent<Button>().interactable = false;
    }
}

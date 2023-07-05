using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class User2Process : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ArrowForUser2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Calculate()
    {
        NoMaster().CustomProperties.TryGetValue("canClickToProcess", out object canClickToProcess);
        if (!PhotonNetwork.IsMasterClient && ((bool)canClickToProcess) && WhoPlayingPlayer().NickName == NoMaster().NickName)
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
        var hashtable = NoMaster().CustomProperties;

        hashtable["process"] = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        hashtable["canClickForRightNumber"] = true;
        hashtable["canClickToProcess"] = false;
        NoMaster().SetCustomProperties(hashtable);



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
        ArrowForUser2.SetActive(false);
        gameObject.GetComponent<Button>().interactable = false;
    }
}

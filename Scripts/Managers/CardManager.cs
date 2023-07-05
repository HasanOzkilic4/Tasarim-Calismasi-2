using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviourPunCallbacks //, IPunObservable
{
    public GameObject Card;
    public Transform CardsParent;
    public GameObject SetDesign;
    GameObject[] otherCards;


    public TextMeshProUGUI userName1;
    public TextMeshProUGUI userName2;

    public GameObject user1IsPlay;
    public GameObject user2IsPlay;

    void Awake()
    {

       // GetComponent<PhotonView>().RPC("CardInitilazer", RpcTarget.Others);
        CardsManager(PhotonNetwork.IsMasterClient);



    
    }

    [PunRPC]
    public void SetIsPlayUser(int number)
    {
        if (number == 1 )
        {
            user1IsPlay.GetComponent<Image>().color = Color.green;
            user2IsPlay.GetComponent<Image>().color = Color.red;
        }
        else if(number == 2)
        {
            user2IsPlay.GetComponent<Image>().color = Color.green;
            user1IsPlay.GetComponent<Image>().color = Color.red;
        }


    }

    [PunRPC]
    public void UserNameSet()
    {
        userName1.text = PhotonNetwork.MasterClient.NickName;
        userName2.text = NoMaster().NickName;
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
    private void Start()
    {
        Invoke("CardsDesignStart" , 7f);
        GetComponent<PhotonView>().RPC("UserNameSet" , RpcTarget.AllBufferedViaServer);

     
    }

    public void CardsDesignStart()
    {
        GetComponent<PhotonView>().RPC("CardsDesign", RpcTarget.AllBufferedViaServer, PhotonNetwork.IsMasterClient);
    }



    [PunRPC]
    public void  CardsDesign(bool isMasterClient)
    {
        if (isMasterClient)
        {
           otherCards =  GameObject.FindGameObjectsWithTag("nomaster");
            foreach (var item in otherCards)
            {
                item.transform.position = Camera.main.WorldToScreenPoint(new Vector3(.9f, 0, 0));
                item.transform.SetParent(CardsParent);
                
                
            }
        }
       
    }

    [PunRPC]
    public void CardInitilazer()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i %2 ==0 && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue($"{i}", out object value);
                GameObject card = PhotonNetwork.Instantiate("Card", Camera.main.WorldToScreenPoint(new Vector3(.9f, 0, 0)), Quaternion.identity);
                card.GetComponent<PhotonView>().RPC("Initilaze", RpcTarget.AllBufferedViaServer, ((int)value).ToString(), "master");
                card.transform.SetParent(CardsParent);
            }
            else if(!PhotonNetwork.IsMasterClient && i%2 ==1)
            {
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue($"{i}", out object value);
                GameObject card = PhotonNetwork.Instantiate("Card", Camera.main.WorldToScreenPoint(new Vector3(.9f, 0, 0)), Quaternion.identity);

                card.GetComponent<PhotonView>().RPC("Initilaze", RpcTarget.AllBufferedViaServer, ((int)value).ToString(), "nomaster");
                card.transform.SetParent(CardsParent);
            }
          
        }
    }

    [PunRPC]
    public void CardsManager(bool isMasterClient)
    {
        if (isMasterClient)
        {
            for (int i = 0; i < 10; i+=2)
            {
                PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue($"{i}" , out object value);
                GameObject card = PhotonNetwork.Instantiate("Card", Camera.main.WorldToScreenPoint(new Vector3(.9f, 0, 0)), Quaternion.identity);
              
                card.GetComponent<PhotonView>().RPC("Initilaze" , RpcTarget.AllBufferedViaServer , ((int)value).ToString() , "master");
              
               
                
                card.transform.SetParent(CardsParent);
            }
           
        }
        else
        {
            // sonradan eklendi CardsDesign 'in else alaný
            Invoke("NoMasterClientCardDesignManager" , 4f);
            Invoke("NoMasterClientCardsDesign", 5f);
          
            
        }
    }
    public void NoMasterClientCardDesignManager()
    {
        otherCards = GameObject.FindGameObjectsWithTag("master");
        foreach (var item in otherCards)
        {
            item.transform.position = Camera.main.WorldToScreenPoint(new Vector3(.9f, 0, 0));
            item.transform.SetParent(CardsParent);
        }

    }
    public void NoMasterClientCardsDesign()
    {
        for (int i = 1; i < 10; i += 2)
        {
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue($"{i}", out object value);
            GameObject card = PhotonNetwork.Instantiate("Card", Camera.main.WorldToScreenPoint(new Vector3(.9f, 0, 0)), Quaternion.identity);

            card.GetComponent<PhotonView>().RPC("Initilaze", RpcTarget.AllBufferedViaServer, ((int)value).ToString(), "nomaster");


            card.transform.SetParent(CardsParent);
        }
    }


}

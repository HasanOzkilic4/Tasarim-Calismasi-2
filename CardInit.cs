using Assets.Scripts.Entities;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CardInit : MonoBehaviourPunCallbacks
{

    


    Player playingPlayer;

    TextMeshProUGUI txt;



    GameObject CardsPanel;


    GameObject CardManager;

    GameObject ArrowForUser1;
    GameObject ArrowForUser2;




    ExitGames.Client.Photon.Hashtable props;

    public void PlayerConfig()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            props = new ExitGames.Client.Photon.Hashtable()
          {
              { "user1Box1" , new Vector3(16728f , 19727.29f , 10f)} ,
              { "user1Box2" , new Vector3(82173.84f, 19727.29f, 10.00f)} ,
              { "user1ResultBox" , new Vector3(115867.70f, 19727.29f, 10.00f)} ,
                { "clickCount" , 0},
                {"leftNumber" , 0 },
                {"rightNumber" , 0 },
                   {"canClickForRightNumber" ,true },
                {"process" , "" },
                {"arrow" , 1 },
                 {"canClickToProcess" ,false },
                {"tag1", "left1" },
                {"tag2" ,"right1" },
                    {"healthCount" , 3 },
                {"number" , 0 }
              



          };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else
        {
            props = new ExitGames.Client.Photon.Hashtable()
          {
              { "user2Box1" , new Vector3(16728.00f, 98120.16f, 10.00f)} ,
              { "user2Box2" , new Vector3(81922.20f, 98120.16f, 10.00f)} ,
              { "user2ResultBox" , new Vector3(115163.50f, 98120.16f, 10.00f)} ,
                { "clickCount" , 0 },
                {"leftNumber" , 0 },
                {"rightNumber" , 0 } ,
                {"canClickForRightNumber" ,true },
                  {"process" , "" },
                   {"arrow" , 2 },
                {"canClickToProcess" ,false },
                 {"tag1", "left2" },
                {"tag2" ,"right2" },
                {"healthCount" , 3 },
               



          };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        }
    }




    GameObject TimeCounterText;
    TextMeshProUGUI master, noMaster;
    private void Awake()
    {
    

       
      

        playingPlayer = PhotonNetwork.LocalPlayer;
       
        PlayerConfig();
        CardsPanel = GameObject.Find("CardsPanel");

        CardManager = GameObject.Find("CardManager");
        ArrowForUser1 = GameObject.Find("ArrowForUser1");
        ArrowForUser2 = GameObject.Find("ArrowForUser2");
       
        TimeCounterText = GameObject.Find("Time"); //TimeCounterText olacak

        noMaster = GameObject.Find("CurrentNoMasterClientScore").GetComponent<TextMeshProUGUI>();
        master = GameObject.Find("CurrentMasterClientScore").GetComponent<TextMeshProUGUI>();










    }
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
       
        CardManager.GetComponent<PhotonView>().RPC("SetIsPlayUser", RpcTarget.AllBufferedViaServer, 1);


    }



   
    public void Deneme()
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
        playingPlayer.CustomProperties.TryGetValue("canClickForRightNumber", out object canClickForRightNumber);

      
        if (playingPlayer.NickName == PhotonNetwork.LocalPlayer.NickName && ((bool)canClickForRightNumber) && ThisGameWinner.COUNTER == 0)
        {
            if (playingPlayer.NickName == PhotonNetwork.MasterClient.NickName)
            {
                PhotonNetwork.LocalPlayer.SetScore(PhotonNetwork.LocalPlayer.GetScore() + 10);
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("user1Box1", out object user1Box1);
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("user1Box2", out object user1Box2);

                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("clickCount", out object clickCount);
                object vector3 = ((int)clickCount) == 0 ? user1Box1 : user1Box2;



                props["canClickForRightNumber"] = false;
                props["canClickToProcess"] = true;

                string tag = (int)clickCount == 0 ? "left1" : "right1";

                GetComponent<PhotonView>().RPC("OnClick", RpcTarget.AllBufferedViaServer, (Vector3)vector3, (int)clickCount, tag);


                props["clickCount"] = (int)clickCount + 1;

                if ((int)clickCount == 0)
                {
                    props["leftNumber"] = int.Parse(txt.text);
                }
                else
                {
                    props["rightNumber"] = int.Parse(txt.text);

                    playingPlayer.CustomProperties.TryGetValue("process", out object process);
                    string ýslem = (string)process;
                    playingPlayer.CustomProperties.TryGetValue("leftNumber", out object leftNumber);
                    int left = (int)leftNumber;

                    PhotonNetwork.MasterClient.CustomProperties.TryGetValue("user1ResultBox", out object user1ResultBox);
                    Vector3 resultBox = (Vector3)user1ResultBox;
                    int result = 0;
                    GameObject card = new GameObject();
                    switch (ýslem)
                    {
                        // result box bir kart olabilir ve bu kartýn görünürlüðünü açabilirsin...
                        case "+":

                            result = left + int.Parse(txt.text);
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);

                            break;


                        case "-":
                            result = left - int.Parse(txt.text);
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);
                            break;


                        case "*":
                            result = left * int.Parse(txt.text);
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);
                            break;


                        case "/":
                            int yeniSonuc = int.Parse(txt.text) == 0 ? 1 : int.Parse(txt.text);
                            result = left / yeniSonuc;
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);
                            break;


                        default:
                            break;


                    }
                 


                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);




                CardManager.GetComponent<PhotonView>().RPC("SetIsPlayUser", RpcTarget.AllBufferedViaServer, 2);

              


            }
            else
            {
                PhotonNetwork.LocalPlayer.SetScore(PhotonNetwork.LocalPlayer.GetScore() + 10);
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("user2Box1", out object user2Box1);
                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("user2Box2", out object user2Box2);


                PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("clickCount", out object clickCount);
                object vector3 = ((int)clickCount) == 0 ? user2Box1 : user2Box2;

                props["canClickForRightNumber"] = false;
                props["canClickToProcess"] = true;

                string tag = (int)clickCount == 0 ? "left2" : "right2";

                GetComponent<PhotonView>().RPC("OnClick", RpcTarget.AllBufferedViaServer, (Vector3)vector3, (int)clickCount, tag);


                props["clickCount"] = (int)clickCount + 1;

                if ((int)clickCount == 0)
                {
                    props["leftNumber"] = int.Parse(txt.text);
                }
                else
                {
                    props["rightNumber"] = int.Parse(txt.text);

                    playingPlayer.CustomProperties.TryGetValue("process", out object process);
                    string ýslem = (string)process;
                    playingPlayer.CustomProperties.TryGetValue("leftNumber", out object leftNumber);
                    int left = (int)leftNumber;

                    playingPlayer.CustomProperties.TryGetValue("user2ResultBox", out object user2ResultBox);
                    Vector3 resultBox = (Vector3)user2ResultBox;
                    int result = 0;
                    GameObject card = new GameObject();
                    switch (ýslem)
                    {
                        // result box bir kart olabilir ve bu kartýn görünürlüðünü açabilirsin...
                        case "+":

                            result = left + int.Parse(txt.text);
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);

                            break;


                        case "-":
                            result = left - int.Parse(txt.text);
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);
                            break;


                        case "*":
                            result = left * int.Parse(txt.text);
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);
                            break;


                        case "/":
                            int yeniSonuc = int.Parse(txt.text) == 0 ? 1 : int.Parse(txt.text);
                            result = left / yeniSonuc;
                            card = PhotonNetwork.Instantiate("ResultCard", Camera.main.ScreenToWorldPoint(resultBox), Quaternion.identity);
                            card.GetComponent<PhotonView>().RPC("InitilazaForResults", RpcTarget.AllBufferedViaServer, result);
                            break;


                        default:
                            break;
                    }

                   


                }

                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                CardManager.GetComponent<PhotonView>().RPC("SetIsPlayUser", RpcTarget.AllBufferedViaServer, 1);




            }


           
            
                TimeCounterText.GetComponent<PhotonView>().RPC("ClockStart", RpcTarget.AllBufferedViaServer, false);
                TimeCounterText.GetComponent<PhotonView>().RPC("ClockStart", RpcTarget.AllBufferedViaServer, true);
            
           
            //
            gameObject.GetComponent<PhotonView>().RPC("SetCurrentScore", RpcTarget.AllBufferedViaServer);

            //  CardsPanelChildCount();
            //GetScoreManager.CLÝCK_COUNT--;

            //gameObject.GetComponent<PhotonView>().RPC("GameExit", RpcTarget.AllBufferedViaServer);


        }
        else if (playingPlayer.NickName == PhotonNetwork.LocalPlayer.NickName && !((bool)canClickForRightNumber))
        {
            playingPlayer.CustomProperties.TryGetValue("arrow", out object arrow);
            int arrw = ((int)arrow);

            GetComponent<PhotonView>().RPC("CantClickRightNumber", RpcTarget.AllBufferedViaServer, arrw);

        }

        playingPlayer = PhotonNetwork.LocalPlayer;
        gameObject.GetComponent<PhotonView>().RPC("SetCurrentScore", RpcTarget.AllBufferedViaServer);
    }




    [PunRPC]
    public void CantClickRightNumber(int number)
    {
        // lütfen iþlem seçiniz...
        GameObject newArrow = number == 1 ? ArrowForUser1 : ArrowForUser2;

    

        if (!newArrow.activeSelf)
        {
            newArrow.SetActive(true);
        }
        newArrow.GetComponent<Image>().enabled = true;
        newArrow.GetComponent<Animator>().enabled = true;





    }


[PunRPC]
    public void OnClick(Vector3 vector, int clickCount, string tagName)
    {



        txt.enabled = true;


        gameObject.tag = tagName;
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(vector);

        gameObject.GetComponent<Button>().interactable = false;





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

    public Vector3 ScreenToWorldPointManager(GameObject objectForWorldPoint)
    {
        var temp = Camera.main.WorldToScreenPoint(objectForWorldPoint.transform.position);
        return Camera.main.ScreenToWorldPoint(temp);
    }


    [PunRPC]
    public void Initilaze(string cardName, string cardTagName)
    {
        gameObject.name = cardName;
        gameObject.tag = cardTagName;
        GetComponentInChildren<TextMeshProUGUI>().text = cardName;
    }


    [PunRPC]
    public void SetCurrentScore()
    {
        int currentMasterClientScore = default, currentNoMasterClientScore = default;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.NickName == PhotonNetwork.MasterClient.NickName)
            {
                item.CustomProperties.TryGetValue("tag1", out object tag1);
                var result = GameObject.FindWithTag((string)tag1);
                currentMasterClientScore = result == null ? 0 : int.Parse(result.GetComponentInChildren<TextMeshProUGUI>().text);
                master.text = currentMasterClientScore.ToString();


            }
            else
            {
                item.CustomProperties.TryGetValue("tag1", out object tag1);
                var result = GameObject.FindWithTag((string)tag1);
                currentNoMasterClientScore = result == null ? 0 : int.Parse(result.GetComponentInChildren<TextMeshProUGUI>().text);
                noMaster.text = currentNoMasterClientScore.ToString();

            }
        }
    }






}

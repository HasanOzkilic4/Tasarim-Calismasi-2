
using Assets.Scripts.Entities;
using Firebase.Database;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class clockscript : MonoBehaviour
{
    static FirebaseDatabase database;
    static DatabaseReference reference;
 
     TheGameHistory a;


    public GameObject CardsPanel;
   

    private async void Awake()
    {
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
        reference = database.RootReference;
        a = await Get(PhotonNetwork.MasterClient.NickName);
    }


 



    [PunRPC]
    public void ClockStart(bool canSayac)
    {
        if (canSayac)
        {
            StartCoroutine(ClockTime());
        }
        else
        {
            StopAllCoroutines();
        }

    }

    public static int KACÝNCÝ_OYUN = 0;
    IEnumerator ClockTime()
    {
        for (int i = 10; i >= 0; i--)
        {
            if (i == 0)
            {

                KACÝNCÝ_OYUN++;
                int currentMasterClientScore = default, currentNoMasterClientScore = default;

                string kacinci_oyun = KACÝNCÝ_OYUN == 1 ? "game1" : (KACÝNCÝ_OYUN == 2 ? "game2" : "game3");
                foreach (var item in PhotonNetwork.PlayerList)
                {
                    if (item.NickName == PhotonNetwork.MasterClient.NickName)
                    {
                        item.CustomProperties.TryGetValue("tag1", out object tag1);
                        var result = GameObject.FindWithTag((string)tag1);
                        currentMasterClientScore = result == null ? 1000 : int.Parse(result.GetComponentInChildren<TextMeshProUGUI>().text);


                    }
                    else
                    {
                        item.CustomProperties.TryGetValue("tag1", out object tag1);
                        var result = GameObject.FindWithTag((string)tag1);
                        currentNoMasterClientScore = result == null ? 1000 : int.Parse(result.GetComponentInChildren<TextMeshProUGUI>().text);

                    }
                }







                int currentRandomNumber = PlayerPrefs.GetInt("randomNumber");

                if (PhotonNetwork.IsMasterClient)
                {
                    if (currentMasterClientScore == 1000 && currentNoMasterClientScore == 1000)
                    {
                        currentNoMasterClientScore = 0;
                    }
                    else if(PhotonNetwork.MasterClient.GetScore() == NoMaster().GetScore() && CardsPanel.GetComponentsInChildren<TextMeshProUGUI>().Length != 2)
                    {
                        // Eðer sýra master Client daysa ve sýrasýný oynamamýþsa yenilir.
                        //CardsPanel.GetComponentsInChildren<TextMeshProUGUI>().Length != 2 oyun bitmemiþ anlamýna gelir.
                        currentMasterClientScore = 1000;
                    }
                    else if (PhotonNetwork.MasterClient.GetScore() > NoMaster().GetScore() && CardsPanel.GetComponentsInChildren<TextMeshProUGUI>().Length != 2)
                    {
                        // Eðer sýra no Master Cllient daysa ve sýrasýný oynamamýþsa yenilir.
                        currentNoMasterClientScore = 1000;
                    }


                    if (Mathf.Abs(currentRandomNumber - currentMasterClientScore) < Mathf.Abs(currentRandomNumber - currentNoMasterClientScore))
                    {
                        a.MasterClientScore += 1;
                    }
                    else if (Mathf.Abs(currentRandomNumber - currentMasterClientScore) > Mathf.Abs(currentRandomNumber - currentNoMasterClientScore))
                    {
                        a.NoMasterClientScore += 1;
                    }
                    else
                    {
                        KACÝNCÝ_OYUN--;
                        // return equals
                    }



                    if (kacinci_oyun == "game1")
                    {
                        a.MasterClient1Score = currentMasterClientScore;
                        a.NoMasterClient1Score = currentNoMasterClientScore;
                    }
                    else if (kacinci_oyun == "game2")
                    {
                        a.MasterClient2Score = currentMasterClientScore;
                        a.NoMasterClient2Score = currentNoMasterClientScore;
                    }
                    else if (kacinci_oyun == "game3")
                    {
                        a.MasterClient3Score = currentMasterClientScore;
                        a.NoMasterClient3Score = currentNoMasterClientScore;
                    }

                    new RandomNumberManager().UpdateTheGameHistory(PhotonNetwork.MasterClient.NickName, a);
                }












                if (PhotonNetwork.IsMasterClient)
                {
                    var items = GetRandomValuesZeroBetweenNine();
                    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                    for (int j = 0; j < items.Count; j++)
                    {
                        props.Add($"{j}", items[j]);
                    }
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }

                if (a.MasterClientScore == 3 || a.NoMasterClientScore == 3 ||
                    (a.MasterClientScore == 2 && a.NoMasterClientScore == 0) || (a.MasterClientScore == 0 && a.NoMasterClientScore == 2) 
                    || (a.MasterClientScore ==2 && a.NoMasterClientScore == 1) || (a.MasterClientScore == 1 && a.NoMasterClientScore == 2))
                {

                    if (a.MasterClientScore == 3 || (a.MasterClientScore == 2 && a.NoMasterClientScore == 0) 
                        || (a.MasterClientScore == 2 && a.NoMasterClientScore == 1))
                    {
                        // PlayerPrefs.SetString("winner", "MasterClient");
                        // ThisGameWinner.THIS_GAME_WINNER = "MasterClient";
                        //var cc = PhotonNetwork.LocalPlayer.CustomProperties; //new lenecek 
                        //cc.Add("master" , 1);
                        //PhotonNetwork.LocalPlayer.SetCustomProperties(cc);
                    }
                    else 
                    {

                        //var cc = PhotonNetwork.LocalPlayer.CustomProperties;
                        //cc.Add("master", 2);
                        //PhotonNetwork.LocalPlayer.SetCustomProperties(cc);
                        //  ThisGameWinner.THIS_GAME_WINNER = "NoMasterClient";
                        //PlayerPrefs.SetString("winner", "NoMasterClient");
                    }
                    //  Invoke("LoadLevelForWinnerScene" , 1f);
                    // PhotonNetwork.LoadLevel("WinnerScene"); eski
                    SceneManager.LoadScene("WinnerScene"); // yeni
                    // PhotonNetwork.LoadLevel("WinnerScene");
                }
                else
                {

                    PhotonNetwork.LoadLevel("Wait");

                }

            }




            gameObject.GetComponent<Image>().fillAmount = i * 0.1f;
            yield return new WaitForSeconds(1);

        }

    }
    public void LoadLevelForWinnerScene()
    {
           PhotonNetwork.LoadLevel("WinnerScene");
    }


    List<int> GetRandomValuesZeroBetweenNine()
    {
        List<int> fromZeroToNine = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> result = new List<int>();
        for (int t = 0; t < 10; t++)
        {
            int chooseÝtem = fromZeroToNine[UnityEngine.Random.Range(0, fromZeroToNine.Count)];
            result.Add(chooseÝtem);
            fromZeroToNine.Remove(chooseÝtem);


        }
        return result;
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









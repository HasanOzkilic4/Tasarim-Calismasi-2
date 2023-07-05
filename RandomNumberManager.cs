using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using Firebase.Database;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class RandomNumberManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI randomNumber;
    static DatabaseReference reference;
    static FirebaseDatabase database;
    public TextMeshProUGUI master, noMaster;

    private void Awake()
    {
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
        reference = database.RootReference;
    }
     void Start()
    {
      

        GetNumber();
        Invoke("Deneme" ,2f);
    }
    public void GetNumber()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var hash = PhotonNetwork.MasterClient.CustomProperties;
            hash["number"] = UnityEngine.Random.Range(10, 100);
            PhotonNetwork.MasterClient.SetCustomProperties(hash);


        }
    }
    private void FixedUpdate()
    {

        SetCurrentScore();
    }

 
    
    public void SetCurrentScore()
    {
        int currentMasterClientScore = default, currentNoMasterClientScore = default;
        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.NickName == PhotonNetwork.MasterClient.NickName)
            {
                item.CustomProperties.TryGetValue("tag1", out object tag1);
                if (tag1 != null)
                {
                    var result = GameObject.FindWithTag((string)tag1);
                    currentMasterClientScore = result == null ? 0 : int.Parse(result.GetComponentInChildren<TextMeshProUGUI>().text);
                    master.text = currentMasterClientScore.ToString();
                }
               


            }
            else
            {
                item.CustomProperties.TryGetValue("tag1", out object tag1);
                if (tag1 !=null)
                {
                    var result = GameObject.FindWithTag((string)tag1);
                    currentNoMasterClientScore = result == null ? 0 : int.Parse(result.GetComponentInChildren<TextMeshProUGUI>().text);
                    noMaster.text = currentNoMasterClientScore.ToString();
                }
              

            }
        }
    }

    public void Deneme()
    {
        gameObject.GetComponent<PhotonView>().RPC("RandomNumberConfig", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void RandomNumberConfig()
    {


        PhotonNetwork.MasterClient.CustomProperties.TryGetValue("number", out var number);

        int sayi = (int)number;
        randomNumber.fontSize = 190f;
        randomNumber.text = sayi.ToString();

        PlayerPrefs.SetInt("randomNumber", int.Parse(randomNumber.text));


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AddTheGameHistory(TheGameHistory entity)
    {
        string userWithJson = JsonUtility.ToJson(entity);
        reference.Child("TheGameHistory").Child(entity.GameOwner).SetRawJsonValueAsync(userWithJson);

    }

    async Task<IResult> IsThereSuchATheGameHistoryInTheSystem(string gameOwner)
    {
        List<TheGameHistory> users = await GetAll();

        if (users.Any(u => u.GameOwner == gameOwner))
        {
            return new Result(true, "Böyle bir oyun ismi zaten mevcut");

        }

        return new Result(false);

    }



    public async Task<List<TheGameHistory>> GetAll(Func<TheGameHistory, bool> filter = null)
    {
        if (filter == null)
        {
            var users = await GetAllUsers();
            return users;
        }
        else
        {
            var users = await GetAllUsers();
            return users.Where(filter).ToList();

        }

        async Task<List<TheGameHistory>> GetAllUsers()
        {

            List<TheGameHistory> getAllTheGameHistory = new List<TheGameHistory>();
            var getTheGameHistory = await reference.Child("TheGameHistory").GetValueAsync();
           
            foreach (var user in getTheGameHistory.Children.ToList())
            {
                string gameOwner = String.Empty;


                TheGameHistory newUser = null;

                foreach (var item in user.Children.ToList())
                {


                    switch (item.Key)
                    {
                        case "GameOwner":
                            gameOwner = item.Value.ToString();
                            break;
                    }
                }
                newUser = new TheGameHistory(gameOwner)
                {
                    GameOwner = gameOwner
                };
                getAllTheGameHistory.Add(newUser);


            }

            return getAllTheGameHistory;
        }

    }

    public void UpdateTheGameHistory(string currentUsername, TheGameHistory entity) // sadece güncellenecek olan alanlarýn özellikleri de alýnabilir.
    {
        // daha efektif çözümler bulabilirsin

        Delete(currentUsername);
        AddTheGameHistory(entity);
  

    }


    public async   Task<TheGameHistory> Get(string gameOwner)
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

    public async void Delete(string userName)
    {
        await reference.Child("TheGameHistory").Child(userName).RemoveValueAsync();
    }







}



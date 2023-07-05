using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCardScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ResultsPanel;

    
    private void Awake()
    {
        ResultsPanel = GameObject.Find("ResultsPanel");
       
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void InitilazaForResults(int result)
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = result.ToString();
        gameObject.transform.SetParent(ResultsPanel.transform); // daha sonra güncellenebilir.
        gameObject.GetComponent<Button>().interactable = false;
        Player player = WhoSetResultBoxPlayer();


        player.CustomProperties.TryGetValue("tag1", out object left);
        player.CustomProperties.TryGetValue("tag2", out object right);


        string newLeft = (string)left;
        string newRight = (string)right;




        var cardLeft = GameObject.FindGameObjectWithTag(newLeft);
        var cardRight = GameObject.FindGameObjectWithTag(newRight);




        StartCoroutine(CardDestroy(cardRight, cardLeft, player));


        playingPlayer = PhotonNetwork.LocalPlayer;

       



    }



    IEnumerator CardDestroy(GameObject card, GameObject left, Player player)
    {
        yield return new WaitForSeconds(2f);
        left.GetComponentInChildren<TextMeshProUGUI>().text = GetComponentInChildren<TextMeshProUGUI>().text;
        var hash = player.CustomProperties;
        hash["leftNumber"] = int.Parse(left.GetComponentInChildren<TextMeshProUGUI>().text);
        player.SetCustomProperties(hash);

        card.SetActive(false);

        // PhotonNetwork.Destroy(gameObject);
        gameObject.SetActive(false);
    }



    Player playingPlayer = PhotonNetwork.LocalPlayer;
    public Player WhoSetResultBoxPlayer()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.PlayerList[i].NickName)
            {
                continue;
            }


            if (playingPlayer.GetScore() != PhotonNetwork.PlayerList[i].GetScore())
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



    /////////////////////



   
}

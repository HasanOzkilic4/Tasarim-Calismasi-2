using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreGlobalAyarlamaForMaster : MonoBehaviour
{
    
    void Start()
    {
        gameObject.GetComponent<PhotonView>().RPC("ScoreCONFİG", RpcTarget.AllBufferedViaServer);
    }



    [PunRPC]
    public void ScoreCONFİG()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("mCScore").ToString();
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreGlobalAyarlama : MonoBehaviour
{
  
    void Start()
    {
        gameObject.GetComponent<PhotonView>().RPC("ScoreCONF�G", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void ScoreCONF�G()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("nMCScore").ToString();
    }
}

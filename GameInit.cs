using Assets.Scripts.Entities;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameInit : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI counter;
    private void Awake()
    {
        ThisGameWinner.COUNTER = 5 ;
    //        gameObject.SetActive(true);
        
    }

    public GameObject Time;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.GetComponent<PhotonView>().RPC("CounterManager", RpcTarget.AllBufferedViaServer);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public IEnumerator CounterManager()
    {
        while (ThisGameWinner.COUNTER != 0)
        {
            yield return new WaitForSeconds(1);
            ThisGameWinner.COUNTER--;
            counter.text = ThisGameWinner.COUNTER.ToString();
        }
        gameObject.SetActive(false);
        Time.GetComponent<PhotonView>().RPC("ClockStart", RpcTarget.AllBufferedViaServer, true);

        StopCoroutine("CounterManager");

    }
}

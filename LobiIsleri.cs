using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobiIsleri : MonoBehaviourPunCallbacks
{
   

    public TMP_InputField odaIsmi, odaSayisi;

    public void CreatedCustomRoom()
    {
        PhotonScript.Instance.ServerLogin(odaIsmi.text,  true , byte.Parse(odaSayisi.text));
 
    }
  

    public void GoToUserPage()
    {
        SceneManager.LoadScene("UserPage");
    }



  
}

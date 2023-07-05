using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginCustomRoom : MonoBehaviourPunCallbacks
{

    public TMP_InputField roomName;
    public void LoginRoom()
    {
        PhotonScript.Instance.ServerLogin(roomName.text ,true);
    }


}

using Assets.Abstract;
using Assets.API.UserController;
using Assets.Concrete;
using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserRegisteryManager : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField password;
    public GameObject existUsernamePanel;

    

    IUserService _userService;
    void Start()
    {
        _userService = new UsersManager(new DatabaseUserDal());
      
    }
   


    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLoginPage()
    {
        SceneManager.LoadScene(0);
    }
    public async void Add()
    {
        IResult result = await _userService.AddUser(new User(userName.text.Trim()) { Password = password.text });
        if (!result.IsSuccess)
        {
            StartCoroutine(ExistUsernamePanelManager(result.Message));

        }
        else
        {
            PlayerPrefs.SetString("Username" , userName.text.Trim());
            SceneManager.LoadScene("UserPage");
        }
    }

    IEnumerator ExistUsernamePanelManager(string message)
    {
        existUsernamePanel.SetActive(true);
        existUsernamePanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(2);
        existUsernamePanel.SetActive(false);
    }
}

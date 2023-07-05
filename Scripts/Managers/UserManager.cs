using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Assets.Abstract;
using Assets.Concrete;
using Assets.Scripts.Entities;
using Firebase.Database;
using Assets.Scripts.Results;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using Assets.API.UserController;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public class UserManager : MonoBehaviour
{


    public TMP_InputField userName;
    public TMP_InputField password;
    public GameObject existUsernamePanel;

    IUserService _userService;


    private void Awake()
    {

    }
    void Start()
    {
        _userService = new UsersManager(new DatabaseUserDal());
        // ControlllerMethodForFirabaseSDK();
    }
    void Update()
    {


    }

    public void GoToUserRegisteryScene()
    {
       
        SceneManager.LoadScene("UserRegistery");
    }

    public async void LogIn()
    {
        var result = await _userService.LogIn(userName.text , password.text);
        if (result.IsSuccess)
        {
            PlayerPrefs.SetString("Username" , result.Data.Name);
            SceneManager.LoadScene("UserPage");
        }
        else
        {
            existUsernamePanel.SetActive(true);
            existUsernamePanel.GetComponentInChildren<TextMeshProUGUI>().text = result.Message;
        }
    }





  



    void ControlllerMethodForFirabaseSDK()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = FirebaseApp.DefaultInstance;


            }
            else
            {
                Debug.LogError(String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }



}

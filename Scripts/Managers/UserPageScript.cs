using Assets.Abstract;
using Assets.API.UserController;
using Assets.Concrete;
using Assets.Scripts.Entities;
using Assets.Scripts.Results;
using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class UserPageScript : MonoBehaviour
{
    public Animator settingPanelAnimator;
    IUserService _userService;
    string username;
    User user;
  
    public TextMeshProUGUI usernameText;

    public TextMeshProUGUI birinci;
    public TextMeshProUGUI ikinci;
    public TextMeshProUGUI ucuncu;
    public TextMeshProUGUI dorduncu;
    public TextMeshProUGUI besinci;
    List<TextMeshProUGUI> rankeds;

    FirebaseDatabase database;
    DatabaseReference reference;

    public void DeleteTheGameHistory()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Delete(PhotonNetwork.MasterClient.NickName);
        }

    }
    public async void Delete(string userName)
    {
        if (reference.Child("TheGameHistory").Child(username) != null)
        {
            await reference.Child("TheGameHistory").Child(userName).RemoveValueAsync();
        }
        
    }
    private void Awake()
    {
        database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");
        reference = database.RootReference;
        _userService = new UsersManager(new DatabaseUserDal());
      
        username = PlayerPrefs.GetString("Username");
        rankeds = new List<TextMeshProUGUI>() { birinci, ikinci, ucuncu, dorduncu, besinci };
        
     

    }
    async void Start()
    {
        DeleteTheGameHistory();
        PhotonNetwork.Disconnect();
        

        UserSet(username);
        KupaScoreManager();
        var a = await _userService.RankedArrangement();
        var b = a.Data;
        for (int i = 0; i < rankeds.Count; i++)
        {
            if (b.Count == i)
            {
                break;
            }
            rankeds[i].text = b[i].Name + "   " + b[i].Score;
        }



    }



    public async void UserSet(string userName) //ok
    {
        var result = await _userService.Get(userName);
        user = result.Data;
        usernameText.text = user.Name;
    }


    public void UserSettingsTrigger() //ok
    {
        settingPanelAnimator.SetTrigger("SettingsPanel");

    }

    public async void Delete() //ok
    {
        var result = await _userService.Delete(user.Name);
        if (result.IsSuccess)
        {
            SceneManager.LoadScene("GameMenu");
        }
        else
        {
            //
        }

    }

    public TMP_InputField input;
    public GameObject paswwordPanel;
    public void UserPasswordReset() //ok
    {
       paswwordPanel.SetActive(true);
    }

    public GameObject passwordPanelSuccessfully;
    public async void UserUpdateForPassword() //ok
    {
        var person = await _userService.Get(username);
        long score = person.Data.Score;
        var result = await _userService.Update(user.Name, new User(person.Data.Name) { Password = input.text , Score = score });
        if (!result.IsSuccess)
        {
            Debug.Log("Tekrar dene");
        }
        else
        {
            UserSet(user.Name);

            StartCoroutine("PasswordPanelSuccessfully");
        }
        paswwordPanel.SetActive(false);
    }

    IEnumerator PasswordPanelSuccessfully() //ok
    {
        passwordPanelSuccessfully.SetActive(true);
        yield return new WaitForSeconds(2);
        passwordPanelSuccessfully.SetActive(false);

    }

    public GameObject userNameChangedPanelOpen;
    public void UserNameChangedPanelOpen()
    {
        userNameChangedPanelOpen.SetActive(true);
    }

    public TMP_InputField inputForUserName;
    async Task<IResult> IsThereSuchAUsernameInTheSystem(string userName)
    {
        IDataResult<List<User>> users = await GetAll();

        if (users.Data.Any(u => u.Name == userName))
        {
            return new Result(true, "Böyle bir kullanýcý ismi zaten mevcut");

        }

        return new Result(false);

    }
    IUserDal _userDal;
    public async Task<IDataResult<List<User>>> GetAll()
    {
        _userDal = new DatabaseUserDal();
        List<User> users = await _userDal.GetAll();
        return new DataResult<List<User>>(true, users);
    }
    public GameObject userNameExist;
    public async void UserUpdateForUsername()
    {
     var isThereSuchAUsernameInTheSystem =  await IsThereSuchAUsernameInTheSystem(inputForUserName.text.Trim());
        if (!isThereSuchAUsernameInTheSystem.IsSuccess)
        {
            var person = await _userService.Get(username);
            long score = person.Data.Score;
            var result = await _userService.Update(user.Name, new User(inputForUserName.text.Trim()) { Password = person.Data.Password, Score = score });
            if (!result.IsSuccess)
            {
                Debug.Log("Tekrar dene");
            }
            else
            {
                PlayerPrefs.SetString("Username" , inputForUserName.text.Trim());
                username = PlayerPrefs.GetString("Username");
                UserSet(inputForUserName.text.Trim());
                userNameChangedPanelOpen.SetActive(false);
                StartCoroutine("PasswordPanelSuccessfullyForUserName");

                #region Liderlik Tablosu Güncellemesi
                var a = await _userService.RankedArrangement();
                var b = a.Data;
                for (int i = 0; i < rankeds.Count; i++)
                {
                    if (b.Count == i)
                    {
                        break;
                    }
                    rankeds[i].text = b[i].Name + "   " + b[i].Score;
                }
                #endregion
            }
        }
        else
        {
            StartCoroutine(UserNameExist(isThereSuchAUsernameInTheSystem.Message));
        }
       
    }
    IEnumerator UserNameExist(string message)
    {
        userNameExist.SetActive(true);
        userNameExist.GetComponent<TextMeshProUGUI>().text = message;

        
        yield return new WaitForSeconds(4);
        userNameExist.SetActive(false);
    }

    public GameObject uNSP; //userNameSuc.....P...;
    IEnumerator PasswordPanelSuccessfullyForUserName() //ok
    {
        uNSP.SetActive(true);
        yield return new WaitForSeconds(2);
        uNSP.SetActive(false);

    }


    public void GoToGameScene() //ok
    {
        PhotonScript.Instance.ServerLogin();



    }

    public void GoToLobiKurma()
    {
       SceneManager.LoadScene("LobiIsleri");
      
      
    }


    public void  GameExit()
    {

        
        Application.Quit();

    }



    public TextMeshProUGUI kupaScore;
    public async void KupaScoreManager()
    {
    IDataResult<User> user =  await  _userService.Get(username);
        var score = user.Data.Score;
        kupaScore.text = score.ToString();  
    }
}



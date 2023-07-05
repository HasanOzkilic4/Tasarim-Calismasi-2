using Assets.Abstract;
using Assets.Scripts.Entities;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace Assets.Concrete
{
    public class DatabaseUserDal : IUserDal
    {


        FirebaseDatabase database;
        DatabaseReference reference;
        public DatabaseUserDal()
        {
            database = FirebaseDatabase.GetInstance("https://test2-bfd1b-default-rtdb.firebaseio.com/");

            reference = database.RootReference;
        }



        public async void Add(User entity)
        {
            string userWithJson = JsonUtility.ToJson(entity);
            await reference.Child("Users").Child(entity.Name).SetRawJsonValueAsync(userWithJson);

        }

        public async void Delete(string userName)
        {
            await reference.Child("Users").Child(userName).RemoveValueAsync();
        }

        public async Task<User> Get(string userName)
        {
            string name = String.Empty;
            string password = "";
           
            long score = 10;
            User user = null;

            var result = await reference.Child("Users").Child(userName).GetValueAsync();
            foreach (var item in result.Children.ToList())
            {
                switch (item.Key)
                {
                    case "Name":
                        name = item.Value.ToString();
                        break;

                    case "Score":
                        score = long.Parse(item.Value.ToString());
                        break;

                   

                    case "Password":
                        password = item.Value.ToString();
                        break;

                    default:

                        break;
                }
            }
            user = new User(name)
            {
               
                Password = password,
                Score = score,
                Name = name
            };
            return user;
        }

        public async Task<List<User>> GetAll(Func<User, bool> filter = null)
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

            async Task<List<User>> GetAllUsers()
            {

                List<User> getAllUser = new List<User>();
                var getUsers = await reference.Child("Users").GetValueAsync();
                foreach (var user in getUsers.Children.ToList())
                {
                    string name = String.Empty;
                    string password = "";
                  
                    long score = 10;


                    User newUser = null;

                    foreach (var item in user.Children.ToList())
                    {


                        switch (item.Key)
                        {
                            case "Name":
                                name = item.Value.ToString();
                                break;


                            case "Password":
                                password = item.Value.ToString();
                                break;


                            case "Score":
                                score = (long)item.Value;
                                break;


                           


                            default:
                                break;
                        }
                    }
                    newUser = new User(name)
                    {
                      
                        Name = name,
                        Password = password,
                        Score = score
                    };
                    getAllUser.Add(newUser);


                }

                return getAllUser;
            }

        }

        public void Update(string currentUsername, User entity) // sadece güncellenecek olan alanların özellikleri de alınabilir.
        {
            // daha efektif çözümler bulabilirsin
            Delete(currentUsername);
            Add(entity);

        }
    }
}

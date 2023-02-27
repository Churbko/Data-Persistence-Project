using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
#if UNITY_EDITOR//ce codage n'est utilisé que lorsqu'il concerne l'éditeur
using UnityEditor;
#endif
public class UIMenuManager : MonoBehaviour
{
    //This code enables you to access the MainManager object from any other script. 
    //public static UIMenuHandler Instance;
   // private string userName;
    //public int score;
    public TMP_InputField userNameInput;//assign

    private void Awake()
    {
       
    }
    private void Start()
    {
       
        LoadScores();
    }
    public void StartGame()
    {
        //save user in static data
        GetUserName();

        //score is set to 0 
        SharedData.bestScore = 0;

        SceneManager.LoadScene("main");
    }

    public void GetUserName()
    {
        //Debug.Log("getuserName=" + userNameInput.text);
        if (userNameInput.text != "")
        {
            SharedData.userName = userNameInput.text;
        }
        else
            SharedData.userName = "Unknown";

    }

    public void HighScores()
    {
        SceneManager.LoadScene("High Scores");
    }

    public void Exit()
    {
#if (UNITY_EDITOR)

        EditorApplication.ExitPlaymode();
#else
            
            Application.Quit(); // original code to quit Unity player
#endif
    }
   
    [System.Serializable]
    class SaveData
    {
        //données à passer entre scènes

        public int highScore1Score;
        public string highScore1Name;
        public int highScore2Score;
        public string highScore2Name;
        public int highScore3Score;
        public string highScore3Name;
        
    } 
    
    public void LoadScores()
    {
        SortedDictionary<string, int> highScores = new SortedDictionary<string, int>();
        string path = Application.persistentDataPath + "/savefile.json";
        // Debug.Log("json file = "+path);

        if (File.Exists(path))
        {

            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            SharedData.highScore1Score = data.highScore1Score;
            SharedData.highScore1Name = data.highScore1Name;
            SharedData.highScore2Score = data.highScore2Score;
            SharedData.highScore2Name = data.highScore2Name;
            SharedData.highScore3Score = data.highScore3Score;
            SharedData.highScore3Name = data.highScore3Name;


            highScores.Add(data.highScore1Name, data.highScore1Score);
            highScores.Add(data.highScore2Name, data.highScore2Score);
            highScores.Add(data.highScore3Name, data.highScore3Score);


           /* Debug.Log("load!" + SharedData.highScore1Score + " " + SharedData.highScore1Name + " " +
                SharedData.highScore2Score + " " + SharedData.highScore2Name + " " +
                SharedData.highScore3Score + " " + SharedData.highScore3Name);*/
        }
        else
        {
            Debug.Log("file doesn't exist");

            SharedData.highScore1Name = "Nobody1";
            SharedData.highScore2Name = "Nobody2";
            SharedData.highScore3Name = "Nobody3";
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainManager : MonoBehaviour
{

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public Text bestScoreText;
    public TextMeshProUGUI userNameText;
    public string userName;
    public int score;
    public int limitHighScores = 3;//we're keeping the 3 highest scores
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private SortedDictionary< string,int> highScores;


    private bool m_GameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        //create a dictionary for the high score panel and load scores into it
        //easy sorting with dictionaries
        //but unique key which means the same player can't have two lines in the high scores panel
        highScores = new SortedDictionary<string, int>();
        LoadScores();


        bestScoreText.text = "Best score " + SharedData.userName + " : " + Mathf.Max(SharedData.bestScore, score);

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        bestScoreText.text = "Best score " + SharedData.userName + " : " + Mathf.Max(SharedData.bestScore, m_Points);
        SharedData.bestScore = Mathf.Max(SharedData.bestScore, m_Points);
        SaveScores();

        m_GameOver = true;
        GameOverText.SetActive(true);

    }

    [System.Serializable]
    class SaveData
    {
        //if more scores to be kept, use a list 
        public string userName;
        public int bestScore;
        public int highScore1Score;
        public string highScore1Name;
        public int highScore2Score;
        public string highScore2Name;
        public int highScore3Score;
        public string highScore3Name;

    }
    public void SaveScores()
    {

        SaveData data = new SaveData();

        data.userName = SharedData.userName;
        // if highscore already has username, keep the highest score
        int valueInTab=0;
        if (highScores.ContainsKey(data.userName))
        {
            valueInTab = highScores[data.userName];
            highScores.Remove(data.userName);
        }
        highScores.Add(data.userName, Mathf.Max(valueInTab,SharedData.bestScore));

        //sort the highscore dictionary in descending
        var sortedDict = from entry in highScores orderby entry.Value descending select entry;

        /*foreach (KeyValuePair<string, int> user in sortedDict)
        {
            Debug.Log("sorteddict= "+user.Key + " " + user.Value);
        }*/



        //on charge les 3 premiers
        data.highScore1Score = sortedDict.ElementAt(0).Value;
        data.highScore1Name = sortedDict.ElementAt(0).Key;
        data.highScore2Score = sortedDict.ElementAt(1).Value;
        data.highScore2Name = sortedDict.ElementAt(1).Key;
        data.highScore3Score = sortedDict.ElementAt(2).Value;
        data.highScore3Name = sortedDict.ElementAt(2).Key;


        //also store in shared data

        SharedData.highScore1Score = data.highScore1Score;
        SharedData.highScore1Name = data.highScore1Name;
        SharedData.highScore2Score = data.highScore2Score;
        SharedData.highScore2Name = data.highScore2Name;
        SharedData.highScore3Score = data.highScore3Score;
        SharedData.highScore3Name = data.highScore3Name;

      /*  Debug.Log("save!" + SharedData.highScore1Score + " " + SharedData.highScore1Name + " " +
    SharedData.highScore2Score + " " + SharedData.highScore2Name + " " +
    SharedData.highScore3Score + " " + SharedData.highScore3Name);*/
        //on charge les infos dans JSON

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScores()
    {


            highScores.Add(SharedData.highScore1Name, SharedData.highScore1Score);
            highScores.Add(SharedData.highScore2Name, SharedData.highScore2Score);
            highScores.Add(SharedData.highScore3Name, SharedData.highScore3Score);


         /*Debug.Log("load in dictionary " + SharedData.highScore1Score + " " + SharedData.highScore1Name + " " +
            SharedData.highScore2Score + " " + SharedData.highScore2Name + " " +
        SharedData.highScore3Score + " " + SharedData.highScore3Name );*/

    }

  

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }



}

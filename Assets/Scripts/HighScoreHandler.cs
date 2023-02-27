using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class HighScoreHandler : MonoBehaviour
{
    public TextMeshProUGUI userName1, userName2, userName3;
    public TextMeshProUGUI userScore1, userScore2, userScore3;
    // Start is called before the first frame update
    void Start()
    {

        userName1.text = SharedData.highScore1Name;
        userName2.text= SharedData.highScore2Name;
        userName3.text = SharedData.highScore3Name;

        userScore1.text= SharedData.highScore1Score.ToString();
        userScore2.text = SharedData.highScore2Score.ToString();
        userScore3.text = SharedData.highScore3Score.ToString();
    }



    public void GetBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    


}

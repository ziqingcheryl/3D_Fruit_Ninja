using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class prescriptionData {

    public string mainEye;
    public int verticalMin;
    public int verticalMax;
    public int horizontalMin;
    public int horizontalMax;
    public int objectMin;
    public int objectMax;
    public int blurMax;
    public int blurMin;
    public int vividMax;
    public int vividMin;

    public void print () {
        Debug.Log (blurMax);
    }
}

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject scoreText;
    public GameObject bestScoreText;
    public GameObject timeText;
    public GameObject finishText;
    public GameObject spawner;
    public GameObject board;
    public GameObject blurPanel;
    public Image fadeImageRight;
    public Image fadeImageLeft;
    public bool gameStart;

    private int score;
    private float time;
    private int fruitNum;
    private float scorePercent;
    private string[] arguments;
    private int round;
    private float gameTime;

    public string token = "";
    public prescriptionData preData;

    AudioSource timeupSound;

    //initial run function
    void Awake () {
        instance = this;
        arguments = Environment.GetCommandLineArgs ();
        //Get Token

        token = arguments[1].ToString();
        gameTime = 60.0f;

    }

    void Start () {

        gameStart = false;
        round = 1; //round 1
        score = 0;
        time = gameTime;
        timeupSound = GetComponent<AudioSource> ();

    }

    void Update () {
        if (gameStart == true && round == 1) {
            scoreText.GetComponent<TextMesh> ().text = score.ToString ();
            bestScoreText.GetComponent<TextMesh> ().text = "BEST: " + GetBestScore ();
            timeText.GetComponent<TextMesh> ().text = time.ToString ("F1") + "seconds";
            time -= Time.deltaTime;

            //End of 1st round

            if (time < 0 && round == 1) {
                round = 2;
                gameStart = false;
                time = 0;
                fruitNum = spawner.GetComponent<FruitNinja> ().fruitNum;
                StartCoroutine ("Round2");
            }
        } else if (gameStart == true && round == 2) {
            scoreText.GetComponent<TextMesh> ().text = score.ToString ();
            bestScoreText.GetComponent<TextMesh> ().text = "BEST: " + GetBestScore ();
            timeText.GetComponent<TextMesh> ().text = time.ToString ("F1") + "seconds";
            time -= Time.deltaTime;

            //End of 2nd round
            if (time < 0 && round == 2) {
                gameStart = false;
                time = 0;
                fruitNum = spawner.GetComponent<FruitNinja> ().fruitNum;
            }

        }

    }

    void UpdateBestScore () {
        if (GetBestScore () < score)
            PlayerPrefs.SetInt ("BestScore", score);
    }

    int GetBestScore () {

        int bestScore = PlayerPrefs.GetInt ("BestScore");

        return bestScore;

    }

    public void GetScore () {

        score++;

    }
    
    IEnumerator Round2 () {

        board.SetActive (false);

        timeupSound.Play ();

        finishText.GetComponent<TextMesh> ().text = "Times Up!";

        yield return new WaitForSeconds (2f);

        finishText.GetComponent<TextMesh> ().text = "After a short break, round 2 begins.";

        yield return new WaitForSeconds (2f);

        Color startColorRight = fadeImageRight.color;

        Color startColorLeft = fadeImageLeft.color;

        //fade out
        startColorRight.a = 1.0f;
        fadeImageRight.color = startColorRight;
        startColorLeft.a = 1.0f;
        fadeImageLeft.color = startColorRight;

        yield return new WaitForSeconds (5f); //5 second break

        //fade in
        startColorRight.a = 0;
        fadeImageRight.color = startColorRight;
        startColorLeft.a = 0;
        fadeImageLeft.color = startColorRight;

        yield return new WaitForSeconds (1f);
        finishText.GetComponent<TextMesh> ().text = "40 seconds";
        yield return new WaitForSeconds (2f);
        timeupSound.Play ();

        finishText.GetComponent<TextMesh> ().text = "Round 2";
        yield return new WaitForSeconds (1f);
        finishText.GetComponent<TextMesh> ().text = "";

        //starting Game

        board.SetActive (true);
        round = 2;
        time = gameTime;
        gameStart = true;
    }

    IEnumerator Finish () {

        board.SetActive (false);

        timeupSound.Play ();

        finishText.GetComponent<TextMesh> ().text = "Times Up!";

        Debug.Log (score + " " + fruitNum);

        yield return new WaitForSeconds (2f);

        finishText.GetComponent<TextMesh> ().text = (((float) score / fruitNum) * 100).ToString ("F1") + "% You have achieved!";

        yield return new WaitForSeconds (4f);

        UpdateBestScore ();

        SceneManager.LoadScene (0); //game restart

    }

}
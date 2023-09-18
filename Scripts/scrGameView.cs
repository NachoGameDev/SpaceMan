using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrGameView : MonoBehaviour
{
    public Text scoreText, coinsText, recordText;
    private scrPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<scrPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.singleton.currentGameState == GameState.INGAME){
            int coins = GameManager.singleton.collectedObjects;
            float score = player.GetTraveledDistance();
            float record = PlayerPrefs.GetFloat("maxscore",0f);

            coinsText.text = ": " + coins.ToString();
            scoreText.text = "Points: " + score.ToString("f1");
            recordText.text = "Record: " + record.ToString("f1");
        }
    }
}

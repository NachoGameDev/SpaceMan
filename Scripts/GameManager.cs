using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Variables fijas para los estados del juego
public enum GameState{
    MENU,
    INGAME,
    GAMEOVER
}

public class GameManager : MonoBehaviour
{
    //Por default, el estado del juego es el menú principal
    public GameState currentGameState = GameState.MENU;

    //Singleton. Se asegura de que solo va a haber un GameManager en todo el juego
    public static GameManager singleton;

    private scrPlayer player;

    public int collectedObjects = 0;

    //Al iniciar el juego, asigna a esta instancia como el singleton
    void Awake()
    {
        if(singleton == null){
            singleton = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGameState(GameState.MENU);
        //Identifica al player en la escena
        player = GameObject.FindWithTag("Player").GetComponent<scrPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //"Submit" se configura en Unity > Edit > Project Settings > Input Manager
        if(currentGameState != GameState.INGAME && Input.GetButtonDown("Submit")){
            StartGame();
        }
    }

    public void StartGame(){
        SetGameState(GameState.INGAME);
        player.Respawn();
    }

    public void GameOver(){
        SetGameState(GameState.GAMEOVER);
    }

    public void BackToMenu(){
        SetGameState(GameState.MENU);
    }

    //Cambia el game state
    private void SetGameState(GameState newGameState){
        switch(newGameState){
            case GameState.MENU:
                MenuManager.singleton.HideGameMenu();
                MenuManager.singleton.HideGameOverMenu();
                MenuManager.singleton.ShowMainMenu();
                break;
            case GameState.INGAME:
                LevelManager.singleton.RemoveAllLevelBlocks();
                LevelManager.singleton.GenerateInitialBlocks();
                player.Respawn();
                MenuManager.singleton.HideMainMenu();
                MenuManager.singleton.HideGameMenu();
                MenuManager.singleton.ShowGameMenu();
                break;
            case GameState.GAMEOVER:
                MenuManager.singleton.HideGameMenu();
                MenuManager.singleton.HideMainMenu();
                MenuManager.singleton.ShowGameOverMenu();
                break;  
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject (scrCollectable collectable){
        collectedObjects += collectable.value;
    }
}
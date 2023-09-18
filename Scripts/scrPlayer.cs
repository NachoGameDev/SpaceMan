using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPlayer : MonoBehaviour
{
    //Singleton
    public scrPlayer singleton;

    //Variables
    public float jumpForce = 8f;
    public float runningSpeed = 2f;
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private Vector3 startPosition;
    //Con esta variable, en Unity le paso al script la capa que le asigné al suelo: Ground
    public LayerMask groundMask;
    //Punteros a los booleanos del animator en Unity
    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    [SerializeField]
    private int healthPoints, manaPoints;
    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30,
        MIN_HEALTH = 10, MIN_MANA = 0, SUPER_JUMP_COST = 5;
    public const float SUPER_JUMP_FORCE = 1.5f;

    public float jumpRaycastDistance = 1.5f;

    private void Awake() {
        //Asigna a esta instancia el singleton
        if(singleton == null){
            singleton = this;
        }
        //Hace que playerRigidbody apunte al Rigidbody que se agregó al player en Unity
        playerRigidbody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start() {
        //Guarda la posición para el respawn
        startPosition = this.transform.position;
    }

    public void Respawn(){
        animator.SetBool(STATE_ALIVE,true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition",0.1f);
    }

    private void RestartPosition(){
        this.transform.position = startPosition;
        this.playerRigidbody.velocity = Vector2.zero;
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<scrCameraFollow>().ResetCameraPosition();
    }

    //Hace que el personaje se mueva automáticamente a la derecha
    //Fixed update usa el reloj interno del procesador en lugar de los frames, para que
    //el movimiento no se lagee
    private void FixedUpdate() {
        if(GameManager.singleton.currentGameState == GameState.INGAME){
            if(playerRigidbody.velocity.x < runningSpeed){
                //Agrega a la velocidad actual del rigidbody lo que hace falta para llegar al deseado,
                //preservando la velocidad en Y
                playerRigidbody.velocity = new Vector2(runningSpeed, //X
                                                        playerRigidbody.velocity.y); //Y
            }
        }
        else{//Si no es INGAME, para al personaje
            playerRigidbody.velocity = new Vector2(0, //X
                                                    playerRigidbody.velocity.y); //Y
        }
    }

    private void Update() {
        //"Jump" se configura en Unity > Edit > Project Settings > Input Manager
        if(Input.GetButtonDown("Jump")){
            Jump(false);
        }
        if(Input.GetButtonDown("SuperJump")){
            Jump(true);
        }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        //Este permite ver un raycast en el editor al ejecutar
        Debug.DrawRay(this.transform.position, Vector2.down * jumpRaycastDistance, Color.red);
    }

    void Jump(bool superJump){
        float jumpForceFactor = jumpForce;
        if(superJump && manaPoints >= SUPER_JUMP_COST){
            manaPoints -= SUPER_JUMP_COST;
            jumpForceFactor *= SUPER_JUMP_FORCE;
        }

        //Hay que revisar si está tocando el piso para no poder spammear el salto
        if(IsTouchingTheGround() && GameManager.singleton.currentGameState == GameState.INGAME){
            playerRigidbody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();
        }
    }

    //Revisa si el personaje está tocando el piso
    bool IsTouchingTheGround(){
        //Con el raycast envío un rayo invisible del player al piso
        //Raycast(Origen, dirección, largo del rayo, capa con la que debe tocar para activarse); 
        if(Physics2D.Raycast(this.transform.position, Vector2.down, jumpRaycastDistance, groundMask)){
            return true;
        }   
        return false;
    }

    //Método para morir
    public void Die(){
        float traveledDistance = GetTraveledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore",0f);
        if(traveledDistance > previousMaxDistance){
            PlayerPrefs.SetFloat("maxscore",traveledDistance);
        }
        //Activa la animación de morir
        this.animator.SetBool(STATE_ALIVE, false);
        //Cambia el estado del juego a game over
        GameManager.singleton.GameOver();
    }

    public void CollectHealth(int points){
        this.healthPoints += points;
        if(this.healthPoints > MAX_HEALTH){
            this.healthPoints = MAX_HEALTH;
        }
        if(this.healthPoints <= 0){
            Die();
        }
    }

    public void CollectMana(int points){
        this.manaPoints += points;
        if(this.manaPoints > MAX_MANA){
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth(){
        return healthPoints;
    }

    public int GetMana(){
        return manaPoints;
    }

    public float GetTraveledDistance(){
        return this.transform.position.x - startPosition.x;
    }
}
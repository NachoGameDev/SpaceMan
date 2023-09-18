using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrEnemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;
    Rigidbody2D rigidBody;
    public bool isFacingRight = false;
    private Vector3 startPosition;
    public int enemyDamage = 1;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if(isFacingRight){
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0,180,0);
        }
        else{
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if(GameManager.singleton.currentGameState == GameState.INGAME){
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag){
            case "Player":
                other.GetComponent<scrPlayer>().CollectHealth(-enemyDamage);
                break;
            case "Rock":
            case "Enemy":
                isFacingRight = !isFacingRight;
                break;
        }
    }
}

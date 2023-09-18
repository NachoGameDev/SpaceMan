using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType{
    HEALTH_POTION,
    MANA_POTION,
    MONEY
}

public class scrCollectable : MonoBehaviour
{
    public CollectableType type = CollectableType.MONEY;
    SpriteRenderer sprite;
    CircleCollider2D itemCollider;
    bool hasBeenCollected = false;
    public int value = 1;

    private GameObject player;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Show(){
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide(){
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect(){
        Hide();
        hasBeenCollected = true;

        switch(this.type){
            case CollectableType.MONEY:
                GameManager.singleton.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;
            case CollectableType.HEALTH_POTION:
                player.GetComponent<scrPlayer>().CollectHealth(this.value);
                break;
            case CollectableType.MANA_POTION:
                player.GetComponent<scrPlayer>().CollectMana(this.value);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            if(!hasBeenCollected){
                Collect();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType{
    HEALTH_BAR,
    MANA_BAR
}

public class scrPlayerBar : MonoBehaviour
{
    private Slider slider;
    public BarType type;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        switch(type){
            case BarType.HEALTH_BAR:
                slider.maxValue = scrPlayer.MAX_HEALTH;
                break;
            case BarType.MANA_BAR:
                slider.maxValue = scrPlayer.MAX_MANA;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(type){
            case BarType.HEALTH_BAR:
                slider.value = GameObject.FindWithTag("Player").
                    GetComponent<scrPlayer>().GetHealth();
                break;
            case BarType.MANA_BAR:
                slider.value = GameObject.FindWithTag("Player").
                    GetComponent<scrPlayer>().GetMana();
                break;
        }
    }
}

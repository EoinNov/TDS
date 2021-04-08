using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI : MonoBehaviour
{
    Zombie zomb;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
        zomb = FindObjectOfType<Zombie>();
        slider.maxValue = zomb.zombieHeals;
        slider.value = zomb.zombieHeals;
        zomb.onCheckHeals += updateHeals;

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
     
    }
    void updateHeals()
    {
        slider.value = zomb.zombieHeals;
    }
}

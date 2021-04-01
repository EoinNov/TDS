using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI : MonoBehaviour
{
    Zombie zomb;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        zomb = GetComponentInParent<Zombie>();
       
        zomb.onCheckHeals = UpdateHealth;
  

        slider.maxValue = zomb.zombieHealth;
        slider.value = zomb.zombieHealth;
    }



    private void UpdateHealth()
    {
        slider.value = zomb.zombieHealth;
        print("Обновление слайдера через События");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        print("Обновление слайдера через LateUpdate");
        transform.rotation = Quaternion.identity;
    }
}

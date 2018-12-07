using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUIContainer : MonoBehaviour {

    int currIndex = 2;
    public Image[] imageArray;

    //Heart Sprite vars
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void AddHealth() {
        //Max Health, do nothing
        if (currIndex == 2) {
            imageArray[currIndex].sprite = fullHeart;
            return;
        }

        imageArray[currIndex].sprite = fullHeart;
        currIndex++;
    }

    public void ReduceHealth(int damage) {
        while(damage > 0) {
            //Min Health, do nothing
            if (currIndex == 0) {
                imageArray[currIndex].sprite = emptyHeart;
                return;
            }

            imageArray[currIndex].sprite = emptyHeart;
            currIndex--;
            damage--;
        }
    }

    public void ResetHealth() {    
        for(int i=0; i<3; i++) {
            imageArray[i].sprite = fullHeart;
        }
        currIndex = 2;
    }
}

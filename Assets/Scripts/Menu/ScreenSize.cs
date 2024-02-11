using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSize : MonoBehaviour
{
    public Sprite SpriteFull, SpriteNoFull;
    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen; 
        if (Screen.fullScreen)
            GameObject.Find("ScreenSize").GetComponent<Image>().sprite = SpriteFull;
        else
            GameObject.Find("ScreenSize").GetComponent<Image>().sprite = SpriteNoFull;
    }
}

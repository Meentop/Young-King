using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectCell : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text number;

    public void SetImageAndNumber(Sprite image, int number)
    {
        this.image.sprite = image;
        this.number.text = number.ToString();
    }
}

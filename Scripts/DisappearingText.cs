using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisappearingText : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void ShowText(float visibilityTime, float disappearanceSpeed)
    {
        gameObject.SetActive(true);
        StartCoroutine(CorShowText(visibilityTime, disappearanceSpeed));
    }

    IEnumerator CorShowText(float visibilityTime, float disappearanceSpeed)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        yield return new WaitForSeconds(visibilityTime);
        while(text.color.a > 0)
        {
            yield return new WaitForEndOfFrame();
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.01f * disappearanceSpeed);
        }
        gameObject.SetActive(false);
    }
}

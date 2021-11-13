using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    [SerializeField] List<string> descritions;

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void ShowDescripton(int i)
    {
        text.text = descritions[i];
    }
}

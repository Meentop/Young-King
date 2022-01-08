using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISpell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Text descriptionText;
    
    public Image image;
    public string description;
    public Text timer, costText;
    bool mouseOver = false;

    Main main;

    private void Awake()
    {
        timer = transform.GetChild(1).GetComponent<Text>();
        image = GetComponent<Image>();
        costText = transform.GetChild(2).GetComponent<Text>();

        main = Main.Instance;
    }

    private void Update()
    {
        if (mouseOver)
            descriptionText.transform.position = transform.position;
    }

    public void CastSpell(int spellNumber)
    {
        if (spellNumber == 0)
            main.GetActiveHero().StartQSpell();
        else if (spellNumber == 1)
            main.GetActiveHero().StartWSpell();
        else if (spellNumber == 2)
            main.GetActiveHero().StartESpell();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        descriptionText.gameObject.SetActive(true);
        descriptionText.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        descriptionText.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    [SerializeField] Image heroFront;
    [SerializeField] UISpell[] spells;
    [SerializeField] Color activeSpell, usedSpell;

    public static HeroPanel Instance;

    Main main;

    private void Awake()
    {
        Instance = this;
        main = Main.Instance;
    }

    private void Update()
    {
        CastSpell();
    }

    void CastSpell()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            main.GetActiveHero().StartQSpell();
        else if (Input.GetKeyDown(KeyCode.W))
            main.GetActiveHero().StartWSpell();
        else if (Input.GetKeyDown(KeyCode.E))
            main.GetActiveHero().StartESpell();
    }

    public void UpdateHeroPanel(Hero hero)
    {
        heroFront.gameObject.SetActive(true);
        heroFront.sprite = hero.heroFront;
        for (int i = 0; i < 3; i++)
        {
            spells[i].gameObject.SetActive(true);
            spells[i].image.sprite = hero.spells[i].sprite;
            spells[i].description = hero.spells[i].description;
            spells[i].costText.text = hero.spells[i].cost.ToString();
            if (hero.spells[i].used)
            {
                spells[i].image.color = usedSpell;
                spells[i].timer.gameObject.SetActive(true);
                spells[i].timer.text = hero.spells[i].curRecoveryDuration.ToString();
            }
            else if(hero.spells[i].active)
            {
                spells[i].timer.gameObject.SetActive(false);
                spells[i].image.color = activeSpell;
            }
            else
            {
                spells[i].timer.gameObject.SetActive(false);
                spells[i].image.color = Color.white;
            }
        }
    }

    public void OffHeroPanel()
    {
        heroFront.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
            spells[i].gameObject.SetActive(false);
    }
}
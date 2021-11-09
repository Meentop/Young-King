using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int posY, posX;

    [SerializeField] Color moveColor, attackColor, standardColor, wallColor;

    public bool free = true, wall = false, enemyCell = false, pathCell = false;

    Image image;
    Main main;

    States state = States.standard;

    public Cell previousCell = null;

    enum States {
        move,
        attack,
        standard,
        wall
    }

    private void Start()
    {
        image = GetComponent<Image>();
        main = Main.Instance;
        if (wall)
        {
            state = States.wall;
            image.color = wallColor;
        }
    }

    public void MouseDown()
    {
        Debug.Log(posX + " " + posY);
        if (!wall)
        {
            if (state == States.move)
            {
                main.GetActiveHero().StartMovement(posX, posY);
            }
            else if (state == States.attack)
            {
                if (main.GetEnemyOnCell(posX, posY) != null)
                    main.GetActiveHero().StartAttack(new Vector2(posX, posY));
            }
            else if (state == States.standard)
            {
                if (free)
                    main.SetNoactiveAllHeroes();
            }
        }
    }

    public void SetMove()
    {
        if (free && !wall)
        {
            state = States.move;
            image.color = moveColor;
        }
    }

    public void SetAttack()
    {
        if (!wall)
        {
            state = States.attack;
            image.color = attackColor;
        }
    }

    public void SetStandard()
    {
        if (!wall)
        {
            state = States.standard;
            image.color = standardColor;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int posY, posX;

    [SerializeField] Material moveMaterial, attackMaterial, standardMaterial, wallMaterial;

    [HideInInspector] public bool free = true, enemyCell = false, pathCell = false, heroCell = false;
    public bool wall = false;

    MeshRenderer mesh;
    Main main;

    States state = States.standard;

    [HideInInspector] public Cell previousCell = null;
    HeroPanel heroPanel;

    enum States {
        move,
        attack,
        standard,
        wall
    }

    private void Start()
    {
        heroPanel = HeroPanel.Instance;
        mesh = GetComponent<MeshRenderer>();
        main = Main.Instance;
        if (wall)
        {
            state = States.wall;
            mesh.material = wallMaterial;
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
                if (!heroCell)
                    main.SetNoactiveAllHeroes();
                heroPanel.OffHeroPanel();
                main.ClearActiveSpells();
            }
        }
    }

    public void SetMove()
    {
        if (free && !wall)
        {
            state = States.move;
            mesh.material = moveMaterial;
        }
    }

    public void SetAttack()
    {
        if (!wall)
        {
            state = States.attack;
            mesh.material = attackMaterial;
        }
    }

    public void SetStandard()
    {
        if (!wall)
        {
            state = States.standard;
            mesh.material = standardMaterial;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Creature
{
    [HideInInspector] public bool active = false, isMoveCellsActive = false, isAttackCellsActive = false;

    protected override void Start()
    {
        base.Start(); 
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            main.heroes.Remove(GetComponent<Hero>());
            Destroy(gameObject);
        }
        UpdateHPInfo();
    }

    public void MouseDown(bool leftButton)
    {
        if (!isMove && main.heroTurn)
        {
            main.SetNoactiveAllHeroes();
            active = true;
            if (leftButton)
                OnMoveCells();
            else
                OnAttackCells();
        }
    }

    void OnMoveCells()
    {
        isMoveCellsActive = true;
        isAttackCellsActive = false;
        for (int i = 0; i < moveCells.Length / 2; i++)
        {
            if (moveCells[i, X] != NonexistentCell && moveCells[i, Y] != NonexistentCell)
                main.SetMoveCell(moveCells[i, X], moveCells[i, Y]);
        }
    }

    void OnAttackCells()
    {
        if (!wasToAttack)
        {
            isMoveCellsActive = false;
            isAttackCellsActive = true;
            for (int i = 0; i < attackCells.Length / 2; i++)
            {
                if (attackCells[i, X] != -1 && attackCells[i, Y] != -1)
                    main.SetAttackCell(attackCells[i, X], attackCells[i, Y]);
            }
        }
    }

    public void SetNoactive()
    {
        isMoveCellsActive = false;
        isAttackCellsActive = false;
        active = false;
    }
}

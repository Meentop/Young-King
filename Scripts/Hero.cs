using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Creature
{
    [HideInInspector] public bool active = false, isMoveCellsActive = false, isAttackCellsActive = false;

    [Header("Spells")]
    public Sprite heroFront;
    public Spell[] spells = new Spell[3];
    protected HeroPanel heroPanel;

    protected override void Start()
    {
        base.Start();
        heroPanel = HeroPanel.Instance;
        for (int i = 0; i < 3; i++)
        {
            spells[i].curRecoveryDuration = spells[i].recoveryDuration;
        } 
    }

    public override void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            main.creatures.Remove(this);
            main.heroes.Remove(this);
            Destroy(gameObject);
        }
        UpdateHPInfo();
    }

    public void MouseDown(bool leftButton)
    {
        if (!isMove && main.heroTurn && !HasEffect(Effect.stunned))
        {
            main.SetNoactiveAllHeroes();
            active = true;
            heroPanel.UpdateHeroPanel(this);
            if (leftButton)
                OnMoveCells();
            else
                OnAttackCells();
        }
    }

    protected void OnMoveCells()
    {
        isMoveCellsActive = true;
        isAttackCellsActive = false;
        for (int i = 0; i < moveCells.Length / 2; i++)
        {
            if (moveCells[i, X] != NonexistentCell && moveCells[i, Y] != NonexistentCell)
                main.SetMoveCell(moveCells[i, X], moveCells[i, Y]);
        }
    }

    protected void OnAttackCells()
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


    public abstract void StartQSpell();

    public abstract void StartWSpell();

    public abstract void StartESpell();
}


[System.Serializable]
public class Spell 
{
    public Sprite sprite;
    public string description;
    public int recoveryDuration;
    public int curRecoveryDuration;
    public int cost;
    public bool used;
    public bool active;
}
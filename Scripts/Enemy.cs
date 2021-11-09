using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Creature
{
    public Hero target;

    public string status;

    public int importance = 0;

    EnemiesMoveSystem enemiesMoveSystem;

    Pathfinding pathfinding;

    protected override void Start()
    {
        base.Start();
        enemiesMoveSystem = EnemiesMoveSystem.Instance;

        pathfinding = gameObject.GetComponent<Pathfinding>();
    }

    public override void StartAttack(Vector2 heroPos)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, GetDirectionAngle(transform.position, heroPos), transform.rotation.z);
        skinsAnimator.SetTrigger("Attack");
        StartCoroutine(ToDamage(heroPos));
    }

    protected override IEnumerator ToDamage(Vector2 heroPos)
    {
        yield return new WaitForSeconds(durationAnimationAttackInSeconds);
        main.GetHeroOnCell((int)heroPos.x, (int)heroPos.y).GetDamage(damage);
        wasToAttack = true;
        enemiesMoveSystem.EndEnemiesMove();
    }

    public override void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            main.enemies.Remove(GetComponent<Enemy>());
            Destroy(gameObject);
        }
        main.EndStep();
        SetCurrentAllCells();
        main.SetNoactiveAllHeroes();
        UpdateHPInfo();
    }


    public void TargetNearestHero()
    {
        //now this function is target random hero
        Debug.Log(transform.name);
        target = main.heroes[Random.Range(0, main.heroes.Count)];
    }

    const int AttackImportance = 3, MovementImportnce = 1;

    public void CheckStatus()
    {
        SetCurrentAllCells();
        CheckAttack();
        CheckMovement();
    }

    void CheckAttack()
    {
        if (!wasToAttack)
        {
            for (int i = 0; i < attackCellsStr.Length; i++)
            {
                if (main.CheckHeroOnCell(attackCells[i, X], attackCells[i, Y]))
                {
                    SetImportanceAndStatus(AttackImportance, "attack");
                    return;
                }
            }
        }
    }

    Vector2 path;
    void CheckMovement()
    {
        int[,] moveCells = ConvertStringArrayToIntArray(moveCellsStr);
        Vector2 path = pathfinding.GetPath(main.GetCell(posX, posY), main.GetCell(target.posX, target.posY), moveCells);
        if ((path.x <= 9 && path.x >= 0) && (path.y <= 9 && path.y >= 0))
        {
            SetImportanceAndStatus(MovementImportnce, "movement");
            this.path = path;
        }
        else
        {
            Debug.Log("путь не найден " + gameObject.name);
        }
    }


    public void ApplyStatus()
    {
        if (status == "attack")
            ApplyAttack();
        else if (status == "movement")
            ApplyMovement();
        else if (importance == 0)
            enemiesMoveSystem.EndEnemiesMove();
    }

    void ApplyAttack()
    {
        List<Hero> heroesInAttackCells = new List<Hero>();
        for (int i = 0; i < attackCellsStr.Length; i++)
        {
            if(main.CheckHeroOnCell(attackCells[i, X], attackCells[i, Y]))
                heroesInAttackCells.Add(main.GetHeroOnCell(attackCells[i, X], attackCells[i, Y]));
        }
        int randHero = Random.Range(0, heroesInAttackCells.Count);
        Vector2 posHeroToBeDamaged = new Vector2(heroesInAttackCells[randHero].posX, heroesInAttackCells[randHero].posY);
        StartAttack(posHeroToBeDamaged);
    }

    void ApplyMovement()
    {
        StartMovement((int)path.x, (int)path.y);
    }

    protected override void EndMovement()
    {
        enemiesMoveSystem.EndEnemiesMove();
        SetCurrentAllCells();
        skinsAnimator.SetBool("isMove", false);
        isMove = false;
    }


    public void ClearStatusAndImportance()
    {
        importance = 0;
        status = "";
    }

    public void SetImportanceAndStatus(int importance, string status)
    {
        if (importance > this.importance)
        {
            OverrideImportanceAndStatus(importance, status);
        }
        else if(importance == this.importance)
        {
            if (Random.value > 0.5f)
                OverrideImportanceAndStatus(importance, status);
        }
    }

    void OverrideImportanceAndStatus(int importance, string status)
    {
        this.importance = importance;
        this.status = status;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public List<Hero> heroes;
    public List<Enemy> enemies;
    public List<Creature> creatures;

    [SerializeField] Cell[] cells;

    public bool heroTurn = true;

    [SerializeField] int step = 2;

    [SerializeField] Text stepText;

    [SerializeField] GameObject heroTurnImg, enemyTurnImg;

    public static Main Instance;

    EnemiesMoveSystem enemiesMoveSystem;
    HeroPanel heroPanel;

    private void Awake()
    {
        Instance = this;
        heroes.AddRange(FindObjectsOfType<Hero>());
        enemies.AddRange(FindObjectsOfType<Enemy>());
        creatures.AddRange(FindObjectsOfType<Creature>());
    }

    private void Start()
    {
        heroPanel = HeroPanel.Instance;
        enemiesMoveSystem = EnemiesMoveSystem.Instance;
        SetFreeAllCells();
        foreach (Hero hero in heroes)
            hero.SetCurrentAllCells();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && heroTurn)
        {
            SetNoactiveAllHeroes();
            EndStep();
        }
    }

    public void EndStep()
    {
        step--;
        if (step == 0)
        {
            heroTurn = !heroTurn;
            step = 2;
            ClearCreatureAttack();
        }
        stepText.text = step.ToString();
        SetFreeAllCells();
        heroPanel.OffHeroPanel();
        ClearActiveSpells();
        MinusSpellsRecovery();
        MinusEffectsDuration();
        UpdateEffectsPanels();
        if (enemies.Count == 0)
            Debug.Log("Win");
        else if (heroes.Count == 0)
            Debug.Log("Lose");
        if (heroTurn)
            EnableHeroIcon();
        else
        {
            EnableEnemyIcon();
            StartCoroutine(enemiesMoveSystem.StartEnemiesMove());
        }
    }

    void EnableHeroIcon()
    {
        stepText.color = Color.white;
        heroTurnImg.SetActive(true);
        enemyTurnImg.SetActive(false);
    }

    void EnableEnemyIcon()
    {
        stepText.color = Color.red;
        heroTurnImg.SetActive(false);
        enemyTurnImg.SetActive(true);
    }


    public Cell GetCell(int x, int y)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].posX == x && cells[i].posY == y)
                return cells[i];
        }
        return null;
    }

    public void SetMoveCell(int x, int y)
    {
        if(GetCell(x, y) != null)
            GetCell(x, y).SetMove();
    }

    public void SetAttackCell(int x, int y)
    {
        if (GetCell(x, y) != null)
            GetCell(x, y).SetAttack();
    }

    public void SetNoactiveAllHeroes()
    {
        foreach (Hero hero in heroes)
        {
            hero.SetNoactive();
        }
        SetStandardAllCells();
    }

    public void SetStandardAllCells()
    {
        foreach (Cell cell in cells)
        {
            cell.SetStandard();
        }
    }

    public void SetFreeAllCells()
    {
        foreach (Cell cell in cells)
        {
            cell.free = true;
            cell.enemyCell = false;
            cell.heroCell = false;
        }
        for (int i = 0; i < creatures.Count; i++)
            GetCell(creatures[i].posX, creatures[i].posY).free = false;
        for (int i = 0; i < heroes.Count; i++)
            GetCell(heroes[i].posX, heroes[i].posY).heroCell = true;
        for (int i = 0; i < enemies.Count; i++)
            GetCell(enemies[i].posX, enemies[i].posY).enemyCell = true;
    }

    public Hero GetActiveHero()
    {
        foreach (Hero hero in heroes)
        {
            if (hero.active)
                return hero;
        }
        return null;
    }

    public Enemy GetEnemyOnCell(int posX, int posY)
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.posX == posX && enemy.posY == posY)
                return enemy;
        }
        return null;
    }

    public Hero GetHeroOnCell(int posX, int posY)
    {
        foreach (Hero hero in heroes)
        {
            if (hero.posX == posX && hero.posY == posY)
                return hero;
        }
        return null;
    }

    public bool CheckHeroOnCell(int posX, int posY)
    {
        if (GetHeroOnCell(posX, posY) == null)
            return false;
        else
            return true;
    }

    //Pathfinding

    public void ClearPreviousCells()
    {
        foreach (Cell cell in cells)
        {
            cell.previousCell = null;
        }
    }

    public void ClearPathCells()
    {
        foreach (Cell cell in cells)
        {
            cell.pathCell = false;
        }
    }

    public void ClearCreatureAttack()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.wasToAttack = false;
        }
        foreach (Hero hero in heroes)
        {
            hero.wasToAttack = false;
        }
    }

    //Spells

    public void ClearActiveSpells()
    {
        foreach (Hero hero in heroes)
        {
            for (int i = 0; i < 3; i++)
            {
                hero.spells[i].active = false;
            }
        }
    }

    public void MinusSpellsRecovery()
    {
        foreach (Hero hero in heroes)
        {
            for (int i = 0; i < 3; i++)
            {
                if (hero.spells[i].used)
                    hero.spells[i].curRecoveryDuration--;
                if (hero.spells[i].curRecoveryDuration == 0)
                {
                    hero.spells[i].curRecoveryDuration = hero.spells[i].recoveryDuration;
                    hero.spells[i].used = false;
                }
            }
        }
    }

    //Effects

    void MinusEffectsDuration()
    {
        foreach (Creature creature in creatures)
        {
            for (int i = 0; i < creature.effects.Count; i++)
            {
                creature.effects[i].duration--;
                if (creature.effects[i].duration == 0)
                    creature.effects.Remove(creature.effects[i]);
            }
        }
    }

    void UpdateEffectsPanels()
    {
        foreach (Creature creature in creatures)
        {
            creature.effectsPanel.UpdateEffectsPanel();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public List<Hero> heroes;

    public List<Enemy> enemies;

    [SerializeField] Cell[] cells;

    public bool heroTurn = true;

    [SerializeField] int step = 2;

    [SerializeField] Text stepText;

    [SerializeField] GameObject heroTurnImg, enemyTurnImg;

    public static Main Instance;

    EnemiesMoveSystem enemiesMoveSystem;

    private void Awake()
    {
        Instance = this;
        heroes = GetAllHeroes();
        enemies = GetAllEnemies();
    }

    private void Start()
    {
        enemiesMoveSystem = EnemiesMoveSystem.Instance;
        SetFreeAllCells();
        foreach (Hero hero in heroes)
            hero.SetCurrentAllCells();
    }

    List<Hero> GetAllHeroes()
    {
        GameObject[] heroesGameObj = GameObject.FindGameObjectsWithTag("Hero");
        List<Hero> heroes = new List<Hero>();
        for (int i = 0; i < heroesGameObj.Length; i++)
            heroes.Add(heroesGameObj[i].GetComponent<Hero>());
        return heroes;
    }

    List<Enemy> GetAllEnemies()
    {
        GameObject[] enemiesGameObj = GameObject.FindGameObjectsWithTag("Enemy");
        List<Enemy> enemies = new List<Enemy>();
        for (int i = 0; i < enemiesGameObj.Length; i++)
            enemies.Add(enemiesGameObj[i].GetComponent<Enemy>());
        return enemies;
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
        if (heroTurn)
        {
            stepText.color = Color.white;
            heroTurnImg.SetActive(true);
            enemyTurnImg.SetActive(false);
        }
        else
        {
            stepText.color = Color.red;
            heroTurnImg.SetActive(false);
            enemyTurnImg.SetActive(true);
            StartCoroutine(enemiesMoveSystem.StartEnemiesMove());
        }
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
        for (int i = 0; i < heroes.Count; i++)
        {
            GetCell(heroes[i].posX, heroes[i].posY).free = false;
            GetCell(heroes[i].posX, heroes[i].posY).heroCell = true;
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            GetCell(enemies[i].posX, enemies[i].posY).free = false;
            GetCell(enemies[i].posX, enemies[i].posY).enemyCell = true;
        }
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
}
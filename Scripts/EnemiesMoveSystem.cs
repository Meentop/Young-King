using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesMoveSystem : MonoBehaviour
{
    Main main;

    public static EnemiesMoveSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        main = Main.Instance;
    }

    [SerializeField] const float TimeBetweenEnemyMoves = 1f;

    public IEnumerator StartEnemiesMove()
    {
        yield return new WaitForSeconds(TimeBetweenEnemyMoves);
        foreach (Enemy enemy in main.enemies)
            enemy.CheckStatus();
        GetImportantEnemy().ApplyStatus();
    }

    public void EndEnemiesMove()
    {
        foreach (Enemy enemy in main.enemies)
            enemy.ClearStatusAndImportance();
        main.EndStep();
    }

    Enemy GetImportantEnemy()
    {
        int maxImportance = 0;
        List<Enemy> enemiesWithMaxImportance = new List<Enemy>();
        foreach (Enemy enemy in main.enemies) {
            if(enemy.importance > maxImportance)
            {
                enemiesWithMaxImportance.Clear();
                enemiesWithMaxImportance.Add(enemy);
                maxImportance = enemy.importance;
            }
            else if(enemy.importance == maxImportance)
                enemiesWithMaxImportance.Add(enemy);
        }
        return enemiesWithMaxImportance[Random.Range(0, enemiesWithMaxImportance.Count)];
    }
}

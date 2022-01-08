using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Creature : MonoBehaviour
{
    [HideInInspector] public int posX, posY;

    public int[,] moveCells, attackCells;

    protected Main main;

    Transform canvas;

    public bool show = false;

    protected Animator skinsAnimator;

    protected const int NonexistentCell = -1, X = 0, Y = 1;

    protected virtual void Awake()
    {
        posX = (int)transform.position.x;
        posY = (int)transform.position.z;
        UpdateHPInfo();
        SetCurrentAllCells();
    }

    protected virtual void Start()
    {
        main = Main.Instance;
        canvas = transform.GetChild(0).transform;
        skinsAnimator = transform.GetChild(1).GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        canvas.rotation = main.transform.rotation;
        Movement();
    }

    //Movement

    [Header("Movement")]
    [SerializeField] protected string[] moveCellsStr;

    protected bool isMove = false;

    [SerializeField] protected float runSpeed = 5f, walkSpeed = 1.5f;

    protected float moveSpeed;

    [SerializeField] protected float angleOffset = 0;

    protected virtual void Movement()
    {
        if (isMove)
        {
            Vector3 target = new Vector3(posX, transform.position.y, posY);
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (transform.position.x == posX && transform.position.z == posY)
                EndMovement();
        }
    }

    public virtual void StartMovement(int futurePosX, int futurePosY)
    {
        Vector2 futurePosition = new Vector2(futurePosX, futurePosY);
        transform.rotation = Quaternion.Euler(transform.rotation.x, GetDirectionAngle(transform.position, futurePosition), transform.rotation.z);
        SetMoveOrWalk(Vector2.Distance(new Vector2(this.posX, this.posY), futurePosition));
        skinsAnimator.SetBool("isMove", true);
        this.posX = futurePosX;
        this.posY = futurePosY;
        isMove = true;
        main.SetNoactiveAllHeroes();
        main.SetFreeAllCells();
    }

    protected float GetDirectionAngle(Vector3 startPosition, Vector2 futurePosition)
    {
        return (Mathf.Atan2(futurePosition.y - startPosition.z, futurePosition.x - startPosition.x) * (180 / Mathf.PI) * -1) + angleOffset;
    }

    void SetMoveOrWalk(float distance)
    {
        if (distance > 1.5f)
        {
            skinsAnimator.SetBool("isWalk", false);
            moveSpeed = runSpeed;
        }
        else
        {
            skinsAnimator.SetBool("isWalk", true);
            moveSpeed = walkSpeed;
        }
    }

    protected virtual void EndMovement()
    {
        main.EndStep();
        SetCurrentAllCells();
        skinsAnimator.SetBool("isMove", false);
        isMove = false;
    }

    //HP

    [Header("HP")]
    public int hp;

    [SerializeField] Text hpText;

    public abstract void GetDamage(int damage);

    public void UpdateHPInfo()
    {
        hpText.text = hp.ToString();
    }

    //Attack

    [Header("Attack")]
    [SerializeField] protected string[] attackCellsStr;

    public int damage;

    [HideInInspector] public bool wasToAttack = false;

    [SerializeField] protected float durationAnimationAttack = 0.01f;

    public virtual void StartAttack(Vector2 enemyPos)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, GetDirectionAngle(transform.position, enemyPos), transform.rotation.z);
        skinsAnimator.SetTrigger("Attack");
        StartCoroutine(ToDamage(enemyPos));
    }

    protected virtual IEnumerator ToDamage(Vector2 enemyPos)
    {
        yield return new WaitForSeconds(durationAnimationAttack);
        main.GetEnemyOnCell((int)enemyPos.x, (int)enemyPos.y).GetDamage(damage);
        wasToAttack = true;
    }

    //Cells

    public int[,] ConvertStringArrayToIntArray(string[] cellsStr)
    {
        int[,] cells = new int[cellsStr.Length, 2];
        string[] cellCoordinate = new string[2];
        for (int i = 0; i < cellsStr.Length; i++)
        {
            cellCoordinate = cellsStr[i].Split(new char[] { ',' });
            cells[i, X] = int.Parse(cellCoordinate[X]);
            cells[i, Y] = int.Parse(cellCoordinate[Y]);
        }
        return cells;
    }

    void SetCurrentCells(ref int[,] cells)
    {
        for (int i = 0; i < cells.Length / 2; i++)
        {
            if (cells[i, X] + posX >= 0 && cells[i, X] + posX <= 9)
                cells[i, X] = cells[i, X] + posX;
            else
                cells[i, X] = NonexistentCell;

            if (cells[i, Y] + posY >= 0 && cells[i, Y] + posY <= 9)
                cells[i, Y] = cells[i, Y] + posY;
            else
                cells[i, Y] = NonexistentCell;
        }
    }

    public void SetCurrentAllCells()
    {
        moveCells = ConvertStringArrayToIntArray(moveCellsStr);
        attackCells = ConvertStringArrayToIntArray(attackCellsStr);
        SetCurrentCells(ref moveCells);
        SetCurrentCells(ref attackCells);
    }

    //Show

    public void Show()
    {
        if (show)
        {
            for (int i = 0; i < moveCells.Length / 2; i++)
                Debug.Log("move " + moveCells[i, X] + " " + moveCells[i, Y]);
            for (int i = 0; i < attackCells.Length / 2; i++)
                Debug.Log("attack " + attackCells[i, X] + " " + attackCells[i, Y]);
        }
    }

    //Effects

    public CreatureEffector effectsPanel;

    public List<ActiveEffect> effects;

    public void AddEffect(Effect effect, int duration)
    {
        if (!HasEffect(effect))
        {
            effects.Add(new ActiveEffect(effect, duration));
        }
        else
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].effect == effect)
                    effects[i].duration += duration;
            }
        }
    }

    public bool HasEffect(Effect testedEffect)
    {
        foreach (ActiveEffect effect in effects)
        {
            if (effect.effect == testedEffect)
                return true;
        }
        return false;
    }
}

[System.Serializable]
public class ActiveEffect 
{
    public Effect effect;
    public int duration;

    public ActiveEffect(Effect effect, int duration)
    {
        this.effect = effect;
        this.duration = duration;
    }
}
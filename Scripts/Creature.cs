using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Creature : MonoBehaviour
{
    public int posX, posY, damage, hp;

    [SerializeField] Text hpText;

    [SerializeField] protected string[] moveCellsStr, attackCellsStr;

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

    protected virtual void Movement()
    {
        if (isMove)
        {
            skinsAnimator.SetBool("isMove", true);
            Vector3 target = new Vector3(posX, transform.position.y, posY);
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (transform.position.x == posX && transform.position.z == posY)
                EndMovement();
        }
    }

    protected virtual void EndMovement()
    {
        main.EndStep();
        SetCurrentAllCells();
        skinsAnimator.SetBool("isMove", false);
        isMove = false;
    }


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


    public abstract void GetDamage(int damage);

    public void UpdateHPInfo()
    {
        hpText.text = hp.ToString();
    }

    

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


    public bool wasToAttack = false;
    [SerializeField] protected float durationAnimationAttackInSeconds = 0.01f;
    public virtual void StartAttack(Vector2 enemyPos)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, GetDirectionAngle(transform.position, enemyPos), transform.rotation.z);
        skinsAnimator.SetTrigger("Attack");
        StartCoroutine(ToDamage(enemyPos));
    }

    protected float GetDirectionAngle(Vector3 startPosition, Vector2 futurePosition)
    {
        return (Mathf.Atan2(futurePosition.y - startPosition.z, futurePosition.x - startPosition.x) * (180 / Mathf.PI) * -1);
    }

    protected virtual IEnumerator ToDamage(Vector2 enemyPos)
    {
        yield return new WaitForSeconds(durationAnimationAttackInSeconds);
        main.GetEnemyOnCell((int)enemyPos.x, (int)enemyPos.y).GetDamage(damage);
        wasToAttack = true;
    }


    protected bool isMove = false;

    [SerializeField] protected float runSpeed = 5f, walkSpeed = 1.5f;

    protected float moveSpeed;
    public virtual void StartMovement(int futurePosX, int futurePosY)
    {
        Vector2 futurePosition = new Vector2(futurePosX, futurePosY);
        transform.rotation = Quaternion.Euler(transform.rotation.x, GetDirectionAngle(transform.position, futurePosition), transform.rotation.z);
        SetMoveOrWalk(Vector2.Distance(new Vector2(this.posX, this.posY), futurePosition));
        this.posX = futurePosX;
        this.posY = futurePosY;
        isMove = true;
        main.SetNoactiveAllHeroes();
        main.SetFreeAllCells();
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
}

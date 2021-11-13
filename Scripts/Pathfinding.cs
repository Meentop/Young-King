using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Cell startCell, targetCell;

    int[,] moveCells;

    const int X = 0, Y = 1;

    Main main;
    [SerializeField] private List<Cell> checkedCells = new List<Cell>(); 
    //waiting cells wait when the child cells be checked
    [SerializeField] private List<Cell> waitingCells = new List<Cell>();


    private void Start()
    {
        main = Main.Instance;
    }

    public Vector2 GetPath(Cell startCell, Cell targetCell, int[,] moveCells)
    {
        ClearData();
        this.startCell = startCell;
        this.targetCell = targetCell;
        this.moveCells = moveCells;
        waitingCells.Add(startCell);
        return FindPath();
    }

    Vector2 FindPath()
    {
        List<Cell> parentCells = new List<Cell>();
        for (int a = 0; a < 100; a++)
        {
            parentCells.Clear();
            parentCells.AddRange(waitingCells);
            for (int i = 0; i < parentCells.Count; i++)
            {
                for (int j = 0; j < moveCells.Length / 2; j++)
                {
                    if (main.GetCell(moveCells[j, X] + parentCells[i].posX, moveCells[j, Y] + parentCells[i].posY) != null)
                    {
                        Cell childCell = main.GetCell(moveCells[j, X] + parentCells[i].posX, moveCells[j, Y] + parentCells[i].posY);
                        if(childCell == targetCell)
                        {
                            childCell.previousCell = parentCells[i];
                            return GetPathCell();
                        }
                        else if (!childCell.wall && childCell.free && childCell.previousCell == null)
                        {
                            childCell.previousCell = parentCells[i];
                            waitingCells.Add(childCell);
                        }
                    }
                }
                waitingCells.Remove(parentCells[i]);
                checkedCells.Add(parentCells[i]);
            }
        }
        return Vector2.positiveInfinity;
    }

    Vector2 GetPathCell()
    {
        Cell curCell = targetCell;
        while(curCell.previousCell != null)
        {
            curCell = curCell.previousCell;
            curCell.pathCell = true;
        }
        for (int i = 0; i < moveCells.Length / 2; i++)
        {
            if (main.GetCell(startCell.posX + moveCells[i, X], startCell.posY + moveCells[i, Y]).pathCell)
                return new Vector2(startCell.posX + moveCells[i, X], startCell.posY + moveCells[i, Y]);
        }
        return Vector2.positiveInfinity;
    }

    void ClearData()
    {
        waitingCells.Clear();
        checkedCells.Clear();
        main.ClearPreviousCells();
        main.ClearPathCells();
    }
}

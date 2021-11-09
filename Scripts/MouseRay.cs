using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    Main main;

    private void Start()
    {
        main = Main.Instance;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Hero heroOnMouseEnter = hit.collider.gameObject.GetComponent<Hero>();
            if (heroOnMouseEnter)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (heroOnMouseEnter.active && heroOnMouseEnter.isMoveCellsActive)
                        main.SetNoactiveAllHeroes();
                    else
                        heroOnMouseEnter.MouseDown(true);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (heroOnMouseEnter.active && heroOnMouseEnter.isAttackCellsActive)
                        main.SetNoactiveAllHeroes();
                    else
                        heroOnMouseEnter.MouseDown(false);
                }
            }
        }
    }
}

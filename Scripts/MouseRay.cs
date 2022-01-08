using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    Main main;

    [SerializeField] LayerMask layerMask;

    [SerializeField] DisappearingText stunnedText;

    private void Start()
    {
        main = Main.Instance;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            if (Input.GetMouseButtonDown(0))
                hit.collider.gameObject.GetComponent<Cell>().MouseDown();
        }
        if (Physics.Raycast(ray, out hit))
        {
            Hero heroOnMouseEnter = hit.collider.gameObject.GetComponent<Hero>();
            if (heroOnMouseEnter)
            {
                if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    if (heroOnMouseEnter.HasEffect(Effect.stunned))
                        stunnedText.ShowText(2f, 5f); 
                }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    int rotation = 0;

    [SerializeField] float speed;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            rotation -= 90;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            rotation += 90;
        }
        transform.rotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, rotation, 0), Time.deltaTime * speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] Vector2 maxLeftUpPos, maxRightDownPos;

    [SerializeField] Transform canvas;

    Vector3 mouseDownPosition, mousePositionDelta, startMapPosition;

    private void Start()
    {
        startMapPosition = transform.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Input.GetMouseButtonDown(0))
            mouseDownPosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            mousePositionDelta = Input.mousePosition - mouseDownPosition;
            Vector3 pos = new Vector3(startMapPosition.x + mousePositionDelta.x, startMapPosition.y + mousePositionDelta.y);
            float minX = maxRightDownPos.x * canvas.localScale.x + canvas.position.x;
            float maxX = maxLeftUpPos.x * canvas.localScale.x + canvas.position.x;
            float minY = maxLeftUpPos.y * canvas.localScale.y + canvas.position.y;
            float maxY = maxRightDownPos.y * canvas.localScale.y + canvas.position.y;
            transform.position = new Vector3(Mathf.Clamp(pos.x, minX, maxX), Mathf.Clamp(pos.y, minY, maxY));
        }
        if (Input.GetMouseButtonUp(0))
            startMapPosition = transform.position;
    }

}
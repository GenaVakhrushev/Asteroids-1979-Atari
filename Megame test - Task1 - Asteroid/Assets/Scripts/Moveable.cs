using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    protected Vector2 velocity;

    protected float ScreenWidth
    {
        get
        {
            Vector2 leftCorner = Camera.main.ScreenToWorldPoint(Vector2.zero);
            Vector2 rightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
            return Vector2.Distance(leftCorner, rightCorner);
        }
    }

    protected virtual void FixedUpdate()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    //выход за границы камеры (нужно переместить на противоположную сторону)
    protected virtual void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bool outOfScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (!outOfScreen)
            return;

        Vector3 newPos = Vector3.zero;

        //выход слева
        if (screenPos.x < 0)
        {
            newPos = new Vector3(Screen.width, screenPos.y, screenPos.z);
        }
        //выход справа
        if (screenPos.x > Screen.width)
        {
            newPos = new Vector3(0, screenPos.y, screenPos.z);
        }
        //выход снизу
        if (screenPos.y < 0)
        {
            newPos = new Vector3(screenPos.x, Screen.height, screenPos.z);
        }
        //выход сверху
        if (screenPos.y > Screen.height)
        {
            newPos = new Vector3(screenPos.x, 0, screenPos.z);
        }

        transform.position = Camera.main.ScreenToWorldPoint(newPos);
    }
}

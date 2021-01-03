using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LookAtMouse : NetworkBehaviour
{
    void Update()
    {
        if(isLocalPlayer)
        {
            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }
}

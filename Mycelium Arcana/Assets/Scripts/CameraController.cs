using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ymax;
    public float ymin;
    public float xmax;
    public float xmin;
    //create mins and max for x and y and test to see if theyve been reached.
    void Update()
    {
        if (PlayerController.me != null && !PlayerController.me.dead)
        {
            Vector3 targetPos = PlayerController.me.transform.position;
            //targetPos.x = Mathf.Clamp(targetPos.x, xmin, xmax);
            //targetPos.y = Mathf.Clamp(targetPos.y, ymin, ymax);
            targetPos.z = -20;
            transform.position = targetPos;
            if (targetPos.x < xmin)
            {
                targetPos.x = xmin;
            }
        }
    }
}

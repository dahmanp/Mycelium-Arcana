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
        // does the player exist?
        if (PlayerController.me != null && !PlayerController.me.dead)
        {
            Vector3 targetPos = PlayerController.me.transform.position;
            targetPos.z = -20;
            //targetPos.x = Mathf.Clamp(transform.position.x, xmin, xmax);
            //targetPos.y = Mathf.Clamp(transform.position.y, ymin, ymax);
            transform.position = targetPos;
        }
    }
}

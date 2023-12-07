using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimeTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<PlayerController>().startTime = Time.time;
    }
}

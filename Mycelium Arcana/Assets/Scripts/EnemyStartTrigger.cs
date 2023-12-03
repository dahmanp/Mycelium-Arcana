using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartTrigger : MonoBehaviour
{
    public GameObject EnemySpawn;
    public GameObject trigger;

    void Start()
    {
        EnemySpawn.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Holothurian") || other.gameObject.CompareTag("Etori") || other.gameObject.CompareTag("Velkivon") || other.gameObject.CompareTag("Avem"))
        {
            EnemySpawn.SetActive(true);
            Destroy(trigger);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Trigger : MonoBehaviour
{
    public GameObject block;
    public GameObject instruct;
    public GameObject Spawner1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Holothurian") || other.gameObject.CompareTag("Etori") || other.gameObject.CompareTag("Velkivon") || other.gameObject.CompareTag("Avem"))
        {
            if (other.gameObject.GetComponent<PlayerController>().instance.enemiesKilled >= 10)
            {
                Destroy(block);
                Destroy(Spawner1);
            }
            else
            {
                instruct.SetActive(true);
                Invoke("Deactivate", 2.0f);
            }
        }
    }

    void Deactivate()
    {
        instruct.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsTriggers: MonoBehaviourPun
{
    public GameObject[] spawns;
    public GameObject[] triggers;
    public float time;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Holothurian") || other.gameObject.CompareTag("Etori") || other.gameObject.CompareTag("Velkivon") || other.gameObject.CompareTag("Avem"))
        {
            if (this.gameObject == triggers[0])
            {
                other.gameObject.transform.position = spawns[0].transform.position;
            }
            else if (this.gameObject == triggers[1])
            {
                other.gameObject.transform.position = spawns[1].transform.position;
            }
            else if (this.gameObject == triggers[2])
            {
                other.gameObject.transform.position = spawns[2].transform.position;
                other.gameObject.GetComponent<PlayerController>().timeTaken = Time.time - other.gameObject.GetComponent<PlayerController>().startTime;
                time = other.gameObject.GetComponent<PlayerController>().timeTaken;
                //PlayerPrefs.SetFloat("timeTaken", time);
                int temptime = -Mathf.RoundToInt(time * 1000.0f);
                Leaderboard1.instance.SetLeaderboardEntry(temptime);
            }
        }
    }
}

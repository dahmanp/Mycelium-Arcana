using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicTrigger : MonoBehaviour
{
    public GameObject puzzle;
    public GameObject trigger;
    public GameObject blockade;
    public GameObject help;
    public GameObject help2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (puzzle.GetComponent<PicCheck>().puzzleComplete == true && other.gameObject.GetComponent<PlayerController>().hasKey == true)
        {
            blockade.SetActive(false);
            trigger.SetActive(false);
        }
        else if (puzzle.GetComponent<PicCheck>().puzzleComplete == true && other.gameObject.GetComponent<PlayerController>().hasKey != true)
        {
            help2.SetActive(true);
            Invoke("deactivate2", 2.0f);
        }
        else
        {
            help.SetActive(true);
            Invoke("deactivate", 2.0f);
        }
    }

    void deactivate()
    {
        help.SetActive(false);
    }
    void deactivate2()
    {
        help2.SetActive(false);
    }
}

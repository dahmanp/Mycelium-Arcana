using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitCheck : MonoBehaviour
{
    public GameObject[] rowA;
    public GameObject[] rowB;
    public GameObject[] rowC;
    public GameObject[] rowD;

    public bool Acheck = true;
    public bool Bcheck = true;
    public bool Ccheck = true;
    public bool Dcheck = true;
    public bool puzzleComplete = false;

    void Start()
    {
        Acheck = testing(rowA, ref Acheck);
        Bcheck = testing(rowB, ref Bcheck);
        Ccheck = testing(rowC, ref Ccheck);
        Dcheck = testing(rowD, ref Dcheck);
    }

    void Update()
    {
        Acheck = testing(rowA, ref Acheck);
        Bcheck = testing(rowB, ref Bcheck);
        Ccheck = testing(rowC, ref Ccheck);
        Dcheck = testing(rowD, ref Dcheck);
        puzzleComplete = Acheck && Bcheck && Ccheck && Dcheck;
    }

    bool testing(GameObject[] row, ref bool check)
    {
        foreach (GameObject block in row)
        {
            if (!block.GetComponent<CircuitShift>().correct)
            {
                return false;
            }
        }
        return true;
    }
}

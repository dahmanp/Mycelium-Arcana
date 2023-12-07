using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blockType
{
    line,
    edge
}

public class CircuitShift : MonoBehaviour
{
    public blockType type;
    public int num;
    public int key;
    public bool correct = false;

    public void test()
    {
        if (type == blockType.edge)
        {
            this.gameObject.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
            num++;
            if (num == 4)
            {
                num = 0;
            }
        }
        else if (type == blockType.line)
        {
            this.gameObject.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
            num++;
            if (num == 2)
            {
                num = 0;
            }
        }
    }

    void Update()
    {
        if (num == key)
        {
            correct = true;
        }
        else
        {
            correct = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

//BUSTED UP LOL
public class HeaderInfo : MonoBehaviourPun
{
    public TextMeshProUGUI nameText;
    public Image bar;
    private float maxValue;

    public void Initialize(string text, int maxVal)
    {
        nameText.text = text;
        maxValue = maxVal;
        bar.fillAmount = 1.0f;
    }

    [PunRPC]
    void UpdateHealthBar(int value)
    {
        bar.fillAmount = (float)value / maxValue;
    }
}

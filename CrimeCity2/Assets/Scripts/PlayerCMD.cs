using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerCMD : MonoBehaviour
{
    public void ParseCommand(string text) {
        string[] cmdParts = text.Split(" ");
        Debug.Log(text);
    }
}

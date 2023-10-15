using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float TIME {get; private set;} = 0;

    [SerializeField] TMP_Text commandTMP;
    
    [Header("Cursor")]
    [SerializeField] char cursorChar = '|';
    [SerializeField] float blinkTime = 0.2f;
    
    bool cursorOn = true;
    float cTime = 0;

    InputCMD inputCMD;

    void Start() {
        inputCMD = GetComponent<InputCMD>();
    }

    void Update() {
        TIME += Time.deltaTime;

        UpdateCommand();
    }

    void UpdateCommand() {
        commandTMP.text = inputCMD.Text;

        if (TIME > cTime+blinkTime) {
            cursorOn = !cursorOn;
            cTime = TIME;
        }

        string cursor = null;
        if (cursorOn) {
            cursor = cursorChar.ToString();
        } else {
            cursor = "\u00A0";
        }

        commandTMP.text = commandTMP.text.Insert(inputCMD.Position, cursor);
    }
}

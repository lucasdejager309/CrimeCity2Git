using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] TMP_Text commandTMP;
    TMP_Text cursorTMP;
    [SerializeField] char cursorChar = '_';

    InputCMD inputCMD;

    void Start() {
        inputCMD = GetComponent<InputCMD>();
        cursorTMP = commandTMP.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Update() {
        UpdateCommand();
    }

    void UpdateCommand() {
        commandTMP.text = inputCMD.Text;
        
        string cursor = null;

        

        for (int i = 0; i < inputCMD.Position; i++) {
            cursor += " ";
        }

        //hmmmmm
        if (commandTMP.textInfo.lineCount > 1) {
            int lineCharCount = commandTMP.textInfo.lineInfo[0].characterCount;
            cursor = cursor.Remove(0, lineCharCount);
        }

        cursor += cursorChar.ToString();

        cursorTMP.text = cursor;
    }
}

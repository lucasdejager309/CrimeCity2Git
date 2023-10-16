using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextInputUI : MonoBehaviour
{
    TMP_Text commandTMP;

    float cTime = 0;
    bool cursorOn = true;
    bool showCursor;
    
    [SerializeField] float blinkTime = 0.3f;
    [SerializeField] char cursorChar = '_';
    [SerializeField] public RectTransform background;

    string displayText = "";
    int displayPos;

    public bool InFocus = false;

    void Start() {
        commandTMP = GetComponent<TMP_Text>();
    }

    void FixedUpdate() {
        if (InFocus) {
            UpdateDisplay();
        }
    }

    public void Focus(bool b) {
        InFocus = b;
        ShowCursor(InFocus);
    }

    public void UpdateDisplay() {
        if (Time.timeSinceLevelLoad > cTime+blinkTime) {
            cursorOn = !cursorOn;
            cTime = Time.timeSinceLevelLoad;
        }

        string cursor = null;
        if (cursorOn && showCursor) {
            cursor = cursorChar.ToString();
        } else if (showCursor) {
            cursor = "\u00A0";
        } else {
            cursor = "";
        }

        commandTMP.text = displayText.Insert(displayPos, cursor);

        //scales background with command lineCount
        background.sizeDelta = new Vector2(background.sizeDelta.x, commandTMP.fontSize+(commandTMP.fontSize*commandTMP.textInfo.lineCount)-commandTMP.fontSize);
    }

    public void ShowCursor(bool b) {
        showCursor = b;
        UpdateDisplay();
    }

    public void UpdateText(string text, int position) {
        displayPos = position;
        displayText = text;
    }
}

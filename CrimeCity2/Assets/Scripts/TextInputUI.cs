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
    
    [SerializeField] bool expandsWithText;
    [SerializeField] float blinkTime = 0.3f;
    [SerializeField] char cursorChar = '_';
    [SerializeField] public RectTransform background;
    float backgroundStartHeight;

    string displayText = "";
    int displayPos;
    SelectionRange displaySelection = new SelectionRange();

    public bool InFocus = false;

    void Start() {
        commandTMP = GetComponent<TMP_Text>();
        backgroundStartHeight = background.sizeDelta.y;
        if (expandsWithText) commandTMP.overflowMode = TextOverflowModes.Overflow;
        else commandTMP.overflowMode = TextOverflowModes.Truncate;
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

        if (displaySelection.IsActive) {
            commandTMP.text = commandTMP.text.Insert(displaySelection.Start, "<u>");
            if (commandTMP.text.Length > displaySelection.End+5) {
                commandTMP.text = commandTMP.text.Insert(displaySelection.End+5, "</u>");
            } else {
                commandTMP.text += "</u>";
            }
        }

        //scales background with command lineCount
        if (expandsWithText) {
            if (commandTMP.textInfo.lineCount > 1 && commandTMP.fontSize*(commandTMP.textInfo.lineCount) > backgroundStartHeight){
                background.sizeDelta = new Vector2(background.sizeDelta.x, commandTMP.fontSize + commandTMP.fontSize*(commandTMP.textInfo.lineCount-1));
            } else {
                background.sizeDelta = new Vector2(background.sizeDelta.x, backgroundStartHeight);
            }
        }   
    }

    public void ShowCursor(bool b) {
        showCursor = b;
        UpdateDisplay();
    }

    public void UpdateText(string text, int position, SelectionRange selection) {
        displayPos = position;
        displayText = text;
        displaySelection = selection;
    }
}

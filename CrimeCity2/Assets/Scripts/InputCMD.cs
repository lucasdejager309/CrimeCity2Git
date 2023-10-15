using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputCMD : MonoBehaviour
{
    static string alphabet = "abcdefghijklmnopqrstuvwxyz1234567890 ~`!@#$%^&*()-_+{}[]\\|:;\"'<>,.?/";
    
    public string Text { get; private set;} = "";
    public string[] Components { get; private set;} = null;
    bool cap = false;
    public int Position { get; private set;} = 0;

    void OnGUI()
    {
        Event e = Event.current;
        
        if (e.isKey) {
            if (e.keyCode != KeyCode.None) {
                UpdateCapitalization(e);

                if (e.type == EventType.KeyDown) {
                    UpdateString(e);
                }
            }
        }

        UpdateWords();
    }

    void UpdateString(Event e) {
        string key = Input.inputString;
        if (key != "" && alphabet.Contains(key.ToLower())) {
            //key is character to be added to command
            if (cap) {
                Text = Text.Insert(Position, key.ToUpper());
                }
            else {
                Text = Text.Insert(Position, key.ToLower());
            }
            Position++;
        } else {
            //other keys

            //backspace
            if (e.keyCode == KeyCode.Backspace) {
                if (Text.Length > 0) {
                    Position--;
                    Text = Text.Remove(Position, 1);
                }
            }

            //moving cursor < >
            if (e.keyCode == KeyCode.LeftArrow && Position > 0) {
                Position--;
            }
            if (e.keyCode == KeyCode.RightArrow && Position < Text.Length) {
                Position++;
            }
            
            //sumbit command
            if (e.keyCode == KeyCode.Return) {
                //make submit command event happen

                Text = "";
                Position = 0;
            }
        }
    }

    void UpdateCapitalization(Event e) {
        if (!cap) {
            if (e.keyCode == KeyCode.LeftShift && e.type == EventType.KeyDown) {
                cap = true;
            }
        } else {
            if (e.keyCode == KeyCode.LeftShift && e.type == EventType.KeyUp) {
                cap = false;
            }
        }
    }

    void UpdateWords() {
        Components = Text.Split(' ');
    }
}

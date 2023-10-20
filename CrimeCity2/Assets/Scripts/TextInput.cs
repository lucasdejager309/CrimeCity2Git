using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextInput : MonoBehaviour
{
    static string alphabet = "abcdefghijklmnopqrstuvwxyz1234567890~`!@#$%^&*()-_+{}[]|:;\"',.?/ ";
    
    public string Text { get; private set;} = "";
    public int Position { get; private set;} = 0;
    public SelectionRange Selection {get; private set;} = new SelectionRange();

    public UnityEvent<string> CommandSubmitted;

    [SerializeField] TextInputUI UI;
    bool cap = false;

    void Start() {
        UI = GetComponent<TextInputUI>();
    }

    void OnGUI() {
        Event e = Event.current;
        
        if (e.isKey && UI.InFocus) {
            if (e.keyCode != KeyCode.None) {
                UpdateCapitalization(e);

                if (e.type == EventType.KeyDown) {
                    UpdateString(e);
                    UI.UpdateText(Text, Position, Selection);
                    
                }
            }
        }
    }

    void UpdateString(Event e) {
        string key = Input.inputString;

        //moving cursor < >
        if (e.keyCode == KeyCode.LeftArrow	|| e.keyCode == KeyCode.RightArrow) {
            //get direction
            int dir = 0;
            if (e.keyCode == KeyCode.LeftArrow) dir = -1;
            else if (e.keyCode == KeyCode.RightArrow) dir = 1;
            
            int amount = 1;
            //move over entire word when ctrl is pressed
            // if (Input.GetKey(KeyCode.LeftControl)) {
            //     if (Input.GetKey(KeyCode.LeftShift)) Selection.Include(Position+dir, -1);
            //     if (dir == 1) {
            //         for (int i = Position; i < Text.Length; i++) {
            //             amount++;
            //             if (wordBreaks.Contains(Text[i])) {
            //                 break;
            //             }
            //         }
            //     } else if (dir == -1) {
            //         for (int i = Position-1; i >= 0; i--) {
            //             amount++;
            //             if (wordBreaks.Contains(Text[i])) {
            //                 break;
            //             }
            //         }
            //     }
            // } else amount = 1;
            
            MovePosition(dir, amount);
        }

        //Ctrl Keys
        if (Input.GetKey(KeyCode.LeftControl)) {
            //Ctrl + V
            if (Input.GetKeyDown(KeyCode.V)) {
                Paste();
            }

            //Ctrl + C
            if (Input.GetKeyDown(KeyCode.C)) {
                Copy();
            }

            //Ctrl + X
            if (Input.GetKeyDown(KeyCode.X)) {
                Copy();
                BackSpace();
            }
        }

        //add character to string
        if (key != "" && alphabet.Contains(key.ToLower())) {
            if (Selection.IsActive) {
                BackSpace();
            }
            
            if (cap) {
                Text = Text.Insert(Position, key.ToUpper());
                }
            else {
                Text = Text.Insert(Position, key.ToLower());
            }
            Position++;
        }

        //backspace
        if (e.keyCode == KeyCode.Backspace) {
            BackSpace();
        }

        //sumbit command
        if (e.keyCode == KeyCode.Return) {
            Submit();
        }

        //home
        if (e.keyCode == KeyCode.Home) {
            if (Input.GetKey(KeyCode.LeftShift)) Selection.Include(Position-1, -1);
            MovePosition(-1, Position);
        }

        //end
        if (e.keyCode == KeyCode.End) {
            if (Input.GetKey(KeyCode.LeftShift)) Selection.Include(Position, -1);
            MovePosition(1, Text.Length-Position);
        }
    }

    void BackSpace() {
        if (Selection.IsActive) {
            Text = Text.Remove(Selection.Start, Selection.Length+1);
            Position = Selection.Start;
            Selection.Clear();
        } 
        else 
        {
            if (Text.Length > 0 && Position > 0) {
                Position--;
                Text = Text.Remove(Position, 1);
            }
        }
    }

    void Copy() {
        string newClipboard = Text.Substring(Selection.Start, Selection.Length+1);
        if (newClipboard.Length > 0) {
            GUIUtility.systemCopyBuffer = newClipboard;
        }
    }

    void Paste() {
        string clipboard = GUIUtility.systemCopyBuffer;
        Text = Text.Insert(Position, clipboard);
        Position += clipboard.Length;
    }

    void Submit() {
        CommandSubmitted.Invoke(Text);
        Text = "";
        Position = 0;
    }

    void MovePosition(int dir, int amount) {
        if (Position+dir*amount >= 0 && Position+dir*amount <= Text.Length) {
                      
            Position += dir*amount;

            if (Input.GetKey(KeyCode.LeftShift)) {
                Selection.Include(Position, dir);
            } else Selection.Clear();
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
}

public class SelectionRange {
    public int Start {get; private set;} = 0;
    public int End {get; private set;} = 0;
    public int Length {
        get {
            return End-Start;
        }
    }

    public bool IsActive = false;

    public void Include(int pos, int dir) {
        if (dir == 1) {
            pos -= 1;
        }
        if (!IsActive) {
            Start = pos;
            End = pos;
            IsActive = true;
        } else {
            if (pos > End) {
                End = pos;
            } else if (pos < Start) {
                Start = pos;
            }
        }
    }

    public void Clear() {
        IsActive = false;
    }
}
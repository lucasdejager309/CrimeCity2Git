using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public GameObject[] textInputs;

    void Start() {
        textInputs = GameObject.FindGameObjectsWithTag("TextInputUI");
    }

    void OnGUI() {
        Event e = Event.current;
        
        if (e.type == EventType.MouseDown) {
            
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            

            foreach (var i in textInputs) {
                i.GetComponent<TextInputUI>().Focus(false);
            }
            foreach (var r in results) {
                TextInputUI input = null;
                input = r.gameObject.GetComponent<TextInputUI>(); 
                if (input != null) {
                    input.Focus(true);
                    break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    
    void Update()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex){
                popUps[i].SetActive(true);
            }
            else{
                popUps[i].SetActive(false);
            }
        }

        if (popUpIndex == 0) {
            if (popUps.Length == 0) {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) {
                    popUpIndex++;
                }
            } else if (popUps.Length == 1) {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) {
                    popUpIndex++;
                }
            }
        }
    }
}

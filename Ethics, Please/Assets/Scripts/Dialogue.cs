using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueLine {
    [TextArea(10, 40)]
    public string line; // A linha de diálogo
    public GameObject popUp; // O popUp associado (pode ser null)
}

public class Dialogue : MonoBehaviour {
    public TextMeshProUGUI textComponent;
    public List<DialogueLine> dialogueLines; // Lista de linhas de diálogo com popUps associados
    public float textSpeed;

    private int index;

    void Start() {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (textComponent.text == dialogueLines[index].line) {
                NextLine();
            } else {
                StopAllCoroutines();
                textComponent.text = dialogueLines[index].line;
            }
        }
    }

    void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        // Desativa todos os popUps antes de começar a digitar a nova linha
        DeactivateAllPopUps();

        // Ativa o popUp da linha atual (se existir)
        if (dialogueLines[index].popUp != null) {
            dialogueLines[index].popUp.SetActive(true);
        }

        // Digita a linha de diálogo caractere por caractere
        foreach (char c in dialogueLines[index].line.ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() {
        if (index < dialogueLines.Count - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            // Desativa todos os popUps ao finalizar o diálogo
            DeactivateAllPopUps();
            gameObject.SetActive(false);
        }
    }

    void DeactivateAllPopUps() {
        foreach (var dialogueLine in dialogueLines) {
            if (dialogueLine.popUp != null) {
                dialogueLine.popUp.SetActive(false);
            }
        }
    }
}
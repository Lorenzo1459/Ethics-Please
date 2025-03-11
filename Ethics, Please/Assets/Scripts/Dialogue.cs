using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine {
    [TextArea(10, 40)]
    public string line; // A linha de di�logo
    public GameObject popUp; // O popUp associado (pode ser null)
}

public class Dialogue : MonoBehaviour {
    public TextMeshProUGUI textComponent;
    public List<DialogueLine> dialogueLines; // Lista de linhas de di�logo com popUps associados
    private SFXManager sFXManager;
    public float textSpeed;

    //Skip tuto
    private float enterHoldTime = 0f; // Tempo que o jogador segurou a tecla Enter
    private const float holdDuration = 2f; // Tempo necess�rio para pular o tutorial (2 segundos)
    public Image skipTutoImg;


    private int index;

    void Start() {
        textComponent.text = string.Empty;
        sFXManager = FindObjectOfType<SFXManager>();
        StartDialogue();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (textComponent.text == dialogueLines[index].line) {
                NextLine();
            } else {
                StopAllCoroutines();
                sFXManager.StopSFX();
                textComponent.text = dialogueLines[index].line;
            }
        }

        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
            enterHoldTime += Time.deltaTime; // Incrementa o tempo

            if (skipTutoImg != null) {
                skipTutoImg.fillAmount = enterHoldTime / holdDuration;
            }

            // Se o jogador segurou Enter por 3 segundos, pula o tutorial
            if (enterHoldTime >= holdDuration) {
                SkipTutorial();
                return; // Sai do m�todo para evitar conflitos
            }
        } else {
            enterHoldTime = 0f; // Reseta o contador se o jogador soltar a tecla
            if (skipTutoImg != null) {
                skipTutoImg.fillAmount = 0f;
            }
        }
    }

    void SkipTutorial() {
        StopAllCoroutines(); // Para a digita��o do texto
        sFXManager.StopSFX(); // Para o som
        DeactivateAllPopUps(); // Desativa todos os popUps
        skipTutoImg.fillAmount = 0f;
        gameObject.SetActive(false); // Desativa o objeto do tutorial
    }

    void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        // Desativa todos os popUps antes de come�ar a digitar a nova linha
        DeactivateAllPopUps();

        // Ativa o popUp da linha atual (se existir)
        if (dialogueLines[index].popUp != null) {
            dialogueLines[index].popUp.SetActive(true);
        }
        sFXManager.PlaySFXLoop(3);
        // Digita a linha de di�logo caractere por caractere
        foreach (char c in dialogueLines[index].line.ToCharArray()) {
            textComponent.text += c;            
            yield return new WaitForSeconds(textSpeed);
        }
        sFXManager.StopSFX();
    }

    void NextLine() {
        if (index < dialogueLines.Count - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            // Desativa todos os popUps ao finalizar o di�logo
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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEmail : MonoBehaviour {
    public ProposalData proposalData;

    public TMP_Text companyNameText;
    public TMP_Text projectTitleText;
    public TMP_Text projectDescriptionText;

    private Image background; // Referência para o fundo do e-mail
    private Color defaultColor; // Para armazenar a cor original

    void Awake() {
        background = GetComponent<Image>(); // Pegamos a imagem do fundo
        defaultColor = background.color; // Salvamos a cor original
    }

    // Start is called before the first frame update
    void Start() {
        companyNameText.text = proposalData.companyName;
        projectTitleText.text = proposalData.projectTitle;
        projectDescriptionText.text = proposalData.projectDescription.Replace("\\n", "\n");
        projectDescriptionText.text = proposalData.projectDescription;

    }


    public void AcceptEmail() {
        if (proposalData.hasEthicalIssue) {
            Debug.Log("Você aceitou um e-mail problemático! Isso terá consequências...");
            bool isCorrect = !proposalData.hasEthicalIssue;
            StartCoroutine(FlashColor(isCorrect));
            // Adicione penalizações aqui (exemplo: diminuir reputação)
        } else {
            Debug.Log("Bom trabalho! Você aceitou um e-mail ético.");
            bool isCorrect = !proposalData.hasEthicalIssue;
            StartCoroutine(FlashColor(isCorrect));
            // Adicione recompensas aqui (exemplo: ganhar pontos de integridade)
        }

        //NextEmail();
    }

    public void RejectEmail() {
        if (proposalData.hasEthicalIssue) {
            Debug.Log("Você corretamente rejeitou um e-mail problemático!");
            bool isCorrect = proposalData.hasEthicalIssue;
            StartCoroutine(FlashColor(isCorrect));
            // Recompensa por detectar problemas éticos
        } else {
            Debug.Log("Você rejeitou um e-mail legítimo...");
            bool isCorrect = proposalData.hasEthicalIssue;
            StartCoroutine(FlashColor(isCorrect));
            // Penalização por rejeitar propostas boas
        }

        //NextEmail();
    }


    private IEnumerator FlashColor(bool isCorrect) {
        background.color = isCorrect ? Color.green : Color.red;
        yield return new WaitForSeconds(0.5f); // Espera meio segundo
        background.color = defaultColor; // Retorna à cor original
    }
}

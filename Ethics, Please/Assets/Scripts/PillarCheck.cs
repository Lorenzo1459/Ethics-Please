using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Importa��o necess�ria para lidar com TMP_InputField

public class PillarCheck : MonoBehaviour {
    public DisplayEmail displayEmail;
    public EmailManager emailManager;
    private ProposalData emailData;
    private ScoreManager scoreManager;

    public TMP_InputField emailInputField; // Refer�ncia ao campo de texto onde o e-mail aparece
    public float tolerance = 0.5f; // Ajuste para flexibilidade da sele��o

    private void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();
        emailManager = FindObjectOfType<EmailManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void CheckEthicalIssue(TipoProblema tipoProblema) {
        emailData = displayEmail.currentProposal;

        if (emailData == null) {
            Debug.LogWarning("Nenhum e-mail carregado.");
            return;
        }

        string selectedText = GetSelectedText();
        if (string.IsNullOrEmpty(selectedText)) {
            Debug.LogWarning("Nenhum texto selecionado!");
            return;
        }

        bool isSelectionCorrect = false;

        foreach (string problem in emailData.trechoAntietico) {
            if (IsSimilar(selectedText.ToLower(), problem.ToLower())) {
                isSelectionCorrect = true;
                break;
            }
        }

        bool acertouTipoProblema = emailData.tipoProblema == tipoProblema;
        int pontos = 0;
        Color feedbackColor = Color.red;
        string resultado = "Incorreto";

        if (isSelectionCorrect && acertouTipoProblema) {
            Debug.Log("Esse e-mail realmente tem problema �tico " + tipoProblema + " e a sele��o foi correta!");
            pontos = 30;
            feedbackColor = Color.green;
            resultado = "Correto";
        }
        else if (!isSelectionCorrect && acertouTipoProblema){
            Debug.Log("H� problema �tico. Acertou o pilar, mas errou o trecho");
            pontos = 15;
            feedbackColor = Color.yellow;
            resultado = "Acertou o pilar, errou o trecho.";
        }
        else if (isSelectionCorrect && !acertouTipoProblema) {
            Debug.Log("H� problema �tico. Acertou o trecho, mas errou o pilar");
            pontos = 15;
            feedbackColor = Color.yellow;
            resultado = "Acertou o trecho, mas errou o pilar.";
        } else if (emailData.hasEthicalIssue) {
            Debug.Log("H� problema �tico, mas n�o nesse trecho e nem nesse pilar.");
            pontos = 15;
            feedbackColor = Color.yellow;
            resultado = "H� problema �tico, mas n�o nesse trecho e nem nesse pilar.";
        } else {
            Debug.Log("Esse e-mail n�o tem problema �tico!");
            pontos = -5;
            resultado = "O email n�o cont�m problema �tico.";
        }

        scoreManager.AddScore(pontos);
        StartCoroutine(displayEmail.FlashColor(feedbackColor));
        emailManager.UpdateHistoryUI();

        // **Registrar no hist�rico**
        FindObjectOfType<EmailManager>().RegisterDecision(selectedText, tipoProblema, resultado);

        StartCoroutine(displayEmail.CloseEmail());
    }

    private string GetSelectedText() {
        if (emailInputField == null) return "";

        int start = emailInputField.stringPosition;
        int end = emailInputField.selectionStringAnchorPosition;

        if (start > end) {
            int temp = start;
            start = end;
            end = temp;
        }

        if (start == end) return ""; // Nada selecionado

        return emailInputField.text.Substring(start, end - start);
    }

    private bool IsSimilar(string selected, string problem) {
        if (problem.Contains(selected)) return true; // Se o trecho exato estiver dentro do problema, j� conta.

        int maxMatchingChars = 0;

        for (int i = 0; i <= problem.Length - selected.Length; i++) {
            int matchingChars = 0;
            for (int j = 0; j < selected.Length; j++) {
                if (problem[i + j] == selected[j]) matchingChars++;
            }

            maxMatchingChars = Mathf.Max(maxMatchingChars, matchingChars);
        }

        float similarity = (float)maxMatchingChars / selected.Length;
        return similarity >= tolerance;
    }

    public void FalibilidadeCheck() {
        CheckEthicalIssue(TipoProblema.Falibilidade);
    }

    public void OpacidadeCheck() {
        CheckEthicalIssue(TipoProblema.Opacidade);
    }

    public void ViesCheck() {
        CheckEthicalIssue(TipoProblema.Vies);
    }

    public void DiscriminacaoCheck() {
        CheckEthicalIssue(TipoProblema.Discriminacao);
    }

    public void AutonomiaCheck() {
        CheckEthicalIssue(TipoProblema.Autonomia);
    }

    public void PrivacidadeCheck() {
        CheckEthicalIssue(TipoProblema.Privacidade);
    }

    public void ResponsabilidadeCheck() {
        CheckEthicalIssue(TipoProblema.Responsabilidade);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PillarCheck : MonoBehaviour {
    private DisplayEmail displayEmail;
    private EmailManager emailManager;
    private SFXManager sFXManager;
    private ProposalData emailData;
    private ScoreManager scoreManager;
    public GameObject rulebookPanel;

    public TMP_InputField emailInputField; // Referência ao campo de texto onde o e-mail aparece
    public float tolerance = 0.5f; // Ajuste para flexibilidade da seleção   

    private void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();
        emailManager = FindObjectOfType<EmailManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        sFXManager = FindObjectOfType<SFXManager>();

        if (rulebookPanel != null) {
            rulebookPanel.SetActive(true); // Ativa o painel
            rulebookPanel.SetActive(false); // Desativa o painel
        }
    }

    public void ToggleRulebookPanel() {
        if (rulebookPanel != null) {
            rulebookPanel.SetActive(!rulebookPanel.activeSelf);
        }
    }

    public void CheckEthicalIssue(TipoProblema tipoProblema) {
        emailData = displayEmail.currentProposal;
        int totalLength = emailData.trechoAntietico.Sum(p => p.Length);
        float averageProblemSize = (float)totalLength / emailData.trechoAntietico.Count;
        float minAllowedSelectionSize = averageProblemSize * 0.5f;
        float maxAllowedSelectionSize = averageProblemSize * 2.5f;

        if (emailData == null) {
            Debug.LogWarning("Nenhum e-mail carregado.");
            return;
        }

        string selectedText = GetSelectedText();
        if (string.IsNullOrEmpty(selectedText)) {
            Debug.LogWarning("Nenhum texto selecionado!");
            return;
        }

        if (selectedText.Length > maxAllowedSelectionSize) { // Verifica se a seleção é mt grande
            Debug.Log("Seleção muito grande! Tente selecionar um trecho mais específico.");
            displayEmail.CallResultFeedback("Tente novamente", "Selecao grande");
            return;
        }

        if (selectedText.Length < minAllowedSelectionSize) {
            Debug.Log("Seleção muito pequena! Tente selecionar um trecho mais completo.");
            displayEmail.CallResultFeedback("Tente novamente", "Selecao pequena");
            return;
        }

        bool isSelectionCorrect = false;

        foreach (string problem in emailData.trechoAntietico) {
            if (IsSimilar(selectedText.ToLower(), emailData.trechoAntietico.Select(p => p.ToLower()).ToList())) {
                isSelectionCorrect = true;
                break;
            }
        }

        bool acertouTipoProblema = emailData.tipoProblema == tipoProblema;
        int pontos = 0;
        Color feedbackColor = Color.red;
        string resultado = "Incorreto";

        if (isSelectionCorrect && acertouTipoProblema) {
            Debug.Log("Esse e-mail realmente tem problema ético " + tipoProblema + " e a seleção foi correta!");
            pontos = 30;
            feedbackColor = Color.green;
            resultado = "Correto";
            sFXManager.PlaySFX(1); // 1 - Certo
            displayEmail.CallResultFeedback("Correto", "Acertou ambos");
        } else if (!isSelectionCorrect && acertouTipoProblema) {
            Debug.Log("Há problema ético. Acertou o pilar, mas errou o trecho");
            pontos = 15;
            feedbackColor = Color.yellow;
            displayEmail.CallResultFeedback("Quase", "Errou trecho");
            resultado = "Acertou o pilar, errou o trecho.";
        } else if (isSelectionCorrect && !acertouTipoProblema) {
            Debug.Log("Há problema ético. Acertou o trecho, mas errou o pilar");
            pontos = 15;
            feedbackColor = Color.yellow;
            displayEmail.CallResultFeedback("Quase", "Errou pilar");
            resultado = "Acertou o trecho, mas errou o pilar.";
        } else if (emailData.hasEthicalIssue) {
            Debug.Log("Há problema ético, mas não nesse trecho e nem nesse pilar.");
            pontos = 15;
            feedbackColor = Color.yellow;
            displayEmail.CallResultFeedback("Quase", "Errou ambos");
            resultado = "Há problema ético, mas não nesse trecho e nem nesse pilar.";
        } else {
            Debug.Log("Esse e-mail não tem problema ético!");
            pontos = -5;
            sFXManager.PlaySFX(2); // 2 - Errado
            displayEmail.CallResultFeedback("Errado", "Rejeitou etico");
            resultado = "O email não contém problema ético.";
        }

        scoreManager.AddScore(pontos);
        StartCoroutine(displayEmail.FlashColor(feedbackColor));
        emailManager.SaveEmailToHistory(feedbackColor);

        // **Registrar no histórico**
        FindObjectOfType<EmailManager>().RegisterDecision(selectedText, tipoProblema, resultado);

        StartCoroutine(displayEmail.CloseEmail(1.5f));
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

    /*private bool IsSimilar(string selected, string problem) {
        if (problem.Contains(selected)) return true; // Se o trecho exato estiver dentro do problema, j? conta.

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
    }*/

    private bool IsSimilar(string selected, List<string> problems) {
        foreach (string problem in problems) {
            if (selected.Contains(problem)) return true; // Se contém exatamente um dos problemas, já aceita.

            int matchingChars = 0;
            int minLength = Mathf.Min(selected.Length, problem.Length);

            for (int i = 0; i < minLength; i++) {
                if (selected[i] == problem[i]) matchingChars++;
            }

            float penalty = (float)problem.Length / selected.Length;
            float similarity = ((float)matchingChars / problem.Length) * penalty;

            if (similarity >= tolerance) return true;
        }

        return false;
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
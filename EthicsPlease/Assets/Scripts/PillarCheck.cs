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

    private bool isChecking = false;
    private bool jaVerificouEsseEmail = false;
    private float checkCooldown = 1.6f;

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
        if (isChecking || jaVerificouEsseEmail) return;
        isChecking = true;
        StartCoroutine(ResetCheckCooldown());

        emailData = displayEmail.currentProposal;
        if (emailData == null) {
            Debug.LogWarning("Nenhum e-mail carregado.");
            return;
        }

        string selectedText = GetSelectedText();
        bool hasSelection = !string.IsNullOrEmpty(selectedText);

        int pontos = 0;
        Color feedbackColor = Color.red;
        string resultado = "Incorreto";

        if (hasSelection) {
            // **Caso 1: Com seleção de texto**
            int totalLength = emailData.trechoAntietico.Sum(p => p.Length);
            float averageProblemSize = (float)totalLength / emailData.trechoAntietico.Count;
            float minAllowedSelectionSize = averageProblemSize * 0.5f;
            float maxAllowedSelectionSize = averageProblemSize * 2.5f;

            if (selectedText.Length > maxAllowedSelectionSize) {
                Debug.Log("Seleção muito grande! Tente um trecho mais específico.");
                displayEmail.CallResultFeedback("Tente novamente", "Selecao grande");
                isChecking = false;
                return;
            }

            if (selectedText.Length < minAllowedSelectionSize) {
                Debug.Log("Seleção muito pequena! Tente um trecho mais completo.");
                displayEmail.CallResultFeedback("Tente novamente", "Selecao pequena");
                isChecking = false;
                return;
            }

            bool isSelectionCorrect = IsSimilar(selectedText.ToLower(), emailData.trechoAntietico.Select(p => p.ToLower()).ToList());
            bool acertouTipoProblema = emailData.tipoProblema == tipoProblema;

            if (isSelectionCorrect && acertouTipoProblema) {
                pontos = 60; // Máximo de pontos por acertar tudo
                scoreManager.RegistrarEmailRejeitado(true);
                scoreManager.RegistrarPilarCorreto();
                feedbackColor = Color.green;
                resultado = "Correto";
                sFXManager.PlaySFX(1);
                displayEmail.CallResultFeedback("Correto", "Acertou ambos");
            } else if (!isSelectionCorrect && acertouTipoProblema) {
                pontos = 40; // Acertou o pilar, mas não o trecho
                scoreManager.RegistrarEmailRejeitado(true);
                scoreManager.RegistrarPilarCorreto();
                feedbackColor = Color.yellow;
                resultado = "Acertou o pilar, errou o trecho.";
                displayEmail.CallResultFeedback("Quase", "Errou trecho");
            } else if (isSelectionCorrect && !acertouTipoProblema) {
                pontos = 25; // Acertou o trecho, mas errou o pilar
                scoreManager.RegistrarEmailRejeitado(true);
                feedbackColor = Color.yellow;
                resultado = "Acertou o trecho, mas errou o pilar.";
                displayEmail.CallResultFeedback("Quase", "Errou pilar");
            } else if (emailData.hasEthicalIssue) {
                pontos = 15; // Pelo menos percebeu que havia um problema
                scoreManager.RegistrarEmailRejeitado(true);
                feedbackColor = Color.yellow;
                resultado = "Há problema ético, mas não nesse trecho e nem nesse pilar.";
                displayEmail.CallResultFeedback("Quase", "Errou ambos");
            } else {
                pontos = -15; // O email era ético, mas foi marcado como problemático
                scoreManager.RegistrarEmailRejeitado(false);
                scoreManager.RegistrarPenalizacao();
                sFXManager.PlaySFX(2);
                displayEmail.CallResultFeedback("Errado", "Rejeitou etico");
                resultado = "O email não contém problema ético.";
            }
        } else {
            // **Caso 2: Sem seleção de texto**
            bool acertouTipoProblema = emailData.tipoProblema == tipoProblema;

            if (acertouTipoProblema) {
                pontos = 50; // Acertou só o pilar sem selecionar texto
                scoreManager.RegistrarEmailRejeitado(true);
                scoreManager.RegistrarPilarCorreto();
                feedbackColor = Color.green;
                resultado = "Acertou apenas o pilar.";
                sFXManager.PlaySFX(1);
                displayEmail.CallResultFeedback("Correto", "Acertou sem selecao");
            } else if (emailData.hasEthicalIssue) {
                pontos = 25; // Pelo menos percebeu que havia um problema
                scoreManager.RegistrarEmailRejeitado(true);                
                feedbackColor = Color.yellow;
                resultado = "Há problema ético, mas errou o pilar.";
                displayEmail.CallResultFeedback("Quase", "Errou pilar sem selecao");
            } else {
                pontos = -15; // O email era ético e foi marcado como problemático
                scoreManager.RegistrarEmailRejeitado(false);
                scoreManager.RegistrarPenalizacao();
                sFXManager.PlaySFX(2);
                displayEmail.CallResultFeedback("Errado", "Rejeitou etico");
                resultado = "O email não contém problema ético.";
            }
        }

        jaVerificouEsseEmail = true;
        scoreManager.AddScore(pontos);
        StartCoroutine(displayEmail.FlashColor(feedbackColor));
        emailManager.SaveEmailToHistory(feedbackColor);
        emailManager.RegisterDecision(selectedText, tipoProblema, resultado);
        StartCoroutine(displayEmail.CloseEmail(1.5f));
    }

    public void NovoEmailCarregado() {
        jaVerificouEsseEmail = false;

        // Reseta a seleção do input field
        if (emailInputField != null) {
            // Força a limpeza da seleção
            emailInputField.selectionAnchorPosition = 0;
            emailInputField.selectionFocusPosition = 0;
            emailInputField.caretPosition = 0;

            // Remove o foco do campo de texto, se estiver focado
            if (emailInputField.isFocused) {
                emailInputField.DeactivateInputField();
            }

            // Força a atualização do campo de texto
            emailInputField.ForceLabelUpdate();

            // Reativa o inputField para permitir interação
            emailInputField.ActivateInputField();
        }
    }

    private IEnumerator ResetCheckCooldown() {
        yield return new WaitForSeconds(checkCooldown);
        isChecking = false;
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
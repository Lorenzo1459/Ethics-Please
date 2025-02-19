using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmailManager : MonoBehaviour {
    public ProposalDatabase proposalDatabase;
    public TMP_Text emailCountText; // Referência ao número de e-mails no botão

    private Queue<int> emailQueue = new Queue<int>();
    private List<int> indexList = new List<int>();
    private List<EmailHistoryEntry> emailHistory = new List<EmailHistoryEntry>(); // Histórico de e-mails

    private int emailCount = 0; // Número de e-mails disponíveis
    private float emailCooldown = 10f; // Tempo base entre e-mails
    private int historyIndex = -1; // Índice do histórico

    private DisplayEmail displayEmail;
    private ProposalData currentEmailData;

    void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();

        for (int i = 0; i < proposalDatabase.proposals.Count; i++) {
            indexList.Add(i);
        }

        ShuffleEmailIndices();
        AddEmailsToQueue();

        StartCoroutine(DelayedFirstEmail(3f));
        StartCoroutine(GenerateEmailsOverTime());
    }

    private void ShuffleEmailIndices() {
        for (int i = 0; i < indexList.Count; i++) {
            int tmp = indexList[i];
            int r = Random.Range(i, indexList.Count);
            indexList[i] = indexList[r];
            indexList[r] = tmp;
        }
    }

    private void AddEmailsToQueue() {
        foreach (int index in indexList) {
            emailQueue.Enqueue(index);
        }
    }

    private IEnumerator GenerateEmailsOverTime() {
        while (true) {
            yield return new WaitForSeconds(emailCooldown);
            if (emailQueue.Count > 0) {
                emailCount++;
                UpdateEmailCountUI();
            }
        }
    }

    private void UpdateEmailCountUI() {
        if (emailCountText != null) {
            Transform parentObject = emailCountText.transform.parent;

            if (emailCount > 0) {
                emailCountText.text = $"📩 {emailCount}";
                if (parentObject != null) parentObject.gameObject.SetActive(true);
            } else {
                if (parentObject != null) parentObject.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator DelayedFirstEmail(float delay) {
        yield return new WaitForSeconds(delay);
        emailCount++;
        UpdateEmailCountUI();
    }

    public void ShowNextEmail() {
        if (currentEmailData != null) {
            // Salva o e-mail atual no histórico antes de mostrar o novo
            emailHistory.Add(new EmailHistoryEntry(
                currentEmailData.projectDescription,
                "", // Ainda sem seleção
                currentEmailData.tipoProblema,
                TipoProblema.Nenhum, // Ainda sem resposta
                "Pendente"
            ));
        }

        if (emailCount > 0 && emailQueue.Count > 0) {
            int nextEmailIndex = emailQueue.Dequeue();
            currentEmailData = proposalDatabase.proposals[nextEmailIndex]; // Atualiza o e-mail atual
            displayEmail.DisplayEmailByIndex(nextEmailIndex);
            emailCount--;
            UpdateEmailCountUI();
            historyIndex = emailHistory.Count - 1; // Atualiza índice do histórico
        } else {
            Debug.Log("Nenhum e-mail disponível no momento.");
        }
    }

    public void ShowPreviousHistoryEmail() {
        if (historyIndex > 0) {
            historyIndex--;
            displayEmail.DisplayEmailData(emailHistory[historyIndex].proposal);
        }
    }

    public void ShowNextHistoryEmail() {
        if (historyIndex < emailHistory.Count - 1) {
            historyIndex++;
            displayEmail.DisplayEmailData(emailHistory[historyIndex].proposal);
        }
    }

    // Função chamada quando o jogador toma uma decisão
    public void RegisterDecision(string selectedText, TipoProblema chosenProblem, string result) {
        if (currentEmailData == null) return;

        emailHistory[emailHistory.Count - 1] = new EmailHistoryEntry(
            currentEmailData.projectDescription,
            selectedText,
            currentEmailData.tipoProblema,
            chosenProblem,
            result
        );
    }

    // Exibir histórico no console (para debug)
    public void ShowHistory() {
        Debug.Log("=== HISTÓRICO DE E-MAILS ===");
        foreach (var entry in emailHistory) {
            Debug.Log($"E-mail: {entry.emailText}\n" +
                      $"Trecho Selecionado: {entry.selectedText}\n" +
                      $"Problema Correto: {entry.correctProblem}\n" +
                      $"Problema Escolhido: {entry.chosenProblem}\n" +
                      $"Resultado: {entry.result}\n");
        }
    }
    public List<EmailHistoryEntry> GetEmailHistory() {
        return emailHistory;
    }
}

// Classe que representa um e-mail armazenado no histórico
public class EmailHistoryEntry {
    public string emailText;
    public string selectedText;
    public TipoProblema correctProblem;
    public TipoProblema chosenProblem;
    public string result;
    public ProposalData proposal; // Para recuperar o e-mail completo

    public EmailHistoryEntry(string emailText, string selectedText, TipoProblema correctProblem, TipoProblema chosenProblem, string result) {
        this.emailText = emailText;
        this.selectedText = selectedText;
        this.correctProblem = correctProblem;
        this.chosenProblem = chosenProblem;
        this.result = result;
    }
}

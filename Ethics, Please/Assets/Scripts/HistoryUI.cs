using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistoryUI : MonoBehaviour {
    public GameObject historyPanel; // Painel do histórico
    public Transform historyListContent; // Local onde os botões dos e-mails serão listados
    public Button emailButtonPrefab; // Prefab de botão para cada e-mail
    public Button openHistoryButton; // Botão para abrir o histórico
    public Button closeHistoryButton; // Botão para fechar
    public Button prevEmailButton; // Botão para voltar no histórico
    public Button nextEmailButton; // Botão para avançar no histórico
    public DisplayEmail displayEmail; // Referência ao DisplayEmail

    private EmailManager emailManager;
    private int currentHistoryIndex = -1;

    void Start() {
        emailManager = FindObjectOfType<EmailManager>();

        // Configurar os botões
        openHistoryButton.onClick.AddListener(OpenHistory);
        closeHistoryButton.onClick.AddListener(CloseHistory);
        prevEmailButton.onClick.AddListener(ShowPreviousEmail);
        nextEmailButton.onClick.AddListener(ShowNextEmail);

        UpdateHistoryList();
        historyPanel.SetActive(false);
    }

    public void OpenHistory() {
        UpdateHistoryList();
        historyPanel.SetActive(true);
    }

    public void CloseHistory() {
        historyPanel.SetActive(false);
    }

    private void UpdateHistoryList() {
        foreach (Transform child in historyListContent) {
            Destroy(child.gameObject); // Limpa os botões antigos
        }

        List<EmailHistoryEntry> emailHistory = emailManager.GetEmailHistory();

        for (int i = 0; i < emailHistory.Count; i++) {
            int index = i;
            Button emailButton = Instantiate(emailButtonPrefab, historyListContent);
            emailButton.GetComponentInChildren<TMP_Text>().text = $"E-mail {i + 1}: {emailHistory[i].emailText.Substring(0, Mathf.Min(30, emailHistory[i].emailText.Length))}...";
            emailButton.onClick.AddListener(() => ShowEmailByIndex(index));
        }
    }

    private void ShowEmailByIndex(int index) {
        List<EmailHistoryEntry> emailHistory = emailManager.GetEmailHistory();
        if (index >= 0 && index < emailHistory.Count) {
            displayEmail.DisplayHistoryEmail(emailHistory[index]);
            currentHistoryIndex = index;
        }
    }

    private void ShowPreviousEmail() {
        if (currentHistoryIndex > 0) {
            currentHistoryIndex--;
            ShowEmailByIndex(currentHistoryIndex);
        }
    }

    private void ShowNextEmail() {
        List<EmailHistoryEntry> emailHistory = emailManager.GetEmailHistory();
        if (currentHistoryIndex < emailHistory.Count - 1) {
            currentHistoryIndex++;
            ShowEmailByIndex(currentHistoryIndex);
        }
    }
}

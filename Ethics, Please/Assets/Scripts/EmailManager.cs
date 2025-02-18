using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmailManager : MonoBehaviour {
    public ProposalDatabase proposalDatabase;
    public TMP_Text emailCountText; // Referência ao número de e-mails no botão

    private Queue<int> emailQueue = new Queue<int>();
    private List<int> indexList = new List<int>();
    private int emailCount = 0; // Número de e-mails disponíveis
    private float emailTimer = 10f; // Tempo para um novo e-mail chegar
    private float emailCooldown = 10f; // Tempo base entre e-mails

    private DisplayEmail displayEmail;

    void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();

        for (int i = 0; i < proposalDatabase.proposals.Count; i++) {
            indexList.Add(i);
        }

        ShuffleEmailIndices();
        AddEmailsToQueue();

        StartCoroutine(DelayedFirstEmail(3f));
        StartCoroutine(GenerateEmailsOverTime()); // Começa a gerar e-mails automaticamente
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
                emailCount++; // Um novo e-mail chegou
                UpdateEmailCountUI();
            }
        }
    }

    private void UpdateEmailCountUI() {
        if (emailCountText != null) {
            Transform parentObject = emailCountText.transform.parent; // Get the parent (which holds the background)

            if (emailCount > 0) {
                emailCountText.text = $"📩 {emailCount}";
                if (parentObject != null) parentObject.gameObject.SetActive(true); // Show parent (background + text)
            } else {
                if (parentObject != null) parentObject.gameObject.SetActive(false); // Hide parent when no emails
            }
        }
    }

    IEnumerator DelayedFirstEmail(float delay) {
        yield return new WaitForSeconds(delay); // Wait for 3 seconds
        emailCount++; // First email arrives
        UpdateEmailCountUI(); // Update UI
    }

    public void ShowNextEmail() {
        if (emailCount > 0 && emailQueue.Count > 0) {
            int nextEmailIndex = emailQueue.Dequeue();
            displayEmail.DisplayEmailByIndex(nextEmailIndex);
            emailCount--; // Consumiu um e-mail
            UpdateEmailCountUI();
        } else {
            Debug.Log("Nenhum e-mail disponível no momento.");
        }
    }
}

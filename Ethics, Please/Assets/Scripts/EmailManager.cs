using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailManager : MonoBehaviour {
    public ProposalDatabase proposalDatabase;
    private Queue<int> emailQueue = new Queue<int>();
    private List<int> indexList = new List<int>();

    private DisplayEmail displayEmail;
    
    void Start() {
        displayEmail = FindObjectOfType<DisplayEmail>();

        for (int i = 0; i < proposalDatabase.proposals.Count; i++) {
            indexList.Add(i);
        }

        ShuffleEmailIndices();        
        AddEmailsToQueue();
        ShowNextEmail();
        Debug.Log("EmailManager initialized.");
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

    public void ShowNextEmail() {
        if (emailQueue.Count > 0) {
            int nextEmailIndex = emailQueue.Dequeue();
            displayEmail.DisplayEmailByIndex(nextEmailIndex);
        } else {
            Debug.Log("Todos os e-mails foram exibidos. Reiniciando a fila...");
            ShuffleEmailIndices();
            AddEmailsToQueue();
            ShowNextEmail();
        }
    }

    public void RequestNextEmail(float delay = 1.5f) {
        StartCoroutine(WaitForNextEmail(delay));
    }

    private IEnumerator WaitForNextEmail(float time) {
        yield return new WaitForSeconds(time);
        ShowNextEmail();
    }
}

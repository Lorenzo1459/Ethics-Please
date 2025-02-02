using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEmail : MonoBehaviour {
    public ProposalDatabase proposalDatabase;
    private List<int> indexList = new List<int>();
    private int currentEmailIndex = -1;

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
        for (int i = 0; i < proposalDatabase.proposals.Count; i++) {
            indexList.Add(i);
        }
        ShuffleEmailIndices();

        NextEmail();
    }

    void DisplayEmails(int index) {
        if (index >= 0 && index < proposalDatabase.proposals.Count) {
            ProposalData proposal = proposalDatabase.proposals[index];
            companyNameText.text = proposal.companyName;
            projectTitleText.text = proposal.projectTitle;
            projectDescriptionText.text = proposal.projectDescription.Replace("\\n", "\n");
            projectDescriptionText.text = proposal.projectDescription;
        }
    }


    public void AcceptEmail() {
        ProposalData proposalData = proposalDatabase.proposals[currentEmailIndex];
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

        NextEmail();
    }

    public void RejectEmail() {
        ProposalData proposalData = proposalDatabase.proposals[currentEmailIndex];
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

        NextEmail();
    }

    void ShuffleEmailIndices() {
        for (int i = 0; i < indexList.Count; i++) {
            int tmp = indexList[i];
            int r = Random.Range(i, indexList.Count);
            indexList[i] = indexList[r];
            indexList[r] = tmp;
        }
    }

    public void NextEmail() {
        StartCoroutine(WaitForNextEmail(1.5f));
    }

    IEnumerator WaitForNextEmail(float time) {

        yield return new WaitForSeconds(time); // Espera dois segundos

        if (indexList.Count > 0) {
            // Select a random index from the available proposals
            int randomIndex = Random.Range(0, indexList.Count);
            currentEmailIndex = indexList[randomIndex];

            // Display the selected proposal
            DisplayEmails(currentEmailIndex);

            // Remove the selected index to avoid repetition
            indexList.RemoveAt(randomIndex);
        } else {
            // If all proposals have been shown, restart the process
            Debug.Log("All proposals have been displayed!");
            indexList = new List<int>();
            for (int i = 0; i < proposalDatabase.proposals.Count; i++) {
                indexList.Add(i);
            }

            // Shuffle the list of proposals again
            ShuffleEmailIndices();

            // Show the next proposal
            NextEmail();
        }
    }

    private IEnumerator FlashColor(bool isCorrect) {
        background.color = isCorrect ? Color.green : Color.red;
        yield return new WaitForSeconds(1.5f); // Espera 1.5s
        background.color = defaultColor; // Retorna à cor original
    }
}

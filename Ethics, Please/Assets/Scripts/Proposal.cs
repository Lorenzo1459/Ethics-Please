using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Proposal : MonoBehaviour {
    public string proposalName;
    public string proposalDescription;
    public bool hasEthicalIssue;  // Indicates if the proposal has ethical issues

    public TextMeshProUGUI proposalText;  // Reference to the TextMeshPro text that will be displayed

    // Method to configure the details of the proposal
    public void ConfigureProposal(string name, string description, bool ethicalIssue) {
        proposalName = name;
        proposalDescription = description;
        hasEthicalIssue = ethicalIssue;

        proposalText.text = proposalName + "\n" + proposalDescription;
    }
}


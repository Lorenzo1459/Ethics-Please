using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Decision : MonoBehaviour {
    public Proposal currentProposal;       // Reference to the current proposal
    public GameObject feedbackPanel;      // Reference to the feedback panel
    public GameObject proposalPanel;      // Reference to the proposal panel
    public TextMeshProUGUI feedbackText;  // TextMeshPro text for the feedback message
    public GameManager gameManager;       // Reference to the GameManager script

    private float feedbackDelay = 2f;     // Delay before transitioning to the next turn

    // Method called when the player accepts the proposal
    public void AcceptProposal() {
        if (currentProposal.hasEthicalIssue) {
            ShowFeedback("You accepted a proposal with ethical issues!");
        } else {
            ShowFeedback("Proposal accepted successfully!");
        }
    }

    // Method called when the player rejects the proposal
    public void RejectProposal() {
        if (currentProposal.hasEthicalIssue) {
            ShowFeedback("Proposal rejected.");
        } else {
            ShowFeedback("You rejected a proposal without ethical issues.");
        }
    }

    // Displays feedback, hides the proposal panel, and schedules the next turn
    private void ShowFeedback(string message) {
        feedbackPanel.SetActive(true);    // Show feedback panel
        feedbackText.text = message;     // Set feedback message
        proposalPanel.SetActive(false);  // Hide proposal panel

        // Call EndTurn in the GameManager after a delay
        Invoke("EndTurn", feedbackDelay);
    }

    // Calls the EndTurn method in the GameManager
    private void EndTurn() {
        gameManager.EndTurn();
    }
}



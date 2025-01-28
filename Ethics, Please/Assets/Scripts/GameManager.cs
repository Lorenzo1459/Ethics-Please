using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Proposal currentProposal; // Active proposal
    public GameObject proposalPanel;
    public GameObject feedbackPanel;
    public Decision decisionManager; // Reference to the Decision script

    private void Start() {
        GenerateProposal();
    }

    public void GenerateProposal() {
        // Assume ProposalPanel is already set up with a Proposal script attached.
        currentProposal = proposalPanel.GetComponent<Proposal>();

        bool hasIssue = Random.Range(0, 2) == 0;  // Randomized ethical issue
        currentProposal.ConfigureProposal(
            "Facial Recognition App",
            "This app collects facial data for school security.",
            hasIssue
        );

        proposalPanel.SetActive(true);
        feedbackPanel.SetActive(false);  // Hide feedback until player acts
    }

    /*
     public void GenerateProposal()
    {
        // Randomly pick a proposal from the database
        if (proposalDatabase.proposals.Count > 0)
        {
            int randomIndex = Random.Range(0, proposalDatabase.proposals.Count);
            ProposalData selectedProposal = proposalDatabase.proposals[randomIndex];

            // Configure the UI with the selected proposal
            currentProposal.ConfigureProposal(selectedProposal.proposalName, 
                                              selectedProposal.description, 
                                              selectedProposal.hasEthicalIssue);

            proposalPanel.SetActive(true);
            feedbackPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Proposal database is empty!");
        }
    }
     */

    public void EndTurn() {
        feedbackPanel.SetActive(false);    // Hide feedback panel
        proposalPanel.SetActive(true);     // Show proposal panel
        GenerateProposal();                // Generate a new proposal
    }

}


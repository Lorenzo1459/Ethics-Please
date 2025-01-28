using UnityEngine;

public enum ProposalProblem {
    Falibilidade,
    Opacidade,
    Vies,
    Discriminacao,
    Autonomia,
    Privacidade,
    Responsabilidade,
    Nenhum
}


[CreateAssetMenu(fileName = "NewProposal", menuName = "Proposals/Proposal")]
public class ProposalData : ScriptableObject {
    public string proposalName;
    public string description;
    public bool hasEthicalIssue; // True if the proposal has an ethical issue
    public ProposalProblem problema;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProposalDatabase", menuName = "Proposals/Proposal Database")]
public class ProposalDatabase : ScriptableObject {
    public List<ProposalData> proposals;
}

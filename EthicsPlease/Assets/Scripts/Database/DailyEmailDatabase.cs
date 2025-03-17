using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyEmailDatabase", menuName = "DailyEmail/DailyEmail Database")]
public class DailyEmailDatabase : ScriptableObject {
    public List<DailyEmailData> dailyemails;
}

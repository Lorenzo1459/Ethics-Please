using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewDailyEmail", menuName = "Emails/DailyEmail")]
public class DailyEmailData : ScriptableObject {
    public string companyName;
    public string projectTitle;
    [TextArea(10,40)]
    public string projectDescription;          
}

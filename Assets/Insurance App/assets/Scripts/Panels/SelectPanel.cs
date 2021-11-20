using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public Text nameText;
    public Text dateText;

    //onenable 
    public void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
        nameText.text = "NAME: " + UIManager.Instance.activeCase.name;
        dateText.text = "DATE OF INCIDENT: " + UIManager.Instance.activeCase.date;
        UIManager.Instance.currentPanel = "selectPanel";
    }
    //populate the case name

    public void ProcessInfo()
    {
        UIManager.Instance.currentPanel = "overviewPanelSelect";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SearchPanel : MonoBehaviour, IPanel
{
    public InputField caseNumberInput;
    public SelectPanel selectPanel;
    public GameObject loadingSpinner;

    public void OnEnable()
    {
       //Scene searchPanelScene = SceneManager.CreateScene("searchPanelScene");
        UIManager.Instance.currentPanel = "searchPanel";
    }
   

    public void ProcessInfo()
    {
        //enables functionality of list download
        loadingSpinner.gameObject.SetActive(true);

        AWSManager.Instance.GetList(caseNumberInput.text, () =>
        {
            selectPanel.gameObject.SetActive(true);
        });

    }
}
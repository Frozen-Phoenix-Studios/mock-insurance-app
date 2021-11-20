using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BorderPanel : MonoBehaviour
{

    public OverviewPanel overviewPanel;
    public BorderPanel borderPanel;
    public SearchPanel searchPanel;
    public SelectPanel selectPanel;
    public TakePhotoPanel takephotoPanel;
    public LocationPanel locationPanel;
    public ClientInfoPanel clientInfoPanel;

    public int sceneIndex;
    public string openPanel;

    


    public void homeButon()
    {
        SceneManager.LoadScene(0);
    }


    public void backButton()
    {
        openPanel = UIManager.Instance.currentPanel;

        switch (openPanel)
        {
            case "searchPanel":
                {
                    searchPanel.gameObject.SetActive(false);
                    borderPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "mainMenu";
                    break;
                }
            case "selectPanel":
                {
                    selectPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "seachPanel";
                    break;
                }
            case "overviewpanelSelect":
                {
                    overviewPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "selectPanel";
                    break;
                }
            case "overviewPanelCreate":
                {
                    overviewPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "takePhotoPanel";
                    break;
                }
            case "takePhotoPanel":
                {
                    takephotoPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "locationPanel";
                    break;
                }
            case "locationPanel":
                {
                    locationPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "clientInfoPanel";
                    break;
                }
            case "clientInfoPanel":
                {
                    clientInfoPanel.gameObject.SetActive(false);
                    borderPanel.gameObject.SetActive(false);
                    UIManager.Instance.currentPanel = "mainMenu";
                    break;
                }

            default:
                {
                    SceneManager.LoadScene(0);
                    break;
                }

        }
    }

}

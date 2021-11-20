using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPanel : MonoBehaviour, IPanel
{
    public Text caseNumberText;
    public InputField firstName, lastName;
    public GameObject firstNameErrorText;
    public GameObject lastNameErrorText;
    public LocationPanel locationPannel;

    public void OnEnable()
    {
  
        caseNumberText.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
        UIManager.Instance.currentPanel = "clientInfoPanel";
    }


    public void ProcessInfo()
    {
        CheckInputFields();

    }


    public void CheckInputFields()
    {
        firstNameErrorText.gameObject.SetActive(false);
        lastNameErrorText.gameObject.SetActive(false);

        if (string.IsNullOrEmpty(firstName.text))
        {
            firstNameErrorText.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(lastName.text))
            {
                lastNameErrorText.gameObject.SetActive(true);
            }
            if (lastNameErrorText.activeSelf == true || firstNameErrorText.activeSelf == true)
            {
                return;
            }
        }

        else
           if (string.IsNullOrEmpty(lastName.text))
        {
            firstNameErrorText.gameObject.SetActive(false);
            lastNameErrorText.gameObject.SetActive(true);
        }
        else
        {
            firstNameErrorText.gameObject.SetActive(false);
            lastNameErrorText.gameObject.SetActive(false);
            locationPannel.gameObject.SetActive(true);
            UIManager.Instance.activeCase.name = firstName.text + " " + lastName.text;
        }
    }
}
  
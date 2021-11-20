using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public string currentPanel;
    public bool previousPanelSelect = false;
    public GameObject loadingSpinner;
    public static UIManager Instance
    {
       
        get
        {
            if (_instance == null)
            {
                Debug.LogError("THe UI Manager is NULL");
            }

            return _instance;
        }
    }

    public Case activeCase;
    public ClientInfoPanel clientInfoPanel;
    public GameObject borderPanel;

    private void Awake()
    {
        _instance = this;
    }

    public void createNewCase()
    {
        activeCase = new Case();
        activeCase.date = DateTime.Today.ToString("dddd, dd MMMM, yyyy ");
        loadingSpinner.gameObject.SetActive(true);
        AWSManager.Instance.caseIDgenerator();
    }

    public void SubmitButton()
    {
        Case awsCase = new Case();

            awsCase.caseID = activeCase.caseID;
            awsCase.name = activeCase.name;
            awsCase.date = activeCase.date;
            awsCase.locationPhoto = activeCase.locationPhoto;
            awsCase.locationNotes = activeCase.locationNotes;
            awsCase.photoTaken = activeCase.photoTaken;
            awsCase.photoNotes = activeCase.photoNotes;

            BinaryFormatter bf = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/case#" + awsCase.caseID + ".dat";
            FileStream file = File.Create(filePath);
            bf.Serialize(file, awsCase);
            file.Close();

        Debug.Log("Application Data Path : " + Application.persistentDataPath);

        //send to AWS

        AWSManager.Instance.UploadToS3(filePath, awsCase.caseID);
    }


}


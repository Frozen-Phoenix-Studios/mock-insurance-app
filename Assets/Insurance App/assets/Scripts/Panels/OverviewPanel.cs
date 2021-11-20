using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OverviewPanel : MonoBehaviour, IPanel
{
    public Text caseNumberTitle;
    public Text nameTitle;
    public Text dateTitle;
   public RawImage locationPhoto;
    public Text locationNotes;
    public Text photoNotes;
    public RawImage photoTaken;

    public void OnEnable()
    {
        caseNumberTitle.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
        nameTitle.text =  UIManager.Instance.activeCase.name;
        dateTitle.text = UIManager.Instance.activeCase.date;

        Texture2D reconstructedImgMap = new Texture2D(1, 1);
        reconstructedImgMap.LoadImage(UIManager.Instance.activeCase.locationPhoto);

        locationPhoto.texture = (Texture)reconstructedImgMap;

        locationNotes.text = "LOCATION NOTES: \n" + UIManager.Instance.activeCase.locationNotes;
        photoNotes.text = "PHOTO NOTES: \n" + UIManager.Instance.activeCase.photoNotes;

        Texture2D reconstructedImgPhoto = new Texture2D(1, 1);
        reconstructedImgPhoto.LoadImage(UIManager.Instance.activeCase.photoTaken);

        photoTaken.texture = (Texture)reconstructedImgPhoto;


    }

    public void ProcessInfo()
    {

    }
}

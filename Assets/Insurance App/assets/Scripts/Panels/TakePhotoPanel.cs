using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotoPanel : MonoBehaviour, IPanel
{
    public RawImage photoTaken;
    public Image photoThumbnail;
    public InputField photoNotes;
    public Text caseNumberId;
    private string _imgPath;
    private bool _isPhotoTaken = false;
    public OverviewPanel overviewPanel;
    public GameObject photoRequiredText;

    


    public void OnEnable()
    {
        caseNumberId.text = "CASE NUMBER: " + UIManager.Instance.activeCase.caseID;
        UIManager.Instance.currentPanel = "takePhotoPanel";
    }


    public void ProcessInfo()
    {
         byte[] imgDataPhoto = null;

        if(string.IsNullOrEmpty(_imgPath) == false)
        {
            Texture2D img = NativeCamera.LoadImageAtPath(_imgPath, 1000, false);
            imgDataPhoto = img.EncodeToPNG();
        }


        if (string.IsNullOrEmpty(photoNotes.text) == false)
        {
            UIManager.Instance.activeCase.photoNotes = photoNotes.text;
        }

        UIManager.Instance.activeCase.photoTaken = imgDataPhoto;

        if (_isPhotoTaken == true)
        {
            photoRequiredText.SetActive(false);
            overviewPanel.gameObject.SetActive(true);
            UIManager.Instance.currentPanel = "overviewPanelCreate";
        }

        else
        {
            photoRequiredText.SetActive(true);
        }
    }


    public void TakePictureButton()
    {
        TakePicture(1024);
        _isPhotoTaken = true;
        photoRequiredText.SetActive(false);

    }

    public void RetakePictureButton()
    {
        TakePicture(1024);
        _isPhotoTaken = true;
        photoRequiredText.SetActive(false);
    }

    private void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                photoTaken.texture = texture;
                photoTaken.gameObject.SetActive(true);
                photoThumbnail.gameObject.SetActive(false);
               _imgPath = path;
            }
        }, maxSize);

        Debug.Log("Permission result: " + permission);
    }

}

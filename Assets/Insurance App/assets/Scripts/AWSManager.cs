using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;


public class AWSManager : MonoBehaviour
{
    public GameObject clientInfoPanel;
    public GameObject borderPanel;
    public GameObject loadingSpinner;
    public bool randomIDObtained = false;
    public GameObject errorText;
    private static AWSManager _instance;
    public static AWSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AWS Manager is Null");
            }

            return _instance;

        }
    }

    public string S3Region = RegionEndpoint.APSoutheast2.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    private AmazonS3Client _s3Client;
    public AmazonS3Client S3Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(new CognitoAWSCredentials(
                    "ap-southeast-2:207340a5-4de3-429e-82b1-1fb5750d8c19", // Identity Pool ID
                     RegionEndpoint.APSoutheast2 // Region
                     ), _S3Region);
            }
            return _s3Client;
        }
    }

    private void Awake()
    {
        _instance = this;

        UnityInitializer.AttachToGameObject(this.gameObject);

        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

    }

    public void UploadToS3(string path, string caseID)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = "awscasefilestorage",
            Key = "case#" + caseID,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };


        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("Posted to buckett");
                SceneManager.LoadScene(0);
            }
            else
            {
                Debug.Log("Exception occured during uploading" + responseObj.Exception);
            }
        });
    }


    public void GetList(string caseNumber, Action onComplete = null)
    {
        string target = "case#" + caseNumber;


        var request = new ListObjectsRequest()
        {
            BucketName = "awscasefilestorage",
        };

        S3Client.ListObjectsAsync(request, (responseObject) =>
        {
        if (responseObject.Exception == null)
        {

            bool caseFound = responseObject.Response.S3Objects.Any(obj => obj.Key == target);


                if (caseFound == true)
                {
                    errorText.gameObject.SetActive(false);
                    Debug.Log("Case found");

                    S3Client.GetObjectAsync("awscasefilestorage", target, (responseObj) =>
                    {
                        //read the data and apply it to a case (object) to be used

                        //check if response object is not null
                        if (responseObj.Response != null)
                        {
                            //create byte array to store data from file
                            byte[] data = null;

                            //use streamreader to read response data
                            using (StreamReader reader = new StreamReader(responseObj.Response.ResponseStream))
                            {
                                // access memory stream
                                using (MemoryStream memory = new MemoryStream())
                                {
                                    //populate data byte array with memstream data
                                    var buffer = new byte[512];
                                    var  bytesRead = default(int);

                                    while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) >0 )
                                    {
                                        memory.Write(buffer, 0, bytesRead);
                                    }
                                    data = memory.ToArray();
                                }
                            }

                            //convert bytes to case object

                            using (MemoryStream memory = new MemoryStream(data))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                Case downloadedCase = (Case)bf.Deserialize(memory);
                                Debug.Log("Downloaded case name: " + downloadedCase.name);
                                UIManager.Instance.activeCase = downloadedCase;
                                loadingSpinner.gameObject.SetActive(false);
                                if (onComplete != null)
                                {
                                    onComplete();
                                }
                            }

                        }

                    });
                }

                else
                {
                    Debug.Log("Case not found");
                    errorText.gameObject.SetActive(true);
                    loadingSpinner.gameObject.SetActive(false);

                }
            }
            else
            {
                Debug.Log("Error getting list of items from S3: " + responseObject.Exception);
            }

        });
    }


          

    public void caseIDgenerator()
    {

        Debug.Log("Generating case number");

            int caseIDRandom = UnityEngine.Random.Range(1, 999999);

            string caseIDRandomString = caseIDRandom.ToString();

            if (caseIDRandomString.Length <= 6)
            {
                do
                {
                    caseIDRandomString = "0" + caseIDRandomString;
                } while (caseIDRandomString.Length <= 6);


            }


        //get list
        Debug.Log("Random case number generated is : " + caseIDRandomString);
            Debug.Log("Checking generated case number for duplicates");
            string target = "case#" + caseIDRandomString;

            var request = new ListObjectsRequest()
            {
                BucketName = "awscasefilestorage",
            };

            S3Client.ListObjectsAsync(request, (responseObject) =>
            {
                if (responseObject.Exception == null)
                {

                  bool caseFound = responseObject.Response.S3Objects.Any(obj => obj.Key == target);


                    if (caseFound == true)
                    {
                        Debug.Log("Case found");
                        caseIDgenerator();
                    }

                    else if (caseFound == false)
                    {
                        Debug.Log("Generated random case Id");
                        UIManager.Instance.activeCase.caseID = caseIDRandomString;

                        loadingSpinner.gameObject.SetActive(false);

                        clientInfoPanel.gameObject.SetActive(true);

                        borderPanel.SetActive(true);

                        randomIDObtained = true;
                        Debug.Log("End of process");
                    }

                    else
                    {
                        return;
                    }

                }
            });


    }



}


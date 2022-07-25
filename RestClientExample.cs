//modified from https://github.com/dilmerv/UnityRestClient
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using RestClient.Core;
using RestClient.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RestClientExample : MonoBehaviour
{
    // the Url used to connect to Azure Cognitive Services Computer Vision API (v3.2) that contains the Optical Character Recognition (OCR) services. Here the version to use, language to detect, and orientation to use is specified. 
    //it is set in the inspector
    [SerializeField]
    private string RequestUrl;//set in inspector - https://masterprojecttextrecognition.cognitiveservices.azure.com/vision/v3.2/read/analyze

    [SerializeField]
    private string clientId;//set in inspector, this is "Ocp-Apim-Subscription-Key"

    [SerializeField]
    private string clientSecret; //set in inspector, this is the Ocp-Apim-Subscription-Key - f1bd6c983ec44a919d43aaafbcc8e7b7

    // [SerializeField]
    // private string imageToOCR = ""; //set in inspector, not used because we are using screenshots, the url to the image located online that is to be sent to Azure OCR 

    [SerializeField]
    private TextMeshProUGUI header; //for UI

    [SerializeField]
    private TextMeshProUGUI wordsCapture; //for UI
    public GameObject resultsTXTGameObject;
    public Text resultsTXT;
    public GameObject loadingIcon;
    public GameObject tryAgainBTNResults;
    public GameObject saveScriptResultsBTN;
    public GameObject variablesDetectedTXT;
    public GameObject wordsCapturedGameObject;
    //public GameObject background;
    public GameObject debugtextSide;
    public GameObject debugtextDown;
    public Text azureDebugText;
    public Text rightAzureDebugText;
    public GameObject switchToVuforiaBTN;
    public GameObject saveScriptBTN;
    public GameObject startCanvas;
    public ParticleSystem PortalExit;
    public ParticleSystem PortalEntrance;
    public GameObject A3Map3D;
    public GameObject EditTowerModeTitle;


    //public GameObject PointSchemaButton;
    //public GameObject ScanMapBg;
    //public GameObject ChallengeButton;
    //public GameObject readyButton;
    public SocketInteractions socketInteractions;
    public AzureOCRResponse azureOCRResponse;
    public string towerName;
    public bool canUseEditBTN = true;
   



    public void TakeScreenshotAndSendToAzureOCR()
    {
        if (canUseEditBTN == true)
        {
            StartCoroutine(ScreenShotPNG()); //this takes the screenshot and activates the function to send it to Azure, the screen saver needs to be in a coroutine to be able to run at the end of the frame}
        }
    }

    IEnumerator ScreenShotPNG()
    {   
        //disable UI for picture
        Debug.Log("disabelling ui");
        azureDebugText.text ="";
        rightAzureDebugText.text = "";
        tryAgainBTNResults.SetActive(false);
        saveScriptResultsBTN.SetActive(false);
        debugtextSide.SetActive(false);
        debugtextDown.SetActive(false);
        variablesDetectedTXT.SetActive(false);
        switchToVuforiaBTN.SetActive(false);
        saveScriptBTN.SetActive(false);
        wordsCapturedGameObject.SetActive(false);
        startCanvas.SetActive(false);
        resultsTXTGameObject.SetActive(false);
        PortalEntrance.Stop();
        PortalExit.Stop();
        A3Map3D.SetActive(false);
        EditTowerModeTitle.SetActive(false);
        //PointSchemaButton.SetActive(false);
        // ScanMapBg.SetActive(false);
        //ChallengeButton.SetActive(false);
        //readyButton.SetActive(false);
        //save the name of the tower to be edited
        towerName = socketInteractions.towerName;

        // setup the request header, used for indentification to the azure account that is being used
        RequestHeader clientSecurityHeader = new RequestHeader {
            Key = clientId,
            Value = clientSecret
        };

        // setup the request header, used to tell what kind of format the data is being sent as
        RequestHeader contentTypeHeader = new RequestHeader {
            Key = "Content-Type",
            Value = "application/octet-stream" // used for sending byte[] with screenshot image data to be sent to Azure OCR 
            //Value = "application/json" // to be used the url to the image located online that is to be sent to Azure OCR 
            //Value = "multipart/form-data" 
        };

        // We should only read the screen buffer after rendering is complete
        Debug.Log("wating for end of frame");
        yield return new WaitForEndOfFrame();
        azureDebugText.text =" setting up foto ";

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Debug.Log("taking picture");
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        azureDebugText.text ="saved pixels in tex ";
        // Encode texture into PNG
         azureDebugText.text ="saving tex in byteimage";
        byte[] byteImage = tex.EncodeToPNG();
         azureDebugText.text ="tex encoded in byteimage";
        UnityEngine.Object.Destroy(tex);
        azureDebugText.text ="tex destoryed and saving file";

        // Does not work on Android,
        // For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", byteImage);
        //debugUIText.text ="saved file to local storage";
        //Debug.Log("screen saved and write to file: " + Application.dataPath + "/../SavedScreen.png");


        //debugtextSide.SetActive(true);
        azureDebugText.text ="sending foto ";
        //debugtextSide.SetActive(true);

        // Here the information(the string RequestUrl, byte[] byteImage and the headers) needed to seend the image to Azure OCR using HttpPost AND HttpGet in the RestWebClient script is pasted through and to activate the OnRequestComplete function when data from Azure OCR is recived.
        resultsTXTGameObject.SetActive(true);
        A3Map3D.SetActive(true);
        PortalEntrance.Play();
        PortalExit.Play();
        loadingIcon.SetActive(true);
        EditTowerModeTitle.SetActive(true);
        resultsTXT.text = "Getting Results \n  \n Please Wait \n\n\n";
        Debug.Log("starting HttpPost");
        StartCoroutine(RestWebClient.Instance.HttpPost(RequestUrl, byteImage, (response) => OnPostRequestComplete(response), new List<RequestHeader> 
        {
            clientSecurityHeader,
            contentTypeHeader
        }));

        // debugUIText.text =" foto sent ";
        // activating UI and the black background to show the word recived from Azure OCR
        //background.SetActive(true);
        Debug.Log(" activating UI ");
        //debugtext.SetActive(true);
        switchToVuforiaBTN.SetActive(true);
       // saveScriptBTN.SetActive(true);

        // used for when using an online image url and application/json
        // build image url required by Azure Vision OCR for the online image url
        /* ImageUrl imageUrl = new ImageUrl { Url = imageToOCR };

         // the HttpPost for the "application/json" contentype to send the image url converted to json required by Azure Vision OCR in json
         StartCoroutine(RestWebClient.Instance.HttpPost(RequestUrl, JsonUtility.ToJson(imageUrl), (r) => OnRequestComplete(r), new List<RequestHeader> 
         {
             clientSecurityHeader,
             contentTypeHeader
         }));*/

    }

    void OnPostRequestComplete(Response response)
    {
        Debug.Log("OnPostRequestComplete started");
        azureDebugText.text = " recived operationId from azure ";
        Debug.Log(" recived operationId from azure ");
        //here the information and data from request to Azure OCR is displayed on the console
        Debug.Log($"Status Code: {response.StatusCode}");
        Debug.Log($"Error: {response.Error}");
        Debug.Log($"DebugOperationId: {response.DebugOperationId}");

        if(response.DebugOperationId.ToString().Contains("error"))
        {
            //wordsCapturedGameObject.SetActive(true);



            loadingIcon.SetActive(false);
            azureDebugText.text ="Error post, Please try again in a few seconds";
            resultsTXT.text = "Error. Save Script used too many times. Please try again in one minute";
            saveScriptBTN.SetActive(true);
            tryAgainBTNResults.SetActive(true);
            saveScriptResultsBTN.SetActive(true);
        }
        else
        {
        string operationId = response.DebugOperationId.ToString();

        //setting headers to be able to access azure OCR Subscription
        RequestHeader clientSecurityHeader = new RequestHeader
        {
            Key = clientId,
            Value = clientSecret
        };
        
        Debug.Log("starting HttpGet");
        StartCoroutine(RestWebClient.Instance.HttpGet(operationId, (response) => OnGetRequestComplete(response), new List<RequestHeader>
        {
            clientSecurityHeader
        }));
        }
        
    }

    void OnGetRequestComplete(Response response)
    {
        //wordsCapturedGameObject.SetActive(true);
        azureDebugText.text =" recived text from azure ";
        Debug.Log("OnGetRequestComplete started");
        //here the information and data from request to Azure OCR is displayed on the console
        Debug.Log($"Status Code: {response.StatusCode}");
        Debug.Log($"complete Data: {response.Data}");
        Debug.Log($"Error: {response.Error}");
        Debug.Log("stirng Response.Data: " + response.Data.ToString());
        // the data from Azure OCR is displyed on the UI under Header and words captured
        if(string.IsNullOrEmpty(response.Error) && !string.IsNullOrEmpty(response.Data))
        {
            //AzureOCRResponse is from AzureOCRResponse.cs in models
            //Debug.Log("data from azure: " + response.Data);
            azureOCRResponse = JsonUtility.FromJson<AzureOCRResponse>(response.Data);

            //header.text = $"Orientation: {azureOCRResponse.orientation} Language: {azureOCRResponse.language} Text Angle: {azureOCRResponse.textAngle}";

            string words = string.Empty;
            //Debug.Log("starting to show words from azure");
            //Debug.Log("regions: " + azureOCRResponse);
            foreach (var ocrResponse in azureOCRResponse.analyzeResult.readResults)
            {
                //Debug.Log("azure  region");
                foreach (var line in ocrResponse.lines)
                {
                    //displays the whole line of word that OCR has recognised in one
                    //  words+= "{" + line.text + "}";

                    //Debug.Log("line");
                    foreach (var word in line.words)
                    {
                        //Debug.Log("word");
                        //words += word.text + "\n"; //each detected word is displayed on a new line
                        words += word.text + " ";
                       // words +=" {" + word.text + "}";
                        //words += word.text + "  ";
                    }
                }
            }
            
            wordsCapture.text = words;
            //Debug.Log("words contains: "+words);

            int damage = GetVariableValue(words,"damage");
            //Debug.Log("final damage value: " + damage);

            int rateOfFire = GetVariableValue(words, "rateOfFire");
            //Debug.Log("final speed value: " + rateOfFire);

            int range = GetVariableValue(words, "range");
            //Debug.Log("final range value: " + range);

            int numberOfTargets = GetVariableValue(words, "numberOfTargets");
            //Debug.Log("final bullets value: " + numberOfTargets);

            int stunRecovery = GetVariableValue(words, "stunRecovery");
            //Debug.Log("final stun recovery value: " + stunRecovery);

            //to display the final detected variables
            //variablesDetectedTXT.SetActive(true);
            //variablesDetectedTXT.GetComponent<Text>().text = " damge " + damage.ToString() + " speed " + rateOfFire.ToString() + " range " + range.ToString() + " bullets " + numberOfTargets.ToString() + " stun recovery " + stunRecovery.ToString();
            Debug.Log("words: " +words);
            resultsTXTGameObject.SetActive(true);
            loadingIcon.SetActive(false);

            //making the string to show the results of the scanned variables in the way they were made in real life
            string stringForResultsTXT = "These are the variables detected: \n";
            string[] splitString = words.Split(' ');
            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i].Contains("damage"))
                {
                    stringForResultsTXT += "damage: " + damage.ToString() + "\n";
                }

                else if (splitString[i].Contains("range"))
                {
                    stringForResultsTXT += "range: " + range.ToString() + "\n";
                }

                else if (splitString[i].Contains("rateOfFire"))
                {
                    stringForResultsTXT += "rateOfFire: " + rateOfFire.ToString() + "\n";
                }

                else if (splitString[i].Contains("numberOfTargets"))
                {
                    stringForResultsTXT += "numberOfTargets: " + numberOfTargets.ToString() + "\n";
                }

                else if (splitString[i].Contains("stunRecovery"))
                {
                    stringForResultsTXT += "stunRecovery: " + stunRecovery.ToString() + "\n";
                }
            }

            stringForResultsTXT += "\n" + " If these values are incorrect, please scan again.";
            resultsTXT.GetComponent<Text>().text = stringForResultsTXT;


           // resultsTXT.GetComponent<Text>().text = "These are the variables detected: \n" + "damge: " + damage.ToString() + "\n" + " range: " + range.ToString() + "\n" + " rateOfFire: " + rateOfFire.ToString() + "\n" + " numberOfTargets: " + numberOfTargets.ToString() + "\n" + " stunRecovery: " + stunRecovery.ToString()+ "\n" + "\n" + " If these values are incorrect, please scan again.";
            //debugtextSide.SetActive(true);
            //azureDebugText.text = "dmg:" + damage.ToString() + " rFire:" + rateOfFire.ToString() + " range:" + range.ToString() + " numT:" + numberOfTargets.ToString() +  " stu:" + stunRecovery.ToString();
            tryAgainBTNResults.SetActive(true);
            saveScriptResultsBTN.SetActive(true);


            socketInteractions.ChangeTowerVariable(towerName, damage, range, rateOfFire, numberOfTargets, stunRecovery);
            //socketInteractions.ChangeTowerVariable("A3Tower", damage, range, rateOfFire, numberOfTargets, stunRecovery); 
        }

        else if(!string.IsNullOrEmpty(response.Error))
        {
            loadingIcon.SetActive(false);
            //wordsCapturedGameObject.SetActive(true);
            resultsTXT.text = "Error. Save Script has been used too many times in a row. \n Please try again in a few seconds."; //"Error: " + response.Data.ToString();
            tryAgainBTNResults.SetActive(true);
            saveScriptResultsBTN.SetActive(true);
        }

        //Activating UI again 
        //AzureDebugText.text = "";
        saveScriptBTN.SetActive(true);
        switchToVuforiaBTN.SetActive(true);
    }

    public int GetVariableValue(string stringToSearch, string variableName )
    {
        int oneAfterDetectedWordLocation = 0;
        string stringNumberFound = "";
        int foundNumber=0;

        Debug.Log(stringToSearch);

        string[] splitStringIntoWords = stringToSearch.Split(' ');

        for (int i = 0; i < splitStringIntoWords.Length; i++)
        {
            //Debug.Log(splitStringIntoWords[i]);
            if (splitStringIntoWords[i].Contains(variableName))
            {
                //needs to look for number after the word 
                oneAfterDetectedWordLocation = i;
                Debug.Log("found: " + variableName+ " At: " +i );
                break;
            }
        }

        switch (variableName)
        {
            case "damage":

                for (int j = oneAfterDetectedWordLocation; j < splitStringIntoWords.Length; j++)
                {
                    if (splitStringIntoWords[j].Contains("rateOfFire") || splitStringIntoWords[j].Contains("range") || splitStringIntoWords[j].Contains("numberOfTargets") || splitStringIntoWords[j].Contains("stunRecovery"))
                    {
                        Debug.Log("other veriable name found, break");
                        break;
                    }

                    else
                    {
                        Debug.Log("look for number at " + j);
                        Match findNumber = Regex.Match(splitStringIntoWords[j], @"\d+");
                        Debug.Log(splitStringIntoWords[j]);
                        if (findNumber.Success)
                        {
                            stringNumberFound = findNumber.Value;
                            // Report position as a one-based integer.
                            Debug.Log("number found " + findNumber.Value);
                            bool numberFoundBool = int.TryParse(stringNumberFound, out foundNumber);
                            break;
                        }
                        else
                            Debug.Log("a number was not found");
                    }
                }
                break;

            case "rateOfFire":
                for (int j = oneAfterDetectedWordLocation; j < splitStringIntoWords.Length; j++)
                {
                    if (splitStringIntoWords[j].Contains("damage") ||  splitStringIntoWords[j].Contains("range") || splitStringIntoWords[j].Contains("numberOfTargets") || splitStringIntoWords[j].Contains("stunRecovery"))
                    {
                        Debug.Log("other veriable name found, break");
                        break;
                    }

                    else
                    {
                        Debug.Log("look for number at " + j);
                        Match findNumber = Regex.Match(splitStringIntoWords[j], @"\d+");
                        Debug.Log(splitStringIntoWords[j]);
                        if (findNumber.Success)
                        {
                            stringNumberFound = findNumber.Value;
                            // Report position as a one-based integer.
                            Debug.Log("number found " + findNumber.Value);
                            bool numberFoundBool = int.TryParse(stringNumberFound, out foundNumber);
                            break;
                        }
                        else
                            Debug.Log("a number was not found");
                    }
                }
                break;

            case "range":
                for (int j = oneAfterDetectedWordLocation; j < splitStringIntoWords.Length; j++)
                {
                    if (splitStringIntoWords[j].Contains("damage") || splitStringIntoWords[j].Contains("rateOfFire") || splitStringIntoWords[j].Contains("numberOfTargets") || splitStringIntoWords[j].Contains("stunRecovery"))
                    {
                        Debug.Log("other veriable name found, break");
                        break;
                    }

                    else
                    {
                        Debug.Log("look for number at " + j);
                        Match findNumber = Regex.Match(splitStringIntoWords[j], @"\d+");
                        Debug.Log(splitStringIntoWords[j]);
                        if (findNumber.Success)
                        {
                            stringNumberFound = findNumber.Value;
                            // Report position as a one-based integer.
                            Debug.Log("number found " + findNumber.Value);
                            bool numberFoundBool = int.TryParse(stringNumberFound, out foundNumber);
                            break;
                        }
                        else
                            Debug.Log("a number was not found");
                    }
                }
                break;

            case "numberOfTargets":

                for (int j = oneAfterDetectedWordLocation; j < splitStringIntoWords.Length; j++)
                {
                    if (splitStringIntoWords[j].Contains("damage") || splitStringIntoWords[j].Contains("rateOfFire") || splitStringIntoWords[j].Contains("range") || splitStringIntoWords[j].Contains("stunRecovery"))
                    {
                        Debug.Log("other veriable name found, break");
                        break;
                    }

                    else
                    {
                        Debug.Log("look for number at " + j);
                        Match findNumber = Regex.Match(splitStringIntoWords[j], @"\d+");
                        Debug.Log(splitStringIntoWords[j]);
                        if (findNumber.Success)
                        {
                            stringNumberFound = findNumber.Value;
                            // Report position as a one-based integer.
                            Debug.Log("number found " + findNumber.Value);
                            bool numberFoundBool = int.TryParse(stringNumberFound, out foundNumber);
                            break;
                        }
                        else
                            Debug.Log("a number was not found");
                    }
                }

                break;

            case "stunRecovery":
                for (int j = oneAfterDetectedWordLocation; j < splitStringIntoWords.Length; j++)
                {
                    if (splitStringIntoWords[j].Contains("damage") || splitStringIntoWords[j].Contains("rateOfFire") || splitStringIntoWords[j].Contains("range") || splitStringIntoWords[j].Contains("numberOfTargets"))
                    {
                        Debug.Log("other veriable name found, break");
                        break;
                    }

                    else
                    {
                        Debug.Log("look for number at " + j);
                        Match findNumber = Regex.Match(splitStringIntoWords[j], @"\d+");
                        Debug.Log(splitStringIntoWords[j]);
                        if (findNumber.Success)
                        {
                            stringNumberFound = findNumber.Value;
                            // Report position as a one-based integer.
                            Debug.Log("number found " + findNumber.Value);
                            bool numberFoundBool = int.TryParse(stringNumberFound, out foundNumber);
                            break;
                        }
                        else
                            Debug.Log("a number was not found");
                    }
                }
                break;
        }
       
        Debug.Log(variableName + " final number found: " + foundNumber);
        return foundNumber;

    }

    public class ImageUrl 
    {
        public string Url;
    }
}

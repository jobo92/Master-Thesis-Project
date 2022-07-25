//modified from https://github.com/dilmerv/UnityRestClient
using System.Collections;
using System.Collections.Generic;
using RestClient.Core.Models;
using RestClient.Core.Singletons;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
 
namespace RestClient.Core
{
    public class RestWebClient : Singleton<RestWebClient>
    {
        public GameObject debugTxtObject;
        public Text debugUIText;

        [SerializeField]
        private string ResultRequestURL = "https://masterprojecttextrecognition.cognitiveservices.azure.com/vision/v3.2/read/analyzeResults/";


        //private const string defaultContentType = "application/json"; // used for sending url to images located online
        private const string defaultContentType = "application/octet-stream";// used for sending screen shot images
        //private const string defaultContentType = "multipart/form-data"; // it is not currently used


        public IEnumerator HttpPost(string url, byte[] body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {   //we have to encode all the bytes in the specified byte array into a string for it to work, because it does not acept byte[]
            debugTxtObject.SetActive(false);
            debugUIText.text =" recived image data";
            
            using (UnityWebRequest webRequest = UnityWebRequest.Post(url, System.Text.Encoding.UTF8.GetString(body)))
            {
           
                if(headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {// we are setting the headers that contain the login info from RestClientExample, whitch contains the clientSecurityHeader (clientid and client secret) 
                    //and contentTypeHeader(Content-Type, and what kind of data we are sending(json or octet-stream)) to be able to use Azure OCR 
                        webRequest.SetRequestHeader(header.Key, header.Value);
                        Debug.Log("set headers, key " + header.Key +" value " + header.Value );
                        debugUIText.text ="post setting headers";
                    }
                }

                // here we are setting the reference to the UploadHandler object which manages body data to be uploaded to the remote server
                //here we are setting what the content type is, but it seems to work without it also since the contenttype has already been declared
                debugUIText.text ="setting uploadhandlers";
                webRequest.uploadHandler.contentType = defaultContentType;
                //here we are using the byte[] that contains the data for the screenshot image to be analysed 
                webRequest.uploadHandler = new UploadHandlerRaw(body);
                debugUIText.text =" uploadhandlers saved and sending data";

                yield return webRequest.SendWebRequest();
                debugUIText.text =" data sent";


                if(webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    debugUIText.text ="error in sending data";
                    //if something is worng with the request this gets the error code and messeges and sends them to the RestClientExample where they are displayed using Debug.log
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    });
                }
                
                if(webRequest.isDone)
                {
                    debugUIText.text ="data recived and send to show";
                    //if the request is sucsseful this gets the data from Azure OCR and sends it to RestClientExample to be used
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    // After the request, get the operation location (operation ID) for the Get Read Result operation to fetch the detected text
                    // error handling for to many sucssecive tries which results in no operation-location
                    string operationLocation = webRequest.GetResponseHeader("Operation-Location");
                    Debug.Log("Post operationLocation: " + operationLocation);
                    if (string.IsNullOrEmpty(operationLocation))
                    {
                        Debug.Log("post/ no operation location ");
                        debugUIText.text = " post/ no operation location ";
                        string operationidError = "error";
                        callback(new Response
                        {
                            StatusCode = webRequest.responseCode,
                            Error = webRequest.error,
                            DebugOperationId = operationidError
                        });
                    }
                    else
                    {
                        // We only need the ID and not the  full URL
                        const int numberOfCharsInOperationId = 36;
                        string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
                    
                        Debug.Log("FINISHED POST, STARTING GET REQUEST");
                        debugUIText.text = "FINISHED POST, STARTING GET REQUEST";
                        callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        DebugOperationId = operationId
                        });
                    }
                }
            }
        }

        public IEnumerator HttpGet(string operationId, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {

            Debug.Log("GET STARTED/ operationId: " + operationId);
            debugUIText.text = "GET STARTED/ operationId: " + operationId;
            // adding operationid to url to get the exstracted text data from azure 
            string GetReadResultRequestURL = ResultRequestURL + operationId;
            Debug.Log("post URL + operationId: " + GetReadResultRequestURL);

            using (UnityWebRequest webRequest = UnityWebRequest.Get(GetReadResultRequestURL))
            {
                if (headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {// we are setting the headers that contain the login info from RestClientExample, whitch contains the clientSecurityHeader (clientid and client secret) 
                     //and contentTypeHeader(Content-Type, and what kind of data we are sending(json or octet-stream)) to be able to use Azure OCR 
                        webRequest.SetRequestHeader(header.Key, header.Value);
                        Debug.Log("Get headers, key " + header.Key + " value " + header.Value);
                       // debugUIText.text = "Get setting headers";
                    }
                }
                debugUIText.text = " sending GET request";
                Debug.Log("sending GET request"); 
                yield return webRequest.SendWebRequest();
                //Using an interval of 0.3 second to avoid exceeding the requests per second(RPS) rate to Azure. 4 calls with 0.1 and 2 calls with 0.2 on avarege. The free tier limits the request rate to 20 calls per minute
                yield return new WaitForSeconds(0.3f);

                if (webRequest.result == UnityWebRequest.Result.ConnectionError){
                    debugUIText.text = " httpGet connectionError";
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                } 
                
                if(webRequest.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    Debug.Log("Get is done, Data : " + data);

                    string status = webRequest.downloadHandler.text;
                    Debug.Log("status text: " + status);
                    debugUIText.text = "status text: " + status;
                    if (status.Contains("notStarted") || status.Contains("running"))
                    {
                        Debug.Log("Get/ status Contains word notStarted ||running ");
                        debugUIText.text = "Get/ status Contains word notStarted ||running ";
                        Debug.Log("Get called to start ");
                        StartCoroutine(HttpGet(operationId, callback, headers));
                    }

                    else if (status.Contains("error"))
                    {
                        Debug.Log("Get/ status Contains error ");
                        debugUIText.text = "Get/ status Contains error ";
                         callback(new Response
                        {
                            StatusCode = webRequest.responseCode,
                            Error = webRequest.error,
                            Data = data
                        });
                    }

                    else if (status.Contains("succeeded"))
                    {

                        Debug.Log("Get/ status Contains word succeeded");
                        debugUIText.text = "Get/ status Contains error ";
                        callback(new Response
                        {
                            StatusCode = webRequest.responseCode,
                            Error = webRequest.error,
                            Data = data
                        });

                    }

                    
                }
            }
        }

        //Not using currently
        public IEnumerator HttpDelete(string url, System.Action<Response> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    });
                }

                if (webRequest.isDone)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode
                    });
                }
            }
        }
        // this one is not currently used
        public IEnumerator HttpPut(string url, string body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            Debug.Log("started httpPut");
            using(UnityWebRequest webRequest = UnityWebRequest.Put(url, body))
            {
                Debug.Log("headers "+headers +" .tosring"+ headers.ToString());
                if(headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                        Debug.Log("set headers, key " + header.Key +" value " + header.Value );
                    }
                }

                //webRequest.uploadHandler.contentType = defaultContentType;
               // webRequest.uploadHandler = new UploadHandlerRaw(body);
               //webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
                Debug.Log("staring sending");
                yield return webRequest.SendWebRequest();

                if(webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("connection error");
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }
                
                if(webRequest.isDone)
                {
                    Debug.Log("webReg done");
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    Debug.Log("data = " + data);
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Data = data
                    });
                }
            }
        }
        // this one is not currently used
        public IEnumerator HttpHead(string url, System.Action<Response> callback)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Head(url))
            {
                yield return webRequest.SendWebRequest();
                
                if(webRequest.result == UnityWebRequest.Result.ConnectionError){
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }
                
                if(webRequest.isDone)
                {
                    var responseHeaders = webRequest.GetResponseHeaders();
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Headers = responseHeaders
                    });
                }
            }
        }
    }
}
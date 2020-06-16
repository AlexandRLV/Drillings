using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using UnityEngine;

public class GDocsWriter : MonoBehaviour
{
    private static GDocsWriter Instance { get; set; }

    private static string[] Scopes = {DocsService.Scope.DocumentsReadonly};
    private static string applicationName = "Google Docks Debug";
    
    private string editorCredentialsPath = Application.streamingAssetsPath + "/credentials.json";
    private string iosCredentialsPath = Application.streamingAssetsPath + "/credentials.json";
    private string documentId = "1QqMO3Gq2e2f6W1o_oTGynWcOOKv4n3notCt3XWumHAg";
    private UserCredential credential;
    private DocsService service;
    private bool initialized;


    private void Awake()
    {
        Instance = this;

        try
        {
            using (FileStream stream = new FileStream(editorCredentialsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = Application.streamingAssetsPath + "/token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            service = new DocsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });

            DocumentsResource.GetRequest request = service.Documents.Get(documentId);
            Document doc = request.Execute();
            Debug.Log($"Document {doc.Title} loaded");
            initialized = true;
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }
    }
}

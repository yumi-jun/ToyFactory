using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConvertApiDotNet;
using System.IO;

public class pdfConverter : MonoBehaviour
{
    ConvertApi convertApi;
    string convertAPIKey = string.Empty;

    void Start()
    {
        FileInfo fileInfo = new FileInfo("./convertAPI_key.txt");

        if (fileInfo.Exists)
        {
            StreamReader reader = new StreamReader("./convertAPI_key.txt");
            convertAPIKey = reader.ReadToEnd();
            reader.Close();
        }
        else {
            Debug.LogError("File does not exist");
        }

        if(convertAPIKey != string.Empty)
        {
            convertApi = new ConvertApi(convertAPIKey);
        }
    }

    private async void requestConvert()
    {

        var convert = await convertApi.ConvertAsync("pdf", "txt", new ConvertApiFileParam(@"C:/Users/A/Downloads/testAPI2_lectureNote.pdf"));

        await convert.SaveFileAsync(@"./Assets//Result/testAPI2_result.txt");
        Debug.Log(convert.ToString());

    }

    public void convertPdfToTXT() {
        if (convertAPIKey != string.Empty)
        {
            requestConvert();
        }
    }
}

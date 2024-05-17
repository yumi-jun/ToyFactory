using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class openFile : MonoBehaviour
{
    [SerializeField] GameObject converter, selectedFileTitle;
    private string[] parsedFilePath;

    public void onclickOpen() {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("", "", "pdf", false);

        parsedFilePath = paths[0].Split('\\');
        selectedFileTitle.GetComponent<TextMeshProUGUI>().text = "파일 이름: " + parsedFilePath[parsedFilePath.Length-1];

        converter.GetComponent<iTextSharpConverter>().convertPdfToText(paths[0]);
    }
}

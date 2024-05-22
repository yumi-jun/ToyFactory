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

        if (paths.Length >= 1) {
            parsedFilePath = paths[0].Split('\\');
            selectedFileTitle.GetComponent<TextMeshProUGUI>().text = "���� �̸�: " + parsedFilePath[parsedFilePath.Length - 1];
            GameSceneUserDataManager.Instance().setFileName(parsedFilePath[parsedFilePath.Length - 1]);

            converter.GetComponent<iTextSharpConverter>().convertPdfToText(paths[0]);
        }
    }
}

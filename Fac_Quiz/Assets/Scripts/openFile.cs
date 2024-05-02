using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openFile : MonoBehaviour
{
    [SerializeField] GameObject converter;

    public void onclickOpen() {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
        //foreach (string path in paths)
        //{
        //    Debug.Log(path);
        //}

        converter.GetComponent<iTextSharpConverter>().convertPdfToText(paths[0]);
    }
}

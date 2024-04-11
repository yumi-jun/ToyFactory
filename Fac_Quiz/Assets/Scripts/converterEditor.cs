using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(pdfConverter))]
public class converterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        pdfConverter converter = (pdfConverter)target;
        if (GUILayout.Button("convert")) { 
            converter.convertPdfToTXT();
        }
    }
}

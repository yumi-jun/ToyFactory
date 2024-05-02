using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using iTextSharp.text;

public class iTextSharpConverter : MonoBehaviour
{
    string result = string.Empty;
    [SerializeField] GameObject quizMaker;

    public void convertPdfToText(string path) {
        PdfReader pdfReader = new PdfReader(path.ToString());
        Debug.Log(pdfReader.NumberOfPages);
        for (int i = 1; i <= pdfReader.NumberOfPages; i++)
        {
            result += PdfTextExtractor.GetTextFromPage(pdfReader, i) + "\r\n\r\n";
        }

        Debug.Log(result);
        GUIUtility.systemCopyBuffer = result;
        quizMaker.GetComponent<quizGenerator>().inputLectureNoteStr = result;
    }
}

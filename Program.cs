using System;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PDFFileSizeSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] srcFilePaths;

            srcFilePaths = Directory.GetFiles("SOURCE FOLDER");

            foreach(var filepath in srcFilePaths)
            {
                using (var pdfDoc = new PdfDocument(new PdfReader(filepath)))
                {
                    var filename = Path.GetFileNameWithoutExtension(filepath);
                    var outputDir = @"DESTINATION FOLDER";
                    var splitter = new CustomSplitter(pdfDoc, outputDir, filename);
                    var splittedDocs = splitter.SplitBySize(15000000); //15 million bytes

                    foreach (var splittedDoc in splittedDocs)
                    {
                        splittedDoc.Close();
                    }
                }
            }
        }
    }
}


class CustomSplitter : PdfSplitter
{
    private int _order;
    private readonly string _destinationFolder;
    public string _filename;

    public CustomSplitter(PdfDocument pdfDocument, string destinationFolder, string filename) : base(pdfDocument)
    {
        _filename = filename;
        _destinationFolder = destinationFolder;
        _order = 1;
    }

    protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
    {
        return new PdfWriter(_destinationFolder + _filename + "-Part " + _order++.ToString("0000") + ".pdf");
    }
}
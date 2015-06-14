using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFTest
{
    class Program
    {
        log4net.ILog log = log4net.LogManager.GetLogger("PDFTest");

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(System.Configuration.ConfigurationManager.AppSettings["log4NetConfigFile"]));

            Program program = new Program();



            program.JoinPDFDocuments(new List<string>() { "../../PDF files/Report.pdf", "../../PDF files/Arboretum.pdf" });
            program.JoinPDFDocuments(new List<string>() { "../../PDF files/Report.pdf", "../../PDF files/Report.pdf" });
            program.JoinPDFDocuments(new List<string>() { "../../PDF files/Arboretum.pdf", "../../PDF files/Arboretum.pdf" });

            program.JoinPDFDocuments(new List<string>() { "../../PDF files/Troublesome.pdf", "../../PDF files/Troublesome.pdf" });

            program.JoinPDFDocuments(new List<string>() { "../../PDF files/Report.pdf", "../../PDF files/Troublesome.pdf" });

        }

        private string JoinPDFDocuments(List<string> PDFFilePaths)
        {
            try
            {
                log.Debug("JoinPDFDocument()...");
                // Open the output document
                PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();
                // Iterate files
                int indexPDF = 0;
                foreach (string pdfFilePath in PDFFilePaths)
                {
                    log.DebugFormat("PDF document {0}/{1} {2}...", ++indexPDF, PDFFilePaths.Count, pdfFilePath);
                    // Open the document to import pages from it.
                    PdfSharp.Pdf.PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(pdfFilePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                    // Iterate pages
                    int count = inputDocument.PageCount;
                    for (int idx = 0; idx < count; idx++)
                    {
                        log.DebugFormat("Page {0}/{1}...", idx, count);

                        // Get the page from the external document...
                        PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];
                        // ...and add it to the output document.
                        outputDocument.AddPage(page);
                    }
                }
                // Save the document...
                string filename = System.IO.Path.GetTempFileName();
                log.DebugFormat("Saving to {0}...", filename);
                outputDocument.Save(filename);

                return filename;
            }
            catch (Exception e)
            {
                log.Error(e);
                return null;
            }
        }
    }
}

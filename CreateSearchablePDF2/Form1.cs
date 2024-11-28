using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Xml.Linq;
using System.Runtime.Remoting.Contexts;
using iTextSharp.text.log;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf.parser;
using System.Runtime.Remoting;
using System.Collections;
using System.Security.Cryptography;
using System.Reflection;
using System.Configuration;

namespace CreateSearchablePDF2
{

    public partial class Form1 : Form
    {
        private string _SelectedFileName = string.Empty;
        //private string lastDirectory = @"C:\"; // Standardverzeichnis
        private string lastDirectory = ConfigurationManager.AppSettings["InitialSelectDirectory"].ToString();
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSelectOCRResult_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = lastDirectory;
                openFileDialog.Filter = "text (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _SelectedFileName = openFileDialog.FileName;
                    lastDirectory = System.IO.Path.GetDirectoryName(_SelectedFileName);
                }
            }
        }

        private void buttonCreateSearchPDF_Click(object sender, EventArgs e)
        {
            string outPath = ConfigurationManager.AppSettings["ResultDirectory"].ToString();
            if (File.Exists(outPath + "firstTry.pdf"))
                File.Delete(outPath + "firstTry.pdf");

            FileStream fs = new FileStream(outPath + "firstTry.pdf", FileMode.Create);

            Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            // Add meta information to the document
            doc.AddAuthor("Mike Klausnitzer");
            doc.AddCreator("Sample application using iTextSharp");
            doc.AddKeywords("PDF first try");
            doc.AddSubject("test creating a PDF document");
            doc.AddTitle("First try searchable PDF");

            doc.Open();
            doc.Add(new Paragraph("Hello Mike!"));
            doc.Close();
            writer.Close();
            fs.Close();
        }

        private void buttonCreateSearchPDFOCR_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTimeOffset.Now.DateTime;
            string timeOCRStr = dateTime.Year.ToString();
            timeOCRStr += dateTime.Month.ToString();
            timeOCRStr += dateTime.Day.ToString();
            timeOCRStr += dateTime.Hour.ToString();
            timeOCRStr += dateTime.Minute.ToString();
            timeOCRStr += dateTime.Second.ToString();

            string outPath = ConfigurationManager.AppSettings["ResultDirectory"].ToString(); 

            string searchablePDFFileName = outPath + timeOCRStr + "searchableResult.pdf";

            using(StreamReader reader = new StreamReader(_SelectedFileName))
            {
                string ocrText = string.Empty;
                string line = string.Empty;
                ArrayList koordinaten = null;

                float imageHeight = 0;
                float imageWidth = 0;

                //Line: 'Zur Abredimung: BA-08154711', Bounding polygon {{X=18,Y=20},{X=380,Y=20},{X=380,Y=53},{X=18,Y=53}}
                //Die vier Koordinatenpunkte geben die Lage des Textes im Bild an:
                // { X = 18,Y = 20} und { X = 380,Y = 20} beschreiben die obere Kante.
                // { X = 380,Y = 53} und { X = 18,Y = 53} beschreiben die untere Kante.

                float pdfWidth = PageSize.A4.Width - 50;   // Breite - Rand
                float pdfHeight = PageSize.A4.Height - 60; // Höhe - Rand

                using (FileStream fs = new FileStream(searchablePDFFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30); // A4-Dokument mit Rändern

                    //imageHeight = GetDimension(_SelectedFileName, "Image height");
                    //imageWidth = GetDimension(_SelectedFileName, "Image width");

                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // Add meta information to the document
                    document.AddAuthor("Mike Klausnitzer");
                    document.AddCreator("Sample application using iTextSharp");
                    document.AddKeywords("Searchable PDF creation");
                    document.AddSubject("creating a searchable PDF document");
                    document.AddTitle("searchable PDF: " + searchablePDFFileName);

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.TrimStart(' ').StartsWith("Word: "))
                        {
                            ocrText = GetText(line);
                            koordinaten = GetKoordinaten(line);


                            if (koordinaten != null &&
                                !string.IsNullOrEmpty(ocrText))
                            {
                                int[] obenLinks = new int[2];
                                int[] obenRechts = new int[2];
                                int[] untenLinks = new int[2];
                                int[] untenRechts = new int[2];
                                // Ziel-Breite und -Höhe für das A4-Seitenformat
                                int i = 0;
                                foreach (int[] koordinate in koordinaten)
                                {
                                    if(i == 0)
                                    {
                                        obenLinks[0] = koordinate[0]; //x_lo
                                        obenLinks[1] = koordinate[1]; //y_lo
                                    }
                                    else if(i == 1) 
                                    {
                                        obenRechts[0] = koordinate[0]; //x_ro
                                        obenRechts[1] = koordinate[1]; //y_ro
                                    }
                                    else if (i == 2)
                                    {
                                        untenLinks[0] = koordinate[0]; //x_lu
                                        untenLinks[1] = koordinate[1]; //y_lu
                                    }
                                    else if( i == 3)
                                    {
                                        untenRechts[0] = koordinate[0]; //x_ru
                                        untenRechts[1] = koordinate[0]; //y_ru
                                    }
                                    i++;
                                }

                                // OCR-Koordinaten (Bounding Box) – Umrechnen auf A4-PDF-Größe
                                //Position obere Kante im PDF
                                float x1 = 25 + obenLinks[0]; //Rand links + obenLinks x                                    //18 * pdfWidth / imageWidth; //
                                float y1 = PageSize.A4.Height - 30 - obenLinks[1]; //Höhe Dokument - Rand - obenlinks y     //20 * pdfHeight / imageHeight;
                                float x2 = 25 + obenRechts[0]; //Rand links + obenRechts x                                //380 * pdfWidth / imageWidth;
                                float y2 = PageSize.A4.Height - 30 - obenRechts[1];    //53 * pdfHeight / imageHeight;
                                float x3 = 25 + untenLinks[0];
                                float y3 = PageSize.A4.Height - 30 - untenLinks[1];
                                float x4 = 25 + untenRechts[0];
                                float y4 = PageSize.A4.Height - 30 - untenRechts[1];



                                // Berechne die Breite und Höhe der Box im PDF basierend auf OCR-Koordinaten
                                float boxWidth = x2 - x1;
                                float boxHeight = y1 - y3;

                                // Konvertiere die Koordinaten für das A4-PDF
                                float pdfX = x1;  //25 + x1;                    // Verschiebe nach links um Rand
                                float pdfY = y1; //30 + (pdfHeight - y2);       // Verschiebe nach unten um Rand und invertiere Y-Koordinate

                                // Text hinzufügen
                                PdfContentByte cb = writer.DirectContent;
                                cb.BeginText();
                                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                                cb.SetTextMatrix(pdfX, pdfY);
                                cb.ShowText(ocrText);
                                cb.EndText();

                                // Optional: Bounding-Box zur Veranschaulichung zeichnen
                                cb.SetColorStroke(BaseColor.RED);
                                cb.Rectangle(pdfX, pdfY, boxWidth, boxHeight);
                                cb.Stroke();
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Erforderlicher Param nicht gesetzt!");
                        }
                       
                    }
                    document.Close();
                    reader.Close();
                    writer.Close();
                }

            }
        }

        private void buttonCreateSearchPDFOcrLine_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTimeOffset.Now.DateTime;
            string timeOCRStr = dateTime.Year.ToString();
            timeOCRStr += dateTime.Month.ToString();
            timeOCRStr += dateTime.Day.ToString();
            timeOCRStr += dateTime.Hour.ToString();
            timeOCRStr += dateTime.Minute.ToString();
            timeOCRStr += dateTime.Second.ToString();

            string outPath = ConfigurationManager.AppSettings["ResultDirectory"].ToString();

            string searchablePDFFileName = outPath + timeOCRStr + "searchableResult.pdf";

            using (StreamReader reader = new StreamReader(_SelectedFileName))
            {
                string ocrText = string.Empty;
                string line = string.Empty;
                ArrayList koordinaten = null;

                float imageHeight = 0;
                float imageWidth = 0;

                //Line: 'Zur Abredimung: BA-08154711', Bounding polygon {{X=18,Y=20},{X=380,Y=20},{X=380,Y=53},{X=18,Y=53}}
                //Die vier Koordinatenpunkte geben die Lage des Textes im Bild an:
                // { X = 18,Y = 20} und { X = 380,Y = 20} beschreiben die obere Kante.
                // { X = 380,Y = 53} und { X = 18,Y = 53} beschreiben die untere Kante.

                float pdfWidth = PageSize.A4.Width - 50;   // Breite - Rand
                float pdfHeight = PageSize.A4.Height - 60; // Höhe - Rand

                using (FileStream fs = new FileStream(searchablePDFFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30); // A4-Dokument mit Rändern

                    //imageHeight = GetDimension(_SelectedFileName, "Image height");
                    //imageWidth = GetDimension(_SelectedFileName, "Image width");

                    PdfWriter writer = PdfWriter.GetInstance(document, fs);
                    document.Open();

                    // Add meta information to the document
                    document.AddAuthor("Mike Klausnitzer");
                    document.AddCreator("Sample application using iTextSharp");
                    document.AddKeywords("Searchable PDF creation");
                    document.AddSubject("creating a searchable PDF document");
                    document.AddTitle("searchable PDF: " + searchablePDFFileName);

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.TrimStart(' ').StartsWith("Line: "))
                        {
                            ocrText = GetText(line);
                            koordinaten = GetKoordinaten(line);


                            if (koordinaten != null &&
                                !string.IsNullOrEmpty(ocrText))
                            {
                                int[] obenLinks = new int[2];
                                int[] obenRechts = new int[2];
                                int[] untenLinks = new int[2];
                                int[] untenRechts = new int[2];
                                // Ziel-Breite und -Höhe für das A4-Seitenformat
                                int i = 0;
                                foreach (int[] koordinate in koordinaten)
                                {
                                    if (i == 0)
                                    {
                                        obenLinks[0] = koordinate[0]; //x_lo
                                        obenLinks[1] = koordinate[1]; //y_lo
                                    }
                                    else if (i == 1)
                                    {
                                        obenRechts[0] = koordinate[0]; //x_ro
                                        obenRechts[1] = koordinate[1]; //y_ro
                                    }
                                    else if (i == 2)
                                    {
                                        untenLinks[0] = koordinate[0]; //x_lu
                                        untenLinks[1] = koordinate[1]; //y_lu
                                    }
                                    else if (i == 3)
                                    {
                                        untenRechts[0] = koordinate[0]; //x_ru
                                        untenRechts[1] = koordinate[0]; //y_ru
                                    }
                                    i++;
                                }

                                // OCR-Koordinaten (Bounding Box) – Umrechnen auf A4-PDF-Größe
                                //Position obere Kante im PDF
                                float x1 = 25 + obenLinks[0]; //Rand links + obenLinks x                                    //18 * pdfWidth / imageWidth; //
                                float y1 = PageSize.A4.Height - 30 - obenLinks[1]; //Höhe Dokument - Rand - obenlinks y     //20 * pdfHeight / imageHeight;
                                float x2 = 25 + obenRechts[0]; //Rand links + obenRechts x                                //380 * pdfWidth / imageWidth;
                                float y2 = PageSize.A4.Height - 30 - obenRechts[1];    //53 * pdfHeight / imageHeight;
                                float x3 = 25 + untenLinks[0];
                                float y3 = PageSize.A4.Height - 30 - untenLinks[1];
                                float x4 = 25 + untenRechts[0];
                                float y4 = PageSize.A4.Height - 30 - untenRechts[1];



                                // Berechne die Breite und Höhe der Box im PDF basierend auf OCR-Koordinaten
                                float boxWidth = x2 - x1;
                                float boxHeight = y1 - y3;

                                // Konvertiere die Koordinaten für das A4-PDF
                                float pdfX = x1;  //25 + x1;                    // Verschiebe nach links um Rand
                                float pdfY = y1; //30 + (pdfHeight - y2);       // Verschiebe nach unten um Rand und invertiere Y-Koordinate

                                // Text hinzufügen
                                PdfContentByte cb = writer.DirectContent;
                                cb.BeginText();
                                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                                cb.SetTextMatrix(pdfX, pdfY);
                                cb.ShowText(ocrText);
                                cb.EndText();

                                // Optional: Bounding-Box zur Veranschaulichung zeichnen
                                cb.SetColorStroke(BaseColor.RED);
                                cb.Rectangle(pdfX, pdfY, boxWidth, boxHeight);
                                cb.Stroke();
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Erforderlicher Param nicht gesetzt!");
                        }

                    }
                    document.Close();
                    reader.Close();
                    writer.Close();
                }

            }
        }

        private void buttonCreatePDFFullText_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTimeOffset.Now.DateTime;
            string timeOCRStr = dateTime.Year.ToString();
            timeOCRStr += dateTime.Month.ToString();
            timeOCRStr += dateTime.Day.ToString();
            timeOCRStr += dateTime.Hour.ToString();
            timeOCRStr += dateTime.Minute.ToString();
            timeOCRStr += dateTime.Second.ToString();

            string outPath = ConfigurationManager.AppSettings["ResultDirectory"].ToString();

            string searchablePDFFileName = outPath + timeOCRStr + "searchableResult.pdf";

            string ocrText = File.ReadAllText(_SelectedFileName).Replace("\n", Environment.NewLine); ;

            using (FileStream fs = new FileStream(searchablePDFFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30); // A4-Dokument mit Rändern

                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Add meta information to the document
                document.AddAuthor("Mike Klausnitzer");
                document.AddCreator("Sample application using iTextSharp");
                document.AddKeywords("Searchable PDF creation");
                document.AddSubject("creating a searchable PDF document");
                document.AddTitle("searchable PDF: " + searchablePDFFileName);

                // Berechne die Breite und Höhe der Box im PDF basierend auf OCR-Koordinaten
                float boxWidth = PageSize.A4.Width - 50;
                float boxHeight = PageSize.A4.Height -60; //Searchbox füllt dann eigentlich die komplette Seite...

                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                float pdfX = 30;
                float pdfY = 600;
                float lineHeight = 12; // Höhe zwischen den Zeilen
                                       // Text in einzelne Zeilen aufteilen und zeichnen
                foreach (var line in ocrText.Split('\n'))
                {
                    cb.SetTextMatrix(pdfX, pdfY);
                    cb.ShowText(line);
                    pdfY -= lineHeight; // Nächste Zeile tiefer positionieren
                }
                cb.EndText();

                // Optional: Bounding-Box zur Veranschaulichung zeichnen
                //cb.SetColorStroke(BaseColor.RED);
                //cb.Rectangle(pdfX, pdfY, boxWidth, boxHeight);
                //cb.Stroke();

                document.Close();
                fs.Close();
                writer.Close();

            }

        }

        private string GetText(string line)
        {
            string extractedText = string.Empty;
            // Regulärer Ausdruck zum Extrahieren des Textes zwischen Hochkommas
            string pattern = @"'([^']*)'";
            Match match = Regex.Match(line, pattern);
            if (match.Success)
            {
                extractedText = match.Groups[1].Value;
            }
            else
            {
                MessageBox.Show("Kein Text gefunden.");
            }

            return extractedText;
        }

        private float GetDimension(string filename, string nameOfDeminsion)
        {
            float dimension = 0;
            string line = string.Empty;
            string extractedText = string.Empty;

            using (StreamReader reader = new StreamReader(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.TrimStart(' ').StartsWith(nameOfDeminsion))
                    {
                        // Regulärer Ausdruck zum Extrahieren des Textes zwischen Hochkommas
                        string pattern = @"\b\d{1,4}\b";
                        Match match = Regex.Match(line, pattern);
                        if (match.Success)
                        {
                            extractedText = match.Value;
                            float.TryParse(extractedText, out dimension);
                        }
                        else
                        {
                            MessageBox.Show("Kein Höhe/Breite gefunden!");
                        }
                        break;
                    }
                }
                reader.Close();
            }
            return dimension;
        }

        private ArrayList GetKoordinaten(string line)
        {
            string pattern = @"X=(\d+),Y=(\d+)";

            ArrayList coordinates = new ArrayList();
            MatchCollection matches = Regex.Matches(line, pattern);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    coordinates.Add(new int[] { x, y });
                }
            }
            return coordinates;
        }

        
    }
}


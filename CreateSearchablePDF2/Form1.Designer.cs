using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CreateSearchablePDF2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonSelectOCRResult = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCreateSearchPDF = new System.Windows.Forms.Button();
            this.buttonCreateSearchPDFOCR = new System.Windows.Forms.Button();
            this.buttonCreateSearchPDFOcrLine = new System.Windows.Forms.Button();
            this.buttonCreatePDFFullText = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSelectOCRResult
            // 
            this.buttonSelectOCRResult.Location = new System.Drawing.Point(38, 22);
            this.buttonSelectOCRResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSelectOCRResult.Name = "buttonSelectOCRResult";
            this.buttonSelectOCRResult.Size = new System.Drawing.Size(397, 23);
            this.buttonSelectOCRResult.TabIndex = 0;
            this.buttonSelectOCRResult.Text = "Select OCR Result...";
            this.buttonSelectOCRResult.UseVisualStyleBackColor = true;
            this.buttonSelectOCRResult.Click += new System.EventHandler(this.buttonSelectOCRResult_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(45, 261);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(390, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCreateSearchPDF
            // 
            this.buttonCreateSearchPDF.Location = new System.Drawing.Point(45, 66);
            this.buttonCreateSearchPDF.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCreateSearchPDF.Name = "buttonCreateSearchPDF";
            this.buttonCreateSearchPDF.Size = new System.Drawing.Size(390, 23);
            this.buttonCreateSearchPDF.TabIndex = 2;
            this.buttonCreateSearchPDF.Text = "Create searchable PDF...";
            this.buttonCreateSearchPDF.UseVisualStyleBackColor = true;
            this.buttonCreateSearchPDF.Click += new System.EventHandler(this.buttonCreateSearchPDF_Click);
            // 
            // buttonCreateSearchPDFOCR
            // 
            this.buttonCreateSearchPDFOCR.Location = new System.Drawing.Point(45, 115);
            this.buttonCreateSearchPDFOCR.Name = "buttonCreateSearchPDFOCR";
            this.buttonCreateSearchPDFOCR.Size = new System.Drawing.Size(390, 23);
            this.buttonCreateSearchPDFOCR.TabIndex = 3;
            this.buttonCreateSearchPDFOCR.Text = "Create searchable PDT from OCR Word";
            this.buttonCreateSearchPDFOCR.UseVisualStyleBackColor = true;
            this.buttonCreateSearchPDFOCR.Click += new System.EventHandler(this.buttonCreateSearchPDFOCR_Click);
            // 
            // buttonCreateSearchPDFOcrLine
            // 
            this.buttonCreateSearchPDFOcrLine.Location = new System.Drawing.Point(45, 163);
            this.buttonCreateSearchPDFOcrLine.Name = "buttonCreateSearchPDFOcrLine";
            this.buttonCreateSearchPDFOcrLine.Size = new System.Drawing.Size(390, 23);
            this.buttonCreateSearchPDFOcrLine.TabIndex = 4;
            this.buttonCreateSearchPDFOcrLine.Text = "Create searchable PDF from OCR Line";
            this.buttonCreateSearchPDFOcrLine.UseVisualStyleBackColor = true;
            this.buttonCreateSearchPDFOcrLine.Click += new System.EventHandler(this.buttonCreateSearchPDFOcrLine_Click);
            // 
            // buttonCreatePDFFullText
            // 
            this.buttonCreatePDFFullText.Location = new System.Drawing.Point(45, 215);
            this.buttonCreatePDFFullText.Name = "buttonCreatePDFFullText";
            this.buttonCreatePDFFullText.Size = new System.Drawing.Size(390, 23);
            this.buttonCreatePDFFullText.TabIndex = 5;
            this.buttonCreatePDFFullText.Text = "Crreate Searchable PDF from Fulltext";
            this.buttonCreatePDFFullText.UseVisualStyleBackColor = true;
            this.buttonCreatePDFFullText.Click += new System.EventHandler(this.buttonCreatePDFFullText_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 295);
            this.Controls.Add(this.buttonCreatePDFFullText);
            this.Controls.Add(this.buttonCreateSearchPDFOcrLine);
            this.Controls.Add(this.buttonCreateSearchPDFOCR);
            this.Controls.Add(this.buttonCreateSearchPDF);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSelectOCRResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Create Searchable PDF";
            this.ResumeLayout(false);

        }

        #endregion

        private Button buttonSelectOCRResult;
        private Button buttonClose;
        private Button buttonCreateSearchPDF;
        private Button buttonCreateSearchPDFOCR;
        private Button buttonCreateSearchPDFOcrLine;
        private Button buttonCreatePDFFullText;
    }
}


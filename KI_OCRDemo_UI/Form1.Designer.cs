namespace KI_OCRDemo_UI
{
    partial class mainFormOCRDemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainFormOCRDemo));
            buttonClose = new Button();
            buttonSelectFile = new Button();
            buttonDoOCR = new Button();
            labelFileInProcess = new Label();
            labelSelectedFile = new Label();
            pictureBoxSelectedFile = new PictureBox();
            textBoxResultOCR = new TextBox();
            checkBoxKIVision = new CheckBox();
            checkBoxTextract = new CheckBox();
            buttonOptimizeGPT = new Button();
            textBoxGPTResult = new TextBox();
            buttonDirectReadingGPT = new Button();
            textBoxGPTPrompt = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSelectedFile).BeginInit();
            SuspendLayout();
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(2191, 41);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(94, 29);
            buttonClose.TabIndex = 0;
            buttonClose.Text = "close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // buttonSelectFile
            // 
            buttonSelectFile.Location = new Point(25, 38);
            buttonSelectFile.Name = "buttonSelectFile";
            buttonSelectFile.Size = new Size(94, 29);
            buttonSelectFile.TabIndex = 1;
            buttonSelectFile.Text = "select file...";
            buttonSelectFile.UseVisualStyleBackColor = true;
            buttonSelectFile.Click += buttonSelectFile_Click;
            // 
            // buttonDoOCR
            // 
            buttonDoOCR.Location = new Point(140, 38);
            buttonDoOCR.Name = "buttonDoOCR";
            buttonDoOCR.Size = new Size(94, 29);
            buttonDoOCR.TabIndex = 2;
            buttonDoOCR.Text = "do OCR...";
            buttonDoOCR.UseVisualStyleBackColor = true;
            buttonDoOCR.Click += buttonDoOCR_Click;
            // 
            // labelFileInProcess
            // 
            labelFileInProcess.AutoSize = true;
            labelFileInProcess.ForeColor = SystemColors.Info;
            labelFileInProcess.Location = new Point(28, 87);
            labelFileInProcess.Name = "labelFileInProcess";
            labelFileInProcess.Size = new Size(102, 20);
            labelFileInProcess.TabIndex = 3;
            labelFileInProcess.Text = "File in process";
            // 
            // labelSelectedFile
            // 
            labelSelectedFile.AutoSize = true;
            labelSelectedFile.ForeColor = SystemColors.Info;
            labelSelectedFile.Location = new Point(140, 87);
            labelSelectedFile.Name = "labelSelectedFile";
            labelSelectedFile.Size = new Size(54, 20);
            labelSelectedFile.TabIndex = 4;
            labelSelectedFile.Text = "not set";
            // 
            // pictureBoxSelectedFile
            // 
            pictureBoxSelectedFile.Location = new Point(32, 132);
            pictureBoxSelectedFile.Name = "pictureBoxSelectedFile";
            pictureBoxSelectedFile.Size = new Size(687, 817);
            pictureBoxSelectedFile.TabIndex = 5;
            pictureBoxSelectedFile.TabStop = false;
            // 
            // textBoxResultOCR
            // 
            textBoxResultOCR.BackColor = SystemColors.AppWorkspace;
            textBoxResultOCR.Location = new Point(742, 131);
            textBoxResultOCR.Multiline = true;
            textBoxResultOCR.Name = "textBoxResultOCR";
            textBoxResultOCR.ReadOnly = true;
            textBoxResultOCR.Size = new Size(723, 818);
            textBoxResultOCR.TabIndex = 6;
            // 
            // checkBoxKIVision
            // 
            checkBoxKIVision.AutoSize = true;
            checkBoxKIVision.ForeColor = SystemColors.ControlLightLight;
            checkBoxKIVision.Location = new Point(257, 41);
            checkBoxKIVision.Name = "checkBoxKIVision";
            checkBoxKIVision.Size = new Size(121, 24);
            checkBoxKIVision.TabIndex = 9;
            checkBoxKIVision.Text = "KI Vision OCR";
            checkBoxKIVision.UseVisualStyleBackColor = true;
            checkBoxKIVision.CheckedChanged += checkBoxKIVision_CheckedChanged;
            // 
            // checkBoxTextract
            // 
            checkBoxTextract.AutoSize = true;
            checkBoxTextract.ForeColor = SystemColors.ControlLightLight;
            checkBoxTextract.Location = new Point(384, 41);
            checkBoxTextract.Name = "checkBoxTextract";
            checkBoxTextract.Size = new Size(116, 24);
            checkBoxTextract.TabIndex = 10;
            checkBoxTextract.Text = "Textract OCR";
            checkBoxTextract.UseVisualStyleBackColor = true;
            checkBoxTextract.CheckedChanged += checkBoxTextract_CheckedChanged;
            // 
            // buttonOptimizeGPT
            // 
            buttonOptimizeGPT.Location = new Point(1504, 36);
            buttonOptimizeGPT.Name = "buttonOptimizeGPT";
            buttonOptimizeGPT.Size = new Size(212, 29);
            buttonOptimizeGPT.TabIndex = 11;
            buttonOptimizeGPT.Text = "optimize with ChatGPT...";
            buttonOptimizeGPT.UseVisualStyleBackColor = true;
            buttonOptimizeGPT.Click += buttonOptimizeGPT_Click;
            // 
            // textBoxGPTResult
            // 
            textBoxGPTResult.AcceptsReturn = true;
            textBoxGPTResult.BackColor = SystemColors.GradientInactiveCaption;
            textBoxGPTResult.Location = new Point(1504, 265);
            textBoxGPTResult.Multiline = true;
            textBoxGPTResult.Name = "textBoxGPTResult";
            textBoxGPTResult.ReadOnly = true;
            textBoxGPTResult.Size = new Size(781, 684);
            textBoxGPTResult.TabIndex = 12;
            // 
            // buttonDirectReadingGPT
            // 
            buttonDirectReadingGPT.Location = new Point(1760, 38);
            buttonDirectReadingGPT.Name = "buttonDirectReadingGPT";
            buttonDirectReadingGPT.Size = new Size(234, 29);
            buttonDirectReadingGPT.TabIndex = 13;
            buttonDirectReadingGPT.Text = "direct reading with ChatGPT...";
            buttonDirectReadingGPT.UseVisualStyleBackColor = true;
            buttonDirectReadingGPT.Click += buttonDirectReadingGPT_Click;
            // 
            // textBoxGPTPrompt
            // 
            textBoxGPTPrompt.Location = new Point(1504, 164);
            textBoxGPTPrompt.Multiline = true;
            textBoxGPTPrompt.Name = "textBoxGPTPrompt";
            textBoxGPTPrompt.Size = new Size(781, 80);
            textBoxGPTPrompt.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(1504, 131);
            label1.Name = "label1";
            label1.Size = new Size(88, 20);
            label1.TabIndex = 15;
            label1.Text = "GPT Prompt";
            // 
            // mainFormOCRDemo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(2313, 981);
            Controls.Add(label1);
            Controls.Add(textBoxGPTPrompt);
            Controls.Add(buttonDirectReadingGPT);
            Controls.Add(textBoxGPTResult);
            Controls.Add(buttonOptimizeGPT);
            Controls.Add(checkBoxTextract);
            Controls.Add(checkBoxKIVision);
            Controls.Add(textBoxResultOCR);
            Controls.Add(pictureBoxSelectedFile);
            Controls.Add(labelSelectedFile);
            Controls.Add(labelFileInProcess);
            Controls.Add(buttonDoOCR);
            Controls.Add(buttonSelectFile);
            Controls.Add(buttonClose);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mainFormOCRDemo";
            Text = "KODO";
            ((System.ComponentModel.ISupportInitialize)pictureBoxSelectedFile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonClose;
        private Button buttonSelectFile;
        private Button buttonDoOCR;
        private Label labelFileInProcess;
        private Label labelSelectedFile;
        private PictureBox pictureBoxSelectedFile;
        private TextBox textBoxResultOCR;
        private CheckBox checkBoxKIVision;
        private CheckBox checkBoxTextract;
        private Button buttonOptimizeGPT;
        private TextBox textBoxGPTResult;
        private Button buttonDirectReadingGPT;
        private TextBox textBoxGPTPrompt;
        private Label label1;
    }
}

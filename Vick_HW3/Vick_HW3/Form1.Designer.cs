namespace Vick_HW3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFlileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFibonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFibonaciiNumbersFirst100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 24);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(284, 237);
            this.textBox1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromFlileToolStripMenuItem,
            this.loadFibonToolStripMenuItem,
            this.loadFibonaciiNumbersFirst100ToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadFromFlileToolStripMenuItem
            // 
            this.loadFromFlileToolStripMenuItem.Name = "loadFromFlileToolStripMenuItem";
            this.loadFromFlileToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.loadFromFlileToolStripMenuItem.Text = "Load from File...";
            this.loadFromFlileToolStripMenuItem.Click += new System.EventHandler(this.loadFile_Click);
            // 
            // loadFibonToolStripMenuItem
            // 
            this.loadFibonToolStripMenuItem.Name = "loadFibonToolStripMenuItem";
            this.loadFibonToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.loadFibonToolStripMenuItem.Text = "Load Fibonacii numbers (First 50) ";
            this.loadFibonToolStripMenuItem.Click += new System.EventHandler(this.loadFibonToolStripMenuItem_Click);
            // 
            // loadFibonaciiNumbersFirst100ToolStripMenuItem
            // 
            this.loadFibonaciiNumbersFirst100ToolStripMenuItem.Name = "loadFibonaciiNumbersFirst100ToolStripMenuItem";
            this.loadFibonaciiNumbersFirst100ToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.loadFibonaciiNumbersFirst100ToolStripMenuItem.Text = "Load Fibonacii numbers (First 100)";
            this.loadFibonaciiNumbersFirst100ToolStripMenuItem.Click += new System.EventHandler(this.loadFibonaciiNumbersFirst100ToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.saveToolStripMenuItem.Text = "Save to file...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromFlileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFibonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFibonaciiNumbersFirst100ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}


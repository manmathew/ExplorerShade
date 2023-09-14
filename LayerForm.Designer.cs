namespace ExplorerShade
{
    partial class LayerForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerForm));
            notifyIcon1 = new NotifyIcon(components);
            contextMenu = new ContextMenuStrip(components);
            exitToolStripMenuItem = new ToolStripMenuItem();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "ExplorerShade";
            notifyIcon1.Visible = true;
            // 
            // contextMenu
            // 
            contextMenu.ImageScalingSize = new Size(32, 32);
            contextMenu.Items.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(301, 86);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(300, 38);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // LayerForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "LayerForm";
            Text = "Form1";
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}
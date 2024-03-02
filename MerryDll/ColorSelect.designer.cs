
namespace MerryDllFramework
{
    partial class ColorSelect
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
            this.cboxBlack = new System.Windows.Forms.CheckBox();
            this.cboxWhite = new System.Windows.Forms.CheckBox();
            this.cboxBlue = new System.Windows.Forms.CheckBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Lara = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cboxBlack
            // 
            this.cboxBlack.AutoSize = true;
            this.cboxBlack.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxBlack.Location = new System.Drawing.Point(54, 193);
            this.cboxBlack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboxBlack.Name = "cboxBlack";
            this.cboxBlack.Size = new System.Drawing.Size(264, 52);
            this.cboxBlack.TabIndex = 0;
            this.cboxBlack.Text = "黑色/ Đen";
            this.cboxBlack.UseVisualStyleBackColor = true;
            this.cboxBlack.CheckedChanged += new System.EventHandler(this.cboxBlack_CheckedChanged);
            this.cboxBlack.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboxBlack_KeyDown);
            // 
            // cboxWhite
            // 
            this.cboxWhite.AutoSize = true;
            this.cboxWhite.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxWhite.Location = new System.Drawing.Point(54, 289);
            this.cboxWhite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboxWhite.Name = "cboxWhite";
            this.cboxWhite.Size = new System.Drawing.Size(306, 52);
            this.cboxWhite.TabIndex = 0;
            this.cboxWhite.Text = "白色 /Trắng";
            this.cboxWhite.UseVisualStyleBackColor = true;
            this.cboxWhite.CheckedChanged += new System.EventHandler(this.cboxWhite_CheckedChanged);
            this.cboxWhite.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboxWhite_KeyDown);
            // 
            // cboxBlue
            // 
            this.cboxBlue.AutoSize = true;
            this.cboxBlue.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboxBlue.Location = new System.Drawing.Point(54, 388);
            this.cboxBlue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboxBlue.Name = "cboxBlue";
            this.cboxBlue.Size = new System.Drawing.Size(382, 52);
            this.cboxBlue.TabIndex = 0;
            this.cboxBlue.Text = "蓝色/ Xanh lam";
            this.cboxBlue.UseVisualStyleBackColor = true;
            this.cboxBlue.CheckedChanged += new System.EventHandler(this.cboxBlue_CheckedChanged);
            this.cboxBlue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboxBlue_KeyDown);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelect.Font = new System.Drawing.Font("SimSun", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.ForeColor = System.Drawing.Color.Black;
            this.btnSelect.Location = new System.Drawing.Point(522, 241);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelect.Size = new System.Drawing.Size(194, 120);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "OK";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            this.btnSelect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSelect_KeyDown);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(81, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(662, 148);
            this.label1.TabIndex = 2;
            this.label1.Text = "请选择耳机颜色/ Vui lòng chọn màu sắc của tai nghe";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Lara
            // 
            this.Lara.AutoSize = true;
            this.Lara.Font = new System.Drawing.Font("SimSun", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lara.Location = new System.Drawing.Point(54, 478);
            this.Lara.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Lara.Name = "Lara";
            this.Lara.Size = new System.Drawing.Size(142, 52);
            this.Lara.TabIndex = 3;
            this.Lara.Text = "Lara";
            this.Lara.UseVisualStyleBackColor = true;
            this.Lara.CheckedChanged += new System.EventHandler(this.Lara_CheckedChanged);
            // 
            // ColorSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 597);
            this.Controls.Add(this.Lara);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cboxBlue);
            this.Controls.Add(this.cboxWhite);
            this.Controls.Add(this.cboxBlack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ColorSelect";
            this.Padding = new System.Windows.Forms.Padding(11, 13, 11, 13);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ColorSelect";
            this.Load += new System.EventHandler(this.ColorSelect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cboxBlack;
        private System.Windows.Forms.CheckBox cboxWhite;
        private System.Windows.Forms.CheckBox cboxBlue;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox Lara;
    }
}
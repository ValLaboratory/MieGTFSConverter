
namespace MieGTFSConverter {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.GtfsTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RoutesConvertBtn = new System.Windows.Forms.Button();
            this.StopsConvertBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.GtfsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GtfsTextBox
            // 
            this.GtfsTextBox.AllowDrop = true;
            this.GtfsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MieGTFSConverter.Properties.Settings.Default, "gtfsDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.GtfsTextBox.Location = new System.Drawing.Point(59, 52);
            this.GtfsTextBox.Name = "GtfsTextBox";
            this.GtfsTextBox.Size = new System.Drawing.Size(429, 19);
            this.GtfsTextBox.TabIndex = 0;
            this.GtfsTextBox.Text = global::MieGTFSConverter.Properties.Settings.Default.gtfsDir;
            this.GtfsTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.GtfsTextBox_DragDrop);
            this.GtfsTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.GtfsTextBox_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "GTFS";
            // 
            // RoutesConvertBtn
            // 
            this.RoutesConvertBtn.Location = new System.Drawing.Point(59, 119);
            this.RoutesConvertBtn.Name = "RoutesConvertBtn";
            this.RoutesConvertBtn.Size = new System.Drawing.Size(134, 34);
            this.RoutesConvertBtn.TabIndex = 2;
            this.RoutesConvertBtn.Text = "routes.txt 変換";
            this.RoutesConvertBtn.UseVisualStyleBackColor = true;
            this.RoutesConvertBtn.Click += new System.EventHandler(this.RoutesConvertBtn_Click);
            // 
            // StopsConvertBtn
            // 
            this.StopsConvertBtn.Location = new System.Drawing.Point(410, 119);
            this.StopsConvertBtn.Name = "StopsConvertBtn";
            this.StopsConvertBtn.Size = new System.Drawing.Size(134, 34);
            this.StopsConvertBtn.TabIndex = 3;
            this.StopsConvertBtn.Text = "stops.txt 変換";
            this.StopsConvertBtn.UseVisualStyleBackColor = true;
            this.StopsConvertBtn.Click += new System.EventHandler(this.StopsConvertBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(298, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "route_long_name _  jp_parent_route_id → route_short_name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(408, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "緯度、経度をODのデータから取得";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(57, 244);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(37, 12);
            this.StatusLabel.TabIndex = 6;
            this.StatusLabel.Text = "status";
            // 
            // GtfsBtn
            // 
            this.GtfsBtn.Location = new System.Drawing.Point(501, 48);
            this.GtfsBtn.Name = "GtfsBtn";
            this.GtfsBtn.Size = new System.Drawing.Size(43, 27);
            this.GtfsBtn.TabIndex = 7;
            this.GtfsBtn.Text = "…";
            this.GtfsBtn.UseVisualStyleBackColor = true;
            this.GtfsBtn.Click += new System.EventHandler(this.GtfsBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 289);
            this.Controls.Add(this.GtfsBtn);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StopsConvertBtn);
            this.Controls.Add(this.RoutesConvertBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GtfsTextBox);
            this.Name = "Form1";
            this.Text = "三重GTFSコンバーター";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox GtfsTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RoutesConvertBtn;
        private System.Windows.Forms.Button StopsConvertBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button GtfsBtn;
    }
}


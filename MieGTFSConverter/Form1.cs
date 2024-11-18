using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MieGTFSConverter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            this.Text += " v" + Version();

            StatusLabel.Text = "";
        }

        /// <summary>
        /// ツールバージョン
        /// </summary>
        /// <returns></returns>
        private string Version() {
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return ver.FileVersion;
        }

        private void GtfsTextBox_DragDrop(object sender, DragEventArgs e) {
            // ドロップされたすべてのファイル名を取得する
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileName.Length <= 0) {
                return;
            }
            GtfsTextBox.Text = fileName[0];
        }

        private void GtfsTextBox_DragEnter(object sender, DragEventArgs e) {
            // 出力フォルダテキストボックス内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            else
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
        }

        private void GtfsBtn_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "GTFSフォルダを選択して、OKボタンを押してください。";
            if (GtfsTextBox.Text == "") {
                if (System.IO.File.Exists(GtfsTextBox.Text)) {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(GtfsTextBox.Text);
                    fbd.SelectedPath = fileInfo.DirectoryName;
                }
            } else {
                fbd.SelectedPath = GtfsTextBox.Text;
            }

        }

        private async void RoutesConvertBtn_Click(object sender, EventArgs e) {

            Progress<string> progress = new Progress<string>(onProgressChanged);
            IProgress<string> iProgress = (IProgress<string>)progress;
            FileConverter fileConverter = new FileConverter(iProgress);

            string gtfsPath = GtfsTextBox.Text.Trim();

            try {

                RoutesConvertBtn.Enabled = false;


                iProgress.Report("routes.txt 変換 開始");

                if (GtfsTextBox.Text.Trim() == "") {
                    GtfsTextBox.Focus();
                    throw new Exception("GTFS が指定されてません。");
                }


                if (!Directory.Exists(gtfsPath)) {
                    GtfsTextBox.Focus();
                    throw new Exception(gtfsPath + " は存在しません。");
                }

                StatusLabel.Text = "GTFS読み込み";

                fileConverter.ConvertRoutesTxt(gtfsPath + "\\routes.txt");

            } catch (Exception ex) {
                StatusLabel.Text = "routes.txt 変換 失敗\n";
                StatusLabel.Text += ex.Message;

                MessageBox.Show(ex.Message);

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            try {

                iProgress.Report("stops.txt 変換 開始");

                if (GtfsTextBox.Text.Trim() == "") {
                    GtfsTextBox.Focus();
                    throw new Exception("GTFS が指定されてません。");
                }

                if (!Directory.Exists(gtfsPath)) {
                    GtfsTextBox.Focus();
                    throw new Exception(gtfsPath + " は存在しません。");
                }

                await Task.Run(() => {
                    fileConverter.ConvertStopsTxt(gtfsPath + "\\stops.txt");
                });

            } catch (Exception ex) {
                StatusLabel.Text = "stops.txt 変換 失敗\n";
                StatusLabel.Text += ex.Message;

                MessageBox.Show(ex.Message);

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);


            } finally {
                RoutesConvertBtn.Enabled = true;
            }
        }


        // 進捗通知を受けたらラベルに表示
        private void onProgressChanged(string txt) {
            StatusLabel.Text = txt;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Properties.Settings.Default.Save();	// 入力項目を保存
        }
    }
}

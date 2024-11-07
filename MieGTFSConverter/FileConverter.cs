using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MieGTFSConverter {
    public class FileConverter {


        private IProgress<string> iProgress;

        public FileConverter(IProgress<string> _iProgress) {
            iProgress = _iProgress;
        }

        public void ConvertRoutesTxt(string routesTxtPath) {
            if ( ! File.Exists(routesTxtPath) ) {
                throw new Exception(routesTxtPath + " がありません。");
            }

            // routes.txtと同じ場所に routes_変換済.txt を出力
            string outDir = Path.GetDirectoryName(routesTxtPath);
            StreamWriter sw = new StreamWriter(outDir + "\\routes_変換済.txt", false, Encoding.UTF8);


            using (var parser = new TextFieldParser(routesTxtPath)) {

                parser.HasFieldsEnclosedInQuotes = true;
                parser.Delimiters = new string[] { "," };

                // 1行読込
                string[] fields = parser.ReadFields();

                int route_short_name_idx = Array.IndexOf(fields, "route_short_name");
                int route_long_name_idx = Array.IndexOf(fields, "route_long_name");
                int jp_parent_route_id_idx = Array.IndexOf(fields, "jp_parent_route_id");

                if (route_short_name_idx == -1) {
                    throw new Exception("routes.txt の1行目にroute_short_nameがありません。");
                }
                if (route_long_name_idx == -1) {
                    throw new Exception("routes.txt の1行目にroute_long_nameがありません。");
                }
                if (jp_parent_route_id_idx == -1) {
                    throw new Exception("routes.txt の1行目にjp_parent_route_idがありません。");
                }

                sw.WriteLine(string.Join(",",fields));

                // 2行目以降
                int rowCnt = 2;
                while (!parser.EndOfData) {

                    iProgress.Report(rowCnt + "行目読込");
                    fields = parser.ReadFields();

                    fields[route_short_name_idx] = fields[route_long_name_idx] + "_" + fields[jp_parent_route_id_idx];

                    sw.WriteLine(string.Join(",", fields));

                    rowCnt++;
                }

            }

            sw.Close();

            iProgress.Report("下記を出力\n" + outDir + "\\routes_変換済.txt");
        }

        public void ConvertStopsTxt(string stopsTxtPath) {
            if (!File.Exists(stopsTxtPath)) {
                throw new Exception(stopsTxtPath + " がありません。");
            }

            iProgress.Report("zipをダウンロード");

            string outDir = Path.GetDirectoryName(stopsTxtPath);

            if (Directory.Exists(outDir + "\\tmp")) {
                Directory.Delete(outDir + "\\tmp",true);
            }


            string url = "https://bus-vision.jp/sanco/view/opendataSanco.html";
            // urlのHTMLテキストを読み込む
            WebClient webClient = new WebClient();
            byte[] sourceData = webClient.DownloadData(url);

            //バイト配列を文字列に変換
            string source = System.Text.Encoding.UTF8.GetString(sourceData);

            // outDirの下にtmpフォルダを作成
            if ( ! Directory.Exists(outDir + "\\tmp") ) {
                Directory.CreateDirectory(outDir + "\\tmp");
            }

            int no = 0;
            // 下記パターンを抽出
            // https://bus-vision.jp/gtfs_v2/kuwana/gtfsFeed
            MatchCollection results = Regex.Matches(source, "https://bus-vision.jp/gtfs_v2/.*/gtfsFeed");
            foreach (Match m in results) {
                no++;
                string gtfsURL = m.Value; // 発見した文字列
                byte[] data = webClient.DownloadData(gtfsURL);
                using (FileStream fs = new FileStream($"{outDir}\\tmp\\{no}.zip", FileMode.Create, FileAccess.ReadWrite)) {
                    fs.Write(data, 0, data.Length);
                }
            }
            webClient.Dispose();

            // tmp/*.zipを解凍
            iProgress.Report("zipを解凍");
            for (int i=1;i<=no;i++) {
                var file = $"{outDir}\\tmp\\{i}.zip";
                using (ZipArchive archive = ZipFile.OpenRead(file)) {
                    var f = archive.GetEntry("stops.txt");
                    var ms = f.Open();
                    using (var fileStream = new FileStream($"{outDir}\\tmp\\{i}_stops.txt",
                        FileMode.CreateNew, FileAccess.ReadWrite)) {
                        ms.CopyTo(fileStream);
                    }
                }
            }



            // tmp/*_stops.txtを読み込んで キー stop_id, 値 stop_lat,stop_lon を辞書に登録
            Dictionary<string,string[]> latLonDic = new Dictionary<string, string[]>();
            for (int i = 1; i <= no; i++) {
                var file = $"{outDir}\\tmp\\{i}_stops.txt";

                using (var parser = new TextFieldParser(file)) {

                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.Delimiters = new string[] { "," };

                    // 1行読込
                    string[] fields = parser.ReadFields();

                    int stop_id_idx = Array.IndexOf(fields, "stop_id");
                    int stop_lat_idx = Array.IndexOf(fields, "stop_lat");
                    int stop_lon_idx = Array.IndexOf(fields, "stop_lon");

                    if (stop_id_idx == -1) {
                        throw new Exception("stops.txt の1行目にstop_idがありません。");
                    }
                    if (stop_lat_idx == -1) {
                        throw new Exception("stops.txt の1行目にstop_latがありません。");
                    }
                    if (stop_lon_idx == -1) {
                        throw new Exception("stops.txt の1行目にstop_lonがありません。");
                    }


                    // 2行目以降
                    int rowCnt = 2;
                    while (!parser.EndOfData) {

                        iProgress.Report(rowCnt + "行目読込");
                        fields = parser.ReadFields();

                        string stop_id = fields[stop_id_idx];
                        string stop_lat = fields[stop_lat_idx];
                        string stop_lon = fields[stop_lon_idx];

                        if ( ! latLonDic.ContainsKey(stop_id) ) {
                            latLonDic.Add(stop_id,new string[]{stop_lat,stop_lon});
                        }
                        rowCnt++;
                    }
                }
            }

            // stops.txtと同じ場所に routes_変換済.txt を出力
            StreamWriter sw = new StreamWriter(outDir + "\\stops_変換済.txt", false, Encoding.UTF8);


            // stops.txtを読み込み、stop_latとstop_lonを埋める
            using (var parser = new TextFieldParser(outDir + "\\stops.txt")) {

                parser.HasFieldsEnclosedInQuotes = true;
                parser.Delimiters = new string[] { "," };

                // 1行読込
                string[] fields = parser.ReadFields();

                int stop_id_idx = Array.IndexOf(fields, "stop_id");
                int stop_lat_idx = Array.IndexOf(fields, "stop_lat");
                int stop_lon_idx = Array.IndexOf(fields, "stop_lon");

                if (stop_id_idx == -1) {
                    throw new Exception("stops.txt の1行目にstop_idがありません。");
                }
                if (stop_lat_idx == -1) {
                    throw new Exception("stops.txt の1行目にstop_latがありません。");
                }
                if (stop_lon_idx == -1) {
                    throw new Exception("stops.txt の1行目にstop_lonがありません。");
                }

                sw.WriteLine(string.Join(",",fields));


                // 2行目以降
                int rowCnt = 2;
                while (!parser.EndOfData) {

                    iProgress.Report(rowCnt + "行目読込");
                    fields = parser.ReadFields();

                    string stop_id = fields[stop_id_idx].Replace('-','_');

                    if (latLonDic.ContainsKey(stop_id)) {
                        string[] latLon = latLonDic[stop_id];
                        fields[stop_lat_idx] = latLon[0];
                        fields[stop_lon_idx] = latLon[1];
                    }

                    sw.WriteLine(string.Join(",", fields));

                    rowCnt++;
                }
            }

            sw.Close();

            iProgress.Report("下記を出力\n" + outDir + "\\stopss_変換済.txt");

        }
    }
}
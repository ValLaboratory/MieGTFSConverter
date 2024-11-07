using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MieGTFSConverter {
    public class FileConverter {


        private IProgress<string> iProgress;

        public FileConverter(IProgress<string> _iProgress) {
            iProgress = _iProgress;
        }

        public void Convert(string routesTxtPath) {
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
    }
}

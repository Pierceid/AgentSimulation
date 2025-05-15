using AgentSimulation.Structures;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;

namespace AgentSimulation.Utilities {
    public static class Util {
        public static string FormatRange(object min, object max) {
            return $"<{min.ToString()?.Replace(',', '.')},{max.ToString()?.Replace(',', '.')})";
        }

        public static string FormatTime(double time) {
            int daySeconds = (int)(time % 28800);
            int days = (int)(time / 28800);
            int hours = (daySeconds / 3600) % 8 + 6;
            int minutes = (daySeconds % 3600) / 60;
            int seconds = daySeconds % 60;

            return $"{days:D2}d {hours:D2}h {minutes:D2}m {seconds:D2}s";
        }

        public static string GetFilePath(string fileName) {
            return Path.GetFullPath(Path.Combine(Constants.IMAGE_PATH, fileName));
        }

        public static Config LoadConfig(string fileName, int rowNumber) {
            try {
                string fullPath = Path.Combine(Constants.CONFIG_PATH, "config.xlsx");

                string connStr = $"Provider=Microsoft.ACE.OLEDB.12.0;" +
                                 $"Data Source={fullPath};" +
                                 $"Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                using var connection = new OleDbConnection(connStr);
                connection.Open();

                var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string? sheetName = schema?.Rows[0]["TABLE_NAME"].ToString();

                var command = new OleDbCommand($"SELECT * FROM [{sheetName}]", connection);
                using var adapter = new OleDbDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                if (rowNumber - 1 >= 0 && rowNumber - 1 < table.Rows.Count) {
                    var row = table.Rows[rowNumber - 1];
                    int a = Convert.ToInt32(row[0]);
                    int b = Convert.ToInt32(row[1]);
                    int c = Convert.ToInt32(row[2]);
                    int d = Convert.ToInt32(row[3]);
                    return new Config(500, a, b, c, d);
                }

                throw new Exception($"Failed to load config ({rowNumber}) from file: {fileName}");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}

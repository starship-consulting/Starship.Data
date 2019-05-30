using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;

namespace Starship.Data.Converters {
    public class SpreadsheetConverter {
        
        static SpreadsheetConverter() {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private bool IsCsv(string contentType) {

            switch(contentType.ToLower().Trim()) {

                case "text/x-csv":
                case "text/plain":
                case "text/csv":
                case "application/csv":
                case "application/vnd.ms-excel":
                    return true;
            }

            return false;
        }

        public List<Dictionary<string, object>> Read(Stream stream, string contentType = "") {

            var result = new List<Dictionary<string, object>>();
            IExcelDataReader reader = IsCsv(contentType) ? ExcelReaderFactory.CreateCsvReader(stream) : ExcelReaderFactory.CreateReader(stream);
            
            var table = reader.AsDataSet().Tables[0];
            var columns = new Dictionary<string, string>();

            var index = 0;

            foreach (DataRow datarow in table.Rows) {

                index += 1;

                if(index == 1) {
                    foreach (DataColumn column in table.Columns) {
                        var name = datarow[column].ToString().ToLower();

                        if(string.IsNullOrEmpty(name)) {
                            continue;
                        }

                        columns.Add(column.ColumnName, name);
                    }

                    continue;
                }
                
                result.Add(columns.ToDictionary(column => column.Value, column => datarow[column.Key]));
            }

            reader.Dispose();

            return result;
        }
    }
}
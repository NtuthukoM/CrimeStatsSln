using CrimeStats.Domain;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CrimeStats.Application.Reader
{
    public class CrimeStatReader : ICrimeStatReader
    {
        private string fileName { get; set; } = @"C:\Users\ntuth\source\repos\CrimeStatsSln\CrimeStats.Api\Content\2023-2024-1st_Quarter_WEB.xlsx";
        
        public Task<List<CrimeStat>> ReadCrimeStatsAsync()
        {
            List<CrimeStat> crimeStats = new List<CrimeStat>();
            string sheetName = "Crime stats per component";
            int subCategroryRow = 15;
            for(int i = 0; i < 8; i++)
            {
                var crimeStat = new CrimeStat();
                crimeStat.Catergory = GetCellValue(fileName, sheetName, "C14");
                crimeStat.Catergory = crimeStat.Catergory.Substring(crimeStat.Catergory.LastIndexOf("!") + 4);
                crimeStat.SubCategory = GetCellValue(fileName, sheetName, $"C{subCategroryRow}");
                crimeStat.SubCategory = crimeStat.SubCategory.Substring(crimeStat.SubCategory.LastIndexOf("!") + 4);
                List<CrimeStatPreiod> periods = ReadSummaryPeriods(sheetName, subCategroryRow);
                crimeStat.CrimeStatPreiods = periods;
                crimeStats.Add(crimeStat);
                subCategroryRow++;
            }
            return Task.FromResult(crimeStats);
        }

        private List<CrimeStatPreiod> ReadSummaryPeriods(string sheetName, int row)
        {
            var headings = new Dictionary<int, string>()
            {
                {0, "D13" },
                {1,"E13" },
                {2, "F13" },
                {3, "G13" },
                {4, "H13" }
            };
            var periods = new List<CrimeStatPreiod>();
            foreach(var heading in headings)
            {
                string periodStr = GetCellValue(fileName, sheetName, heading.Value);
                periodStr = periodStr.Substring(periodStr.LastIndexOf("$") + 2);
                var values = periodStr.Split("to");
                var period = new CrimeStatPreiod()
                {
                    Order = heading.Key,
                    Year = int.Parse(values[0].Split(" ")[1]),
                    MonthFrom = values[0].Split(" ")[0].Trim(),
                    MonthTo = values[1].Split(" ")[1].Trim(),
                };
                string strPeriodValue = GetCellValue(fileName, sheetName, $"{heading.Value.Replace("13", "")}{row}");
                period.Value = int.Parse(strPeriodValue.Substring(strPeriodValue.LastIndexOf(")")+1));
                periods.Add(period);
            }
            return periods;
        }


        // Retrieve the value of a cell, given a file name, sheet name, 
        // and address name.
        private string GetCellValue(string fileName,
            string sheetName,
            string addressName)
        {
            string value = null;            
            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document =
                SpreadsheetDocument.Open(fileName, false))
            {
                // Retrieve a reference to the workbook part.
                WorkbookPart wbPart = document.WorkbookPart;

                // Find the sheet with the supplied name, and then use that 
                // Sheet object to retrieve a reference to the first worksheet.
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                  Where(s => s.Name == sheetName).FirstOrDefault();

                // Throw an exception if there is no sheet.
                if (theSheet == null)
                {
                    throw new ArgumentException("sheetName");
                }

                // Retrieve a reference to the worksheet part.
                WorksheetPart wsPart =
                    (WorksheetPart)(wbPart.GetPartById(theSheet.Id));

                // Use its Worksheet property to get a reference to the cell 
                // whose address matches the address you supplied.
                Cell theCell = wsPart.Worksheet.Descendants<Cell>().
                  Where(c => c.CellReference == addressName).FirstOrDefault();

                // If the cell does not exist, return an empty string.
                if (theCell.InnerText.Length > 0)
                {
                    value = theCell.InnerText;

                    // If the cell represents an integer number, you are done. 
                    // For dates, this code returns the serialized value that 
                    // represents the date. The code handles strings and 
                    // Booleans individually. For shared strings, the code 
                    // looks up the corresponding value in the shared string 
                    // table. For Booleans, the code converts the value into 
                    // the words TRUE or FALSE.
                    if (theCell.DataType != null)
                    {
                        switch (theCell.DataType.Value)
                        {
                            case CellValues.SharedString:

                                // For shared strings, look up the value in the
                                // shared strings table.
                                var stringTable =
                                    wbPart.GetPartsOfType<SharedStringTablePart>()
                                    .FirstOrDefault();

                                // If the shared string table is missing, something 
                                // is wrong. Return the index that is in
                                // the cell. Otherwise, look up the correct text in 
                                // the table.
                                if (stringTable != null)
                                {
                                    value =
                                        stringTable.SharedStringTable
                                        .ElementAt(int.Parse(value)).InnerText;
                                }
                                break;

                            case CellValues.Boolean:
                                switch (value)
                                {
                                    case "0":
                                        value = "FALSE";
                                        break;
                                    default:
                                        value = "TRUE";
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            return value;
        }

        public Task<List<CrimeStat>> ReadCrimeStatsAsync(string category)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ReadCrimeStatCategoriesAsync()
        {
            List<string> categories = new List<string>();
            //return a list of the sheet names in the excel file:

            using (SpreadsheetDocument document =
                               SpreadsheetDocument.Open(fileName, false))
            {
                // Retrieve a reference to the workbook part.
                WorkbookPart wbPart = document.WorkbookPart;
                foreach(var sheet in wbPart.Workbook.Descendants<Sheet>())
                {
                    if(sheet.Name.ToString().Contains("TOP30"))
                    {
                        categories.Add(sheet.Name);
                    }
                }
            }
            return Task.FromResult(categories);            
        }
    }
}

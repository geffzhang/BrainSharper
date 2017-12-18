namespace BrainSharperTests.TestUtils
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using BrainSharper.Abstract.Data;
    using BrainSharper.General.Utils;
    using BrainSharper.Implementations.Data;

    using MathNet.Numerics.LinearAlgebra;

    public static class TestDataBuilder
    {
        public const string WeatherDataDependentFeatureName = "Play";

        public static IDataFrame BuildSmallDataFrameMixedDataTypes()
        {
            return new DataFrame(
                new DataTable("some")
                {
                    Columns =
                    {
                        new DataColumn("Col1", typeof(string)),
                        new DataColumn("Col2", typeof(int)),
                        new DataColumn("Col3", typeof(string)),
                        new DataColumn("Col4", typeof(int))
                    },
                    Rows =
                    {
                        new object [] { "a1.1", 1, "b1.2", 2 },
                        new object [] { "a2.1", 3, "b2.2", 4 },
                        new object [] { "a3.1", 5, "b3.2", 6 },
                    }
                }, 
                new [] { 100, 101, 102 });
        }

        public static IDataFrame BuildSmallDataFrameNumbersOnly()
        {
            return new DataFrame(
               new DataTable("some")
               {
                   Columns =
                   {
                        new DataColumn("Col1", typeof(object)),
                        new DataColumn("Col2", typeof(object)),
                        new DataColumn("Col3", typeof(object)),
                        new DataColumn("Col4", typeof(object))
                   },
                   Rows =
                   {
                        new object [] { 1, 2, 3, 4 },
                        new object [] { 5, 6, 7, 8 },
                        new object [] { 9, 10, 11, 12 },
                   }
               }, 
               new [] { 100, 101, 102 });
        }

        public static IDataVector<object> BuildMixedObjectsVector()
        {
            return new DataVector<object>(new object[] { "A", 1, "B", 2 }, new []{ "F1", "F2", "F3", "F4" });
        }

        public static IDataVector<object> BuildNumericVector()
        {
            return new DataVector<object>(new object[] { 1.0, 2.0, 3.0, 4.0 }, new[] { "F1", "F2", "F3", "F4" });
        }

        /// <summary>
        /// Creates data frame with values calculated using the following linear equation:
        /// f6 = f1 + 2f2 - 3f3
        /// </summary>
        /// <returns>Data frame with numeric outcomes</returns>
        public static IDataFrame BuildRandomAbstractNumericDataFrameWithRedundantAttrs(int rowCount = 10000, Random randomizer = null, int min = 1, int max = 10)
        {
            randomizer = randomizer ?? new Random();
            var dataTable = new DataTable("some table")
            {
                Columns = { "F1", "F2", "F3", "F4", "F5", "F6" }
            };
            for (int i = 0; i < rowCount; i++)
            {
                var features = new double[6];
                for (int featureIdx = 0; featureIdx < 5; featureIdx++)
                {
                    features[featureIdx] = randomizer.Next(min, max);
                }
                features[5] = CalcualteLinearlyDependentFeatureValue(features);
                dataTable.Rows.Add(features.Select(val => val as object).ToArray());
            }

            return new DataFrame(dataTable);
        }

        /// <summary>
        /// Creates data frame with values calculated using provided function
        /// </summary>
        /// <returns>Data frame with numeric outcomes</returns>
        public static IDataFrame BuildRandomAbstractNumericDataFrame(
            Func<IList<double>, double> resultFunCalc,
            int rowCount = 10000, 
            int featuresCount = 5, 
            Random randomizer = null, 
            int min = 1, 
            int max = 10)
        {
            randomizer = randomizer ?? new Random();
            var columnNames = Enumerable.Range(0, featuresCount).Select(val => "F" + (val + 1).ToString()).ToList();
            columnNames.Add("result");
            var rowsCollection = new List<double[]>();
            
            for (int i = 0; i < rowCount; i++)
            {
                var features = new double[featuresCount];
                for (int featureIdx = 0; featureIdx < featuresCount; featureIdx++)
                {
                    features[featureIdx] = randomizer.NextDoubleInRange(min, max);
                }
                var result = resultFunCalc(features);
                var finalFeatures = features.ToList();
                finalFeatures.Add(result);
                rowsCollection.Add(finalFeatures.ToArray());
            }
            var matrix = Matrix<double>.Build.DenseOfRowArrays(rowsCollection);
            return new DataFrame(matrix, columnNames);
        }

        /// <summary>
        /// Build fish example taken from the book "Machine learning in action" Harrington P., 1st edition page 40
        /// </summary>
        /// <returns>DataFrame</returns>
        public static IDataFrame BuildTrivialAniamlsExample()
        {
            var dataTable = new DataTable()
                                {
                                    Columns =
                                        {
                                            new DataColumn("can survive without coming to surface"),
                                            new DataColumn("Has flippers"),
                                            new DataColumn("Is Fish")
                                        },
                                    Rows =
                                        {
                                            { "y", "y", "y" },
                                            { "y", "y", "y" },
                                            { "y", "n", "n" },
                                            { "n", "y", "n" },
                                            { "n", "y", "n" }
                                        }
                                };
            return new DataFrame(dataTable);
        }

        public static double CalcualteLinearlyDependentFeatureValue(IList<double> features)
        {
            return features[0] + (2 * features[1]) - (3 * features[2]);
        }

        public static IDataFrame ReadIrisData()
        {
            DataTable dt = ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\IrisData.txt", true);
            return new DataFrame(dt);
        }

        public static IDataFrame ReadAdultCensusDataFrame()
        {
            DataTable dt = ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\AdultCensusIncomeBinaryClassification.txt", true);
            return new DataFrame(dt);
        }

        public static IDataFrame ReadCongressData()
        {
            DataTable dt = ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\CongressVoting.txt", true);
            return new DataFrame(dt);
        }

        public static IDataFrame ReadWeatherDataWithCategoricalAttributes()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\WeatherData.txt", true));
        }

        public static IDataFrame ReadWeatherDataWithMixedAttributes()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\WeatherDataNumeric.txt", true));
        }

        public static IDataFrame ReadMushroomDataWithCategoricalAttributes()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\Mushroom.txt", true));
        }

        public static IDataFrame ReadHousingData()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\Housing.txt", true));
        }

        public static IDataFrame ReadHousingDataNormalizedAttrs()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\HousingNormalized.txt", true));
        }

        public static IDataFrame ReadTitanicData()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\titanic_train_data.txt", true));
        }

        public static IDataFrame ReadTitanicQuery()
        {
            return new DataFrame(ReadCsvIntoDataTable(@"BrainSharperTests\DataSets\cleaned_test.csv", true));
        }

        private static DataTable ReadCsvIntoDataTable(string filepath, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string fileName = Path.GetFileName(filepath);
            string directory = Path.GetDirectoryName(filepath) ?? string.Empty;
            var currentDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(currentDirectory, directory);

            string sql = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider= Microsoft.Jet.OLEDB.4.0;Data Source=" + fullPath +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable { Locale = CultureInfo.CurrentCulture };
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}

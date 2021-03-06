﻿using ChoETL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace ChoCSVReaderTest
{
    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    //[ChoCSVFileHeader()]
    [ChoCSVRecordObject(Encoding = "Encoding.UTF32", ErrorMode = ChoErrorMode.IgnoreAndContinue, IgnoreFieldValueMode = ChoIgnoreFieldValueMode.All)]
    public class EmployeeRec : IChoRecord
    {
        [ChoCSVRecordField(1, FieldName = "id")]
        [ChoTypeConverter(typeof(IntConverter))]
        //[Range(1, int.MaxValue, ErrorMessage = "Id must be > 0.")]
        //[ChoFallbackValue(1)]
        public int Id { get; set; }
        [ChoCSVRecordField(2, FieldName ="Name", QuoteField = true)]
        [Required]
        [DefaultValue("ZZZ")]
        //[ChoFallbackValue("XXX")]
        public string Name { get; set; }

        //[ChoCSVRecordField(3, FieldName = "Address")]
        //public string Address { get; set; }

        public bool AfterRecordFieldLoad(int index, string propName, object value)
        {
            throw new NotImplementedException();
        }

        public bool AfterRecordLoad(int index, object source)
        {
            throw new NotImplementedException();
        }

        public bool BeforeRecordFieldLoad(int index, string propName, ref object value)
        {
            throw new NotImplementedException();
        }

        public bool BeforeRecordLoad(int index, ref object source)
        {
            throw new NotImplementedException();
        }

        public bool BeginLoad(object source)
        {
            throw new NotImplementedException();
        }

        public bool EndLoad(object source)
        {
            throw new NotImplementedException();
        }

        public bool RecordFieldLoadError(int index, string propName, object value, Exception ex)
        {
            throw new NotImplementedException();
        }

        public bool RecordLoadError(int index, object source, Exception ex)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ChoCSVRecordConfiguration config = new ChoCSVRecordConfiguration();
            config.RecordFieldConfigurations.Add(new ChoCSVRecordFieldConfiguration("Id", 1));
            config.RecordFieldConfigurations.Add(new ChoCSVRecordFieldConfiguration("Name", 2));

            dynamic row;
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            using (var parser = new ChoCSVReader(reader, config))
            {
                writer.WriteLine("1,Carl");
                writer.WriteLine("2,Mark");
                writer.Flush();
                stream.Position = 0;

                while ((row = parser.Read()) != null)
                {
                    Console.WriteLine(row.Name);
                }
            }
            return;

            //DataTable dt = new ChoCSVReader<EmployeeRec>("Emp.csv").AsDataTable();
            //var z = dt.Rows.Count;
            //return;

            //foreach (var e in new ChoCSVReader<EmployeeRec>("Emp.csv"))
            //    Console.WriteLine(e.ToStringEx());

            //var reader = new ChoCSVReader<EmployeeRec>("Emp.csv");
            //var rec = (object)null;

            //while ((rec = reader.Read()) != null)
            //    Console.WriteLine(rec.ToStringEx());

            //var config = new ChoCSVRecordConfiguration(typeof(EmployeeRec));
            //var e = new ChoCSVReader("Emp.csv", config);
            //dynamic i;
            //while ((i = e.Read()) != null)
            //    Console.WriteLine(i.Id);

            ChoETLFramework.Initialize();
            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            using (var parser = new ChoCSVReader<EmployeeRec>(reader))
            {
                //writer.WriteLine("Id,Name");
                writer.WriteLine("1,Carl");
                writer.WriteLine("2,Mark");
                writer.Flush();
                stream.Position = 0;
                var dr = parser.AsDataReader();
                while (dr.Read())
                {
                    Console.WriteLine(dr[0]);
                }
                //object row = null;
                 
                //parser.Configuration.ColumnCountStrict = true;
                //while ((row = parser.Read()) != null)
                //{
                //    Console.WriteLine(row.ToStringEx());
                //}
            }
        }
    }
}

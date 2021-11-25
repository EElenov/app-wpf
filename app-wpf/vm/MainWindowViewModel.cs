using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using app_domain;
using Microsoft.Win32;
using static app_domain.BogusFill;

namespace app_wpf
{
    public enum ExportFileFormat
    {
        csv,
        xls,
        xlsx,
        sql,
        xml,
        json,
        bin,
    }
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Interface-Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) //setter helper
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        public MainWindowViewModel()
        {
            Designs = FillDataForDesigns(RND.Next(5, 15)).Select(x => MapDesignObject(x));
            CheckBoxCmd = new RelayCommand(o => true, o => DesignSelectionSupport());
            ContextDL_Excel_xls = new CommandWrapper()
            {
                Label = "As Excel File(.xls)",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.xls))
            };
            ContextDL_Excel_xlsx = new CommandWrapper()
            {
                Label = "As Excel File(.xlsx)",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.xlsx))
            };
            ContextDL_Excel_csv = new CommandWrapper()
            {
                Label = "As Excel Table(.csv)",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.csv))
            };
            ContextDL_DBq_sql = new CommandWrapper()
            {
                Label = "Database Query (.sql)",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.sql))
            };
            ContextDL_Ser_XML = new CommandWrapper()
            {
                Label = "XML Serialization",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.xml))
            };
            ContextDL_Ser_JSON = new CommandWrapper()
            {
                Label = "JSON Serialization",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.json))
            };
            ContextDL_Ser_BIN = new CommandWrapper()
            {
                Label = "Binary Serialization",
                Command = new RelayCommand(o => true, o => ExportObject(ExportFileFormat.bin))
            };
            ContextOpen_InApp = new CommandWrapper()
            {
                Label = "Open in *******",
            };
            ContextOpen_Manual = new CommandWrapper()
            {
                Label = "Open Manual",
            };

        }

        private CommandWrapper _SaveChanges;
        public CommandWrapper SaveChanges
        {
            get => _SaveChanges;
            set => SetField(ref _SaveChanges, value);
        }

        private CommandWrapper _DiscardChanges;
        public CommandWrapper DiscardChanges
        {
            get => _DiscardChanges;
            set => SetField(ref _DiscardChanges, value);
        }

        private DummyDesignObject _SelectedDesign;
        public DummyDesignObject SelectedDesign
        {
            get => _SelectedDesign;
            set => SetField(ref _SelectedDesign, value);
        }

        private DummyProgramObject _SelectedProgram;
        public DummyProgramObject SelectedProgram
        {
            get => _SelectedProgram;
            set => SetField(ref _SelectedProgram, value);
        }

        private IEnumerable<DummyDesignObject> _Designs;
        public IEnumerable<DummyDesignObject> Designs
        {
            get => _Designs;
            set => SetField(ref _Designs, value);
        }

        private IEnumerable<DummyProgramObject> _Programs;
        public IEnumerable<DummyProgramObject> Programs
        {
            get => _Programs;
            set => SetField(ref _Programs, value);
        }

        #region ContextMenu-Members
        private CommandWrapper _ContextDL_Excel_xls;
        public CommandWrapper ContextDL_Excel_xls
        {
            get => _ContextDL_Excel_xls;
            set => SetField(ref _ContextDL_Excel_xls, value);
        }

        private CommandWrapper _ContextDL_Excel_xlsx;
        public CommandWrapper ContextDL_Excel_xlsx
        {
            get => _ContextDL_Excel_xlsx;
            set => SetField(ref _ContextDL_Excel_xlsx, value);
        }
        private CommandWrapper _ContextDL_Excel_csv;
        public CommandWrapper ContextDL_Excel_csv
        {
            get => _ContextDL_Excel_csv;
            set => SetField(ref _ContextDL_Excel_csv, value);
        }
        private CommandWrapper _ContextDL_DBq_sql;
        public CommandWrapper ContextDL_DBq_sql
        {
            get => _ContextDL_DBq_sql;
            set => SetField(ref _ContextDL_DBq_sql, value);
        }
        private CommandWrapper _ContextDL_Ser_XML;
        public CommandWrapper ContextDL_Ser_XML
        {
            get => _ContextDL_Ser_XML;
            set => SetField(ref _ContextDL_Ser_XML, value);
        }
        private CommandWrapper _ContextDL_Ser_JSON;
        public CommandWrapper ContextDL_Ser_JSON
        {
            get => _ContextDL_Ser_JSON;
            set => SetField(ref _ContextDL_Ser_JSON, value);
        }
        private CommandWrapper _ContextDL_Ser_BIN;
        public CommandWrapper ContextDL_Ser_BIN
        {
            get => _ContextDL_Ser_BIN;
            set => SetField(ref _ContextDL_Ser_BIN, value);
        }
        private CommandWrapper _ContextOpen_InApp;
        public CommandWrapper ContextOpen_InApp
        {
            get => _ContextOpen_InApp;
            set => SetField(ref _ContextOpen_InApp, value);
        }
        private CommandWrapper _ContextOpen_Manual;
        public CommandWrapper ContextOpen_Manual
        {
            get => _ContextOpen_Manual;
            set => SetField(ref _ContextOpen_Manual, value);
        }
        #endregion

        private ICommand _CheckBoxCmd;
        public ICommand CheckBoxCmd
        {
            get => _CheckBoxCmd;
            set => SetField(ref _CheckBoxCmd, value);
        }

        private DummyDesignObject MapDesignObject(DummyDesign input)
        {
            return new DummyDesignObject()
            {
                Id = input.Id,
                Name = input.Name,
                Description = input.Description,
                Tooltip = input.Tooltip,
                CreationDate = input.CreationDate,
            };
        }

        private DummyProgramObject MapProgramObject(DummyProgram input)
        {
            return new DummyProgramObject()
            {
                Id = input.Id,
                Name = input.Name,
                Description = input.Description,
                Tooltip = input.Tooltip,
                CreationDate = input.CreationDate,
                Parameters = FillDataForParameters(RND.Next(14, 74)).Select(x => MapParameterObject(x))
            };
        }
        private DummyParameterObject MapParameterObject(DummyParameter input)
        {
            return new DummyParameterObject()
            {
                Name = input.Name,
                Tooltip = input.Tooltip,
                Data = input.Data,
                ParamInterpreter = input.ParamInterpreter,
            };
        }

        private void DesignSelectionSupport()
        {
            // multiselection with adding each design programs as a listviewgroup, but for demo its single select only
            if (SelectedDesign is null)
            {
                return;
            }
            Programs = FillDataForPrograms(RND.Next(5, 15), SelectedDesign.Id).Select(x => MapProgramObject(x));
        }

        public void ExportObject(ExportFileFormat format)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"B:\testFolder";
            saveFileDialog.Title = "Save File(s)";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FilterIndex = 1;
            switch (format)
            {
                case ExportFileFormat.csv:
                    saveFileDialog.DefaultExt = "csv";
                    saveFileDialog.Filter = "CSV Table (*.csv)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveCSVFile($"{saveFileDialog.FileName}");
                    }
                    break;
                case ExportFileFormat.xls:
                    saveFileDialog.DefaultExt = "xls";
                    saveFileDialog.Filter = "Excel Table(2003) (*.xls)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveXLSFile($"{saveFileDialog.FileName}");
                    }
                    break;
                case ExportFileFormat.xlsx:
                    saveFileDialog.DefaultExt = "xlsx";
                    saveFileDialog.Filter = "Excel Table (*.xls)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveXLSXFile($"{saveFileDialog.FileName}");
                    }
                    break;
                case ExportFileFormat.xml:
                    saveFileDialog.DefaultExt = "xml";
                    saveFileDialog.Filter = "XML File (*.xml)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveXMLFile($"{saveFileDialog.FileName}");
                    }
                    break;
                case ExportFileFormat.json:
                    saveFileDialog.DefaultExt = "json";
                    saveFileDialog.Filter = "JSON File (*.json)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveJSONFile($"{saveFileDialog.FileName}");
                    }
                    break;
                case ExportFileFormat.bin:
                    saveFileDialog.Filter = "Binary Serialization (*.bin)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveBinaryFile($"{saveFileDialog.FileName}");
                    }
                    break;
                case ExportFileFormat.sql:
                    saveFileDialog.DefaultExt = "sql";
                    saveFileDialog.Filter = "SQL (*.sql)|All files (*.*)";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SaveSQLFile($"{saveFileDialog.FileName}");
                    }
                    break;
                default: break;
            }

        }


        /*
        Dont have office package installed so the libraries are missing and cannot be referenced
        */
        #region Excel
        private void SaveXLSXFile(string path)
        {

        }

        private void SaveXLSFile(string path)
        {
            /*Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Add(misValue);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            foreach()
            {
                xlWorkSheet.Cells[1, 1] = // maping info to cells
            }

            xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);*/
        }
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

        private void SaveCSVFile(string path)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                foreach (var prop in SelectedDesign.GetType().GetProperties())
                {
                    str.AppendLine($"{prop.Name},{prop.GetValue(SelectedDesign, null)};");
                }
                File.WriteAllText(path, str.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SaveSQLFile(string path)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append(
                    $"SELECT * FROM DummyDesignTable WHERE Id=\'" +
                    $"\'{SelectedDesign.Id};");
                File.WriteAllText(path, str.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SaveXMLFile(string path)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                using (var stream = new MemoryStream())
                {
                    using (var writer = XmlWriter.Create(stream))
                    {
                        new XmlSerializer(SelectedDesign.GetType()).Serialize(writer, SelectedDesign);
                        str.Append(Encoding.UTF8.GetString(stream.ToArray()));
                    }
                }
                File.WriteAllText(path, str.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SaveJSONFile(string path)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append(JsonSerializer.Serialize(SelectedDesign));
                File.WriteAllText(path, str.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SaveBinaryFile(string path)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                using (var stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, SelectedDesign);
                    str.Append(Convert.ToBase64String(stream.ToArray()));

                    stream.Close();
                }
                File.WriteAllText(path, str.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

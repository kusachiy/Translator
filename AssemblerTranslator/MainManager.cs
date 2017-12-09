using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AssemblerTranslator.Analyzers;
using System.Diagnostics;

namespace AssemblerTranslator
{
    public class MainManager:ViewModelBase
    {
        private string _fileName = @"C:\Users\hrusha\Desktop\Projects\_study\AssemblerFiles\tests\1.txt";
        private string _codeText;
        private string _answer;
        private string _log;


        public RelayCommand FileOpenCommand { get; private set; }
        public RelayCommand FileSaveCommand { get; private set; }
        public RelayCommand CompileCommand { get; private set; }
        public RelayCommand OpenWorkFolderCommand { get; private set; }
        public RelayCommand SaveAssemblerCodeCommand { get; private set; }


        public string CodeText { get { return _codeText; } set { _codeText = value;RaisePropertyChanged("CodeText"); } }
        public string Answer { get { return _answer; } set { _answer= value; RaisePropertyChanged("Answer"); } }
        public string Log { get { return _log; } set { _log = value; RaisePropertyChanged("Log"); } }

        public MainManager()
        {
            FileOpenCommand = new RelayCommand(OpenFile);
            FileSaveCommand = new RelayCommand(SaveFile);
            CompileCommand = new RelayCommand(Compile);
            SaveAssemblerCodeCommand = new RelayCommand(SaveAssemblerCode);
            OpenWorkFolderCommand = new RelayCommand(OpenFolder);
        }

        private void OpenFolder()
        {
            var path = Path.Combine(Path.GetDirectoryName(_fileName), "asm_files\\Batnik.bat");
            if (File.Exists(path))
            {
                Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + path));
            }
        }

        private void OpenFile()
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            //myDialog.Filter = "(*.txt)";
            myDialog.CheckFileExists = true;
            if (myDialog.ShowDialog() == true)
            {
                _fileName = myDialog.FileName;
                StreamReader sr = new StreamReader(_fileName);
                CodeText = sr.ReadToEnd();
                sr.Close();
            }
        }
        private void SaveFile()
        {
            StreamWriter sw = new StreamWriter(_fileName);
            sw.WriteLine(_codeText);
            sw.Close();
        }
        private void Compile()
        {
            Translator translator = new Translator(_codeText);
            try
            {
                translator.Compile();
            }
            catch (Exception e)
            {
                Log = e.Message;
                return;
            }
            Log = $"Компиляция успешно завершена в {DateTime.Now}";
            Answer = translator.GetAssemblerCode;
        }

        private void SaveAssemblerCode()
        {
            var path = Path.Combine(Path.GetDirectoryName(_fileName), "asm_files\\testCode.asm");
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(Answer);
            sw.Close();
            Log = $"Файл testCode.asm создан в {DateTime.Now}";
        }
    }
}

﻿using GalaSoft.MvvmLight;
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

namespace AssemblerTranslator
{
    public class MainManager:ViewModelBase
    {
        private string _fileName;
        private string _codeText;
        private string _answer;


        public RelayCommand FileOpenCommand { get; private set; }
        public RelayCommand FileSaveCommand { get; private set; }
        public RelayCommand CompileCommand { get; private set; }
        public string CodeText { get { return _codeText; } set { _codeText = value;RaisePropertyChanged("CodeText"); } }
        public string Answer { get { return _answer; } set { _answer= value; RaisePropertyChanged("Answer"); } }



        public MainManager()
        {
            FileOpenCommand = new RelayCommand(OpenFile);
            FileSaveCommand = new RelayCommand(SaveFile);
            CompileCommand = new RelayCommand(Compile);
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
            translator.Compile();
            Answer = translator.GetAssemblerCode;
        }
    }
}

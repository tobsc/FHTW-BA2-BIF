using System;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class LogViewModel
    {
        public string Date { get; set; }
        public string LogType { get; set; }
        public string Origin { get; set; }
        public string Message { get; set; }

        public LogViewModel()
        {
            
        }

        public LogViewModel(string line)
        {
            Refresh(line);
        }

        public void Refresh(string obj)
        {

            if (obj == null) return;

            var target = this;
            var source = obj;

            var split = source.Split(' ');
            var dateTime = split[0] + "T" + split[1].Replace(',', '.');

            target.Date = dateTime;
            target.LogType = split[2];
            target.Origin = split[3];

            for (var i = 5; i < split.Length; i++)
            {
                target.Message += split[i] + " ";
            }
            
        }
    }
}
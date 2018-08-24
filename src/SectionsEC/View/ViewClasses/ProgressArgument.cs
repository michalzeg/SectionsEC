﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.WindowClasses
{
    public class ProgressArgument
    {
        public static ProgressArgument CalculateProgress(int currentIndex, int maxIndex, string name)
        {
            var progress = new ProgressArgument();
            progress.Progress = (currentIndex) * 100 / maxIndex;
            progress.LoadCaseName = name;
            return progress;
        }

        public int Progress;
        public string LoadCaseName;
    }
}
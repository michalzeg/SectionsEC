﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionsEC.Calculations.Progress
{
    public class ProgressArgument
    {
        public static ProgressArgument CalculateProgress(int currentIndex, int maxIndex, string name)
        {
            var progress = new ProgressArgument
            {
                Progress = (currentIndex) * 100 / maxIndex,
                LoadCaseName = name
            };
            return progress;
        }

        public int Progress { get; private set; }
        public string LoadCaseName { get; private set; }
    }
}
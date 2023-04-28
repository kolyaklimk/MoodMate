﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMate.Components.Data.Abstractions;

internal interface IDataAnalysis<T>
{
    public Dictionary<string, (string, int, int)> AnalysedData { get; set; }
    void AddItem(string name, string source, DateTime date, DateTime Choosedate);
    int GetCount();
    void GetPercents();
}

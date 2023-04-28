using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMate.Components.Data.Abstractions;

internal interface IDataLoading<T>
{
    List<T> Data { get; set; }
    Task Load(string path, bool local);
}

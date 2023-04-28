using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMate.Components.Data.Abstractions;

internal interface IDataControl<T>
{
    void Delete(int index);
    void Add(T item);
    void Change(int index, T item);
    Task SortByDate(string sortColumn);
    Task UpdateFile(string path);
}

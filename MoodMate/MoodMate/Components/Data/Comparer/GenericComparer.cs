using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoodMate.Components.Data.Comparer;

sealed internal class GenericComparer<T> : IComparer<T>
{
    public int Compare(T x, T y)
    {
    }
}

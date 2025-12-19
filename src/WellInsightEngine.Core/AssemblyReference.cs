using System.Reflection;

namespace WellInsightEngine.Core;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
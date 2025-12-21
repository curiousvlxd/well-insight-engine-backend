using System.ComponentModel;

namespace WellInsightEngine.Core.Enums;

public enum ParameterDataType : short
{   
    [Description("numeric")]
    Numeric = 1,
    [Description("categorical")]
    Categorical = 2
}
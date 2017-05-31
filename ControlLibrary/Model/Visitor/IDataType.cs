using System.Collections.Generic;

namespace ControlLibrary.Model.Visitor
{
    public interface IDataType
    {
        string GetDisplayValue(object obj, bool CanConvert = true);
        string GetEqualSyntaxForLambdaExpression();
    }
}

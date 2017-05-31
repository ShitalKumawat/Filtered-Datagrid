using System;
namespace ControlLibrary.Model.Visitor
{
    public class DefaultType : IDataType
    {
        public string GetDisplayValue(object obj,bool CanConvert = true)
        {
            var value = obj != null ? Convert.ToString(obj) : "[Blank]";
            return !string.IsNullOrWhiteSpace(value) ? value.Trim() : "[Blank]";
        }

        public string GetEqualSyntaxForLambdaExpression()
        {
            return "=";
        }
    }
}

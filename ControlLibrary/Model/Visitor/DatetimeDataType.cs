using System;

namespace ControlLibrary.Model.Visitor
{
    class DatetimeDataType : IDataType
    {
        public string GetDisplayValue(object obj,bool CanConvert = true)
        {
            var value = obj != null ? CanConvert? Convert.ToDateTime(obj).ToShortDateString() :obj.ToString() : "[Blank]";
            return !string.IsNullOrWhiteSpace(value) ? value.Trim() : "[Blank]";
        }

        public string GetEqualSyntaxForLambdaExpression()
        {
            return "=";
        }
    }
}

namespace ControlLibrary.Model.Visitor
{
    public class StringDataType : IDataType
    {
        public string GetDisplayValue(object obj, bool CanConvert = true)
        {
            var value = obj as string;
            return !string.IsNullOrWhiteSpace(value) ? value.Trim() : "[Blank]";
        }

        public string GetEqualSyntaxForLambdaExpression()
        {
            return ".Equals";
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace DynamicQueryLibrary
{
    public static class DynamicExpression
    {
        public static List<T> SelectExpression<T>(this IEnumerable data, string predicate, List<object> param)
        {
            string whereCondition = string.Empty;
            var selectCondition = string.Empty;
            var lastIndex = predicate.LastIndexOf("AND", StringComparison.CurrentCultureIgnoreCase);
            if (lastIndex != -1)
            {
                whereCondition = predicate.Substring(0, lastIndex);
                selectCondition = predicate.Substring(lastIndex).Replace("AND", "").Trim();

                var result = data.AsQueryable().Where(whereCondition, param).Select(selectCondition, param).Cast<T>().ToList();

                whereCondition = whereCondition.Replace("!=", "==").Replace("AND", "OR");
                if(data.AsQueryable().Where(whereCondition).Count()!=0) result.Add(default(T));

                return  result;

            }

            return string.IsNullOrWhiteSpace(predicate)
                   ? null
                   : data.AsQueryable().Select(predicate, param).Cast<T>().ToList();
        }

        public static List<T> WhereExpression<T>(this IEnumerable data, string predicate, List<object> param)
        {

           return string.IsNullOrWhiteSpace(predicate)
                ? null
                : data.AsQueryable().Where(predicate, param.ToArray<object>()).Cast<T>().ToList();
        }

        public static string GetSelectLamdaExpressionPredicate(this string data,bool isNullableDataType)
        {
            var nullCheckLamdaExpressionPredicate = GetNotNullCheckLamdaExpressionPredicate(data, isNullableDataType);
            

            return !string.IsNullOrWhiteSpace(nullCheckLamdaExpressionPredicate)
                ?
                isNullableDataType
                    ? "( " + nullCheckLamdaExpressionPredicate + " ) ? " + data + " : null"
                    : "( " + nullCheckLamdaExpressionPredicate + " ) AND " + data 
                : data;
        }

        public static string GetNullCheckLamdaExpressionPredicate(this string data,bool isNullableDataType)
        {
            var expression = string.Empty;
            const string nullCheckCondition = " ==null OR ";
            const string delimiter = ".";

            var bindingMembers = data.Split(delimiter[0]).ToList();

            if (!isNullableDataType && bindingMembers.Count == 1)
                return expression;

            if (bindingMembers.Count <= 1)
            {
                expression= data + nullCheckCondition;
            }
            else
            {
                var previousMember = string.Empty;
                foreach (var member in bindingMembers)
                {
                    expression += string.IsNullOrWhiteSpace(previousMember)
                        ? " " + member + nullCheckCondition
                        : previousMember + delimiter + member + nullCheckCondition;
                    previousMember = previousMember + (string.IsNullOrWhiteSpace(previousMember) ? "" : delimiter) +
                                     member;
                }
            }
            var lastIndex = expression.LastIndexOf("OR", StringComparison.CurrentCultureIgnoreCase);
            if (lastIndex != -1)
            {
                expression = expression.Substring(0, lastIndex);
            }

            if (!isNullableDataType)
            {
                lastIndex = expression.LastIndexOf("OR", StringComparison.CurrentCultureIgnoreCase);
                if (lastIndex != -1)
                {
                    expression = expression.Substring(0, lastIndex);
                }
            }

            expression = "( " + expression + " ) ";
            return expression;
        }
        public static string GetNotNullCheckLamdaExpressionPredicate(this string data, bool isNullableDataType)
        {
            var expression = string.Empty;
            const string nullCheckCondition = " !=null AND ";
            const string delimiter = ".";

            var bindingMembers = data.Split(delimiter[0]).ToList();

            if (!isNullableDataType && bindingMembers.Count == 1)
                return expression;

            if (bindingMembers.Count <= 1)
            {
                expression = data + nullCheckCondition;
            }
            else
            {
                var previousMember = string.Empty;
                foreach (var member in bindingMembers)
                {
                    expression += string.IsNullOrWhiteSpace(previousMember)
                        ? " " + member + nullCheckCondition
                        : previousMember + delimiter + member + nullCheckCondition;
                    previousMember = previousMember + (string.IsNullOrWhiteSpace(previousMember) ? "" : delimiter) +
                                     member;
                }
            }
            var lastIndex = expression.LastIndexOf("AND", StringComparison.CurrentCultureIgnoreCase);
            if (lastIndex != -1)
            {
                expression = expression.Substring(0, lastIndex);
            }

            if (!isNullableDataType)
            {
                lastIndex = expression.LastIndexOf("AND", StringComparison.CurrentCultureIgnoreCase);
                if (lastIndex != -1)
                {
                    expression = expression.Substring(0, lastIndex);
                }
            }
            expression = "( " + expression + " ) ";
            return expression;
        }
    }
}

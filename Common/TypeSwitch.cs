using System;

namespace Common
{
    public static class TypeSwitch
    {
        public class CaseInfo
        {

            public bool IsDefault { get; set; }

            public Type Target { get; set; }

            public Action Action { get; set; }

        }

        public static void Do(Type type, params CaseInfo[] cases)
        {
           
            foreach (var entry in cases)
            {

                if (entry.IsDefault || type == entry.Target)
                {

                    entry.Action();

                    break;

                }

            }

        }


        public static CaseInfo Case<T>(Action action)
        {

            return new CaseInfo()
            {

                Action = action,

                Target = typeof(T)

            };

        }


        //public static CaseInfo Case<T>(Action<T> action)
        //{

        //    return new CaseInfo()
        //    {

        //        Action = (x) => action((T)x),

        //        Target = typeof(T)

        //    };

        //}


        public static CaseInfo Default(Action action)
        {

            return new CaseInfo()
            {

                Action = action,

                IsDefault = true

            };

        }

    }
}

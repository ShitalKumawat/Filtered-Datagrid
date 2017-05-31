using System;
using System.Collections.Generic;
using System.Reflection;

namespace ControlLibrary.Model.Visitor
{
    public class DataTypeFactory
    {
        private static DataTypeFactory _instance;

        private Dictionary<string, Type> _datatTypes;

        private Dictionary<string, IDataType> _instances;

        public static DataTypeFactory GetInstance()
        {
            if (_instance == null)
                _instance = new DataTypeFactory();
            return _instance;
        }
        private DataTypeFactory()
        {
            _instances = new Dictionary<string, IDataType>();
            LoadTypes();
        }

        public IDataType CreateInstance(string description)
        {
            if (!_instances.ContainsKey(description))
            {
                Type t = GetTypeToCreate(description);

                var instance = (t != null) ? Activator.CreateInstance(t) as IDataType
                        : new DefaultType();
                _instances.Add(description, instance);
            }

            return _instances[description];
        }

        private Type GetTypeToCreate(string type)
        {
            foreach (var datatType in _datatTypes)
            {
                if (datatType.Key.Contains(type))
                {
                    return _datatTypes[datatType.Key];
                }
            }

            return null;
        }

        private void LoadTypes()
        {
            _datatTypes = new Dictionary<string, Type>();

            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in typesInThisAssembly)
            {
                if (type.GetInterface(typeof(IDataType).ToString()) != null)
                {
                    _datatTypes.Add(type.Name, type);
                }
            }
        }
    }
}

using SSRDB.Entities;
using Npgsql;
using System.Reflection;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace SSRDB.Utils
{
    public static class RepositoryUtils
    {
        public static List<NpgsqlParameter> ParametersGenerator<T>(T obj)
        {
            var excludedTypes = new HashSet<Type>
            {
                typeof(Patient), typeof(Employee), typeof(Service), typeof(Appointment),
                typeof(AppointmentService), typeof(Prescription), typeof(Medication), typeof(Diagnosis)
            };
            var type = typeof(T);
            List<NpgsqlParameter> parameters = [];
            foreach (var Prop in type.GetProperties())
            {
                if (!(Prop.PropertyType.IsGenericType) && !(excludedTypes.Contains(Prop.PropertyType)))
                {
                    parameters.Add(new NpgsqlParameter($"{Prop.Name}", Prop.GetValue(obj)));
                }
                    
            }
            return parameters;
        }

        public static List<PropertyInfo> FieldGen<T>(T obj)
        {
            var excludedTypes = new HashSet<Type>
    {
        typeof(Patient), typeof(Employee), typeof(Service), typeof(Appointment),
        typeof(AppointmentService), typeof(Prescription), typeof(Medication), typeof(Diagnosis)
    };

            return obj!
              .GetType()
              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(p =>
                 !p.PropertyType.IsGenericType
                 && !excludedTypes.Contains(p.PropertyType)
              )
              .ToList();
        }

        public static bool IsAnyNullOrEmpty(object myObject)
        {
            return myObject.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => (string)pi.GetValue(myObject))
            .Any(value => string.IsNullOrEmpty(value));
        }

        public static void CopyObject<T>(T source, T target)
        {
            var type = typeof(T);
            foreach (var sourceProperty in type.GetProperties())
            {
                var targetProperty = type.GetProperty(sourceProperty.Name);
                targetProperty?.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
            foreach (var sourceField in type.GetFields())
            {
                var targetField = type.GetField(sourceField.Name);
                targetField?.SetValue(target, sourceField.GetValue(source));
            }
        }

    }
}

using System;
using System.Linq;
using System.Reflection;

namespace Utils.Extensions
{
    /// <summary>
    /// A static class for reflection type functions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Extension for 'Object' that copies the fields to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyFieldsTo(this object source, object destination)
        {
            // if any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("source or/and destination objects are null");
            
            // getting the Types of the objects
            var sourceType = source.GetType();
            var destinationType = destination.GetType();
            
            //Logger.Log($"source: {sourceType}; destination: {destinationType}");
            
            // collect all the valid properties to map
            var results = from sourceField in sourceType.GetFields()
                let targetField = destinationType.GetField(sourceField.Name)
                where //sourceField.CanRead
                      sourceField.GetValue(source) != null
                      && targetField != null
                      /*&& (targetField.GetSetMethod(true) != null && !targetField.GetSetMethod(true).IsPrivate)
                      && (targetField.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                      && targetField.PropertyType.IsAssignableFrom(targetField.PropertyType)*/
                select new { sourceField, targetField };
            
            // map the properties
            foreach (var fields in results)
            {
                fields.targetField.SetValue(destination, fields.sourceField.GetValue(source));
            }
        }
        
        public static void CopyPropertiesTo(this object source, object destination)
        {
            // if any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("source or/and destination objects are null");
            
            // getting the Types of the objects
            var sourceType = source.GetType();
            var destinationType = destination.GetType();
            
            //Logger.Log($"source: {sourceType}; destination: {destinationType}");
            
            // collect all the valid properties to map
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
            var results = from sourceProperty in sourceType.GetProperties(flags)
                let targetProperty = destinationType.GetProperty(sourceProperty.Name)
                where //sourceField.CanRead
                    sourceProperty.GetValue(source) != null
                    && targetProperty != null
                    && targetProperty.GetSetMethod(true) != null
                /*&& (targetField.GetSetMethod(true) != null && !targetField.GetSetMethod(true).IsPrivate)
                && (targetField.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                && targetField.PropertyType.IsAssignableFrom(targetField.PropertyType)*/
                select new { sourceProperty, targetProperty };
            
            // map the properties
            foreach (var properties in results)
            {
                var sourceProperty = properties.sourceProperty;
                var targetProperty = properties.targetProperty;
                //Logger.Log($"property: {sourceProperty.Name}; value: {sourceProperty.GetValue(source)}");
                targetProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        public static object CallGenericVersion(this object source, string name, Type type, object[] parameters = null)
        {
            var thisType = source.GetType();
            var methodsInfo = thisType
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var methodInfo = methodsInfo.FirstOrDefault(m => 
                m.Name == name 
                && m.IsGenericMethod 
                //&& m.GetParameters().Length == parameters.Length
                );
            if (methodInfo == null)
            {
                throw new ArgumentException($"{thisType}.{name}<{type}> not found");
            }

            var genericMethod = methodInfo.MakeGenericMethod(type);
            return genericMethod.Invoke(source, parameters);
        }
    }
}
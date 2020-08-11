using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace KDScorpionEngineTests
{
    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ExtensionMethods
    {
        [ExcludeFromCodeCoverage]
        public static FieldInfo GetField(this object value, string name)
        {
            var privateFields = (from f in value.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
                                 where f.Name == name
                                 select f).ToArray();

            // If the list is not found throw not found exception
            if (privateFields == null || privateFields.Length <= 0)
                throw new Exception($"Cannot find the field {name} on the given object of type {value.GetType()}");

            return privateFields.FirstOrDefault();
        }

        /// <summary>
        /// Sets the value or reference type with the given <paramref name="name"/> to the value of 0 or null.
        /// </summary>
        /// <param name="fieldContainer">The object that contains the field to set to null.</param>
        /// <param name="name">The name of the field in the <paramref name="fieldContainer"/>.</param>
        public static void SetFieldNull(this object fieldContainer, string name)
        {
            var foundField = fieldContainer.GetField(name);

            if (fieldContainer.IsFieldPrimitive(name))
            {
                switch (foundField.FieldType)
                {
                    case Type intType when intType == typeof(int):
                    case Type uintType when uintType == typeof(uint):
                    case Type longType when longType == typeof(long):
                    case Type ulongType when ulongType == typeof(ulong):
                    case Type shortType when shortType == typeof(short):
                    case Type ushortType when ushortType == typeof(ushort):
                    case Type byteType when byteType == typeof(byte):
                    case Type sbyteType when sbyteType == typeof(sbyte):
                    case Type charType when charType == typeof(char):
                    case Type floatType when floatType == typeof(float):
                    case Type decimalType when decimalType == typeof(decimal):
                    case Type doubleType when doubleType == typeof(double):
                        foundField.SetValue(fieldContainer, 0);
                        break;
                    default:
                        throw new Exception($"The field of type {foundField.FieldType.Name} is unknown.");
                }
            }
            else
            {
                foundField.SetValue(fieldContainer, null);
            }
        }

        public static bool IsFieldPrimitive(this object fieldContainer, string name)
        {
            var foundField = fieldContainer.GetField(name);

            return foundField.FieldType.IsPrimitive;
        }
    }
}

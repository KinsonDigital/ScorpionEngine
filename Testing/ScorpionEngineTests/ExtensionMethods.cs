// <copyright file="ExtensionMethods.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngineTests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides extensions to various things to help make better code.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns <see cref="FieldInfo"/> of a field that matches the field given <paramref name="name"/>
        /// inside of the given object <paramref name="value"/> instance.
        /// </summary>
        /// <param name="value">The object that does or does not contain the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <returns>Information about the field.</returns>
        public static FieldInfo GetField(this object value, string name)
        {
            if (value is null)
            {
                return null;
            }

            var privateFields = (from f in value.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
                                 where f.Name == name
                                 select f).ToArray();

            // If the list is not found throw not found exception
            if (privateFields == null || privateFields.Length <= 0)
            {
                throw new Exception($"Cannot find the field {name} on the given object of type {value.GetType()}");
            }

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

        /// <summary>
        /// Returns a value indicating if the this object with the given field <paramref name="name"/>
        /// is a primitive in the object instance.
        /// </summary>
        /// <param name="fieldContainer">The object that contains the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <returns>
        ///     <see langword="true"/> if the field is a primitive.
        /// </returns>
        public static bool IsFieldPrimitive(this object fieldContainer, string name)
        {
            var foundField = fieldContainer.GetField(name);

            return foundField.FieldType.IsPrimitive;
        }

        /// <summary>
        /// Converts the given list of items of type <typeparamref name="T"/>
        /// to a <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of item in the current array of items and resulting collection.</typeparam>
        /// <param name="items">The list of items to convert.</param>
        /// <returns>The original list of items converted to a read only collection.</returns>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this T[] items)
            => new ReadOnlyCollection<T>(items);
    }
}

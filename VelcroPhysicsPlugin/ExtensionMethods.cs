using ScorpionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VelcroPhysics.Primitives;
using VelcroPhysics.Shared;

namespace VelcroPhysicsPlugin
{
    public static class ExtensionMethods
    {
        private const float PI = 3.1415926535897931f;
        private const float unitToPixel = 100.0f;
        private const float pixelToUnit = 1f / unitToPixel;


        public static float ToPixels(this float value)
        {
            return value * unitToPixel;
        }


        public static float ToPhysics(this float value)
        {
            return value * pixelToUnit;
        }

        
        public static float[] ToPixels(this float[] value)
        {
            return (from p in value select p.ToPixels()).ToArray();
        }


        public static int ToPixels(this int value)
        {
            return (int)(value * unitToPixel);
        }


        public static int ToPhysics(this int value)
        {
            return (int)(value * pixelToUnit);
        }


        public static IVector ToPhysics(this IVector value)
        {
            value.X = value.X.ToPhysics();
            value.Y = value.Y.ToPhysics();

            return value;
        }


        public static IVector ToPixels(this IVector value)
        {
            value.X = ToPixels(value.X);
            value.Y = ToPixels(value.Y);

            return value;
        }


        public static Vector2 ToVelcroVector(this IVector value)
        {
            return new Vector2(value.X, value.Y);
        }


        public static VelcroVector ToVelcroVector(this Vector2 value)
        {
            return new VelcroVector(value.X, value.Y);
        }


        public static VelcroVector[] ToVelcroVector(this Vector2[] value)
        {
            return value.Select(v => v.ToVelcroVector()).ToArray();
        }


        public static VelcroVector[] ToVelcroVectors(this Vertices value)
        {
            var result = new List<VelcroVector>();

            foreach (var v in value)
            {
                result.Add(v.ToVelcroVector());
            }


            return result.ToArray();
        }


        public static Vector2 ToPhysics(this Vector2 value)
        {
            value.X = value.X.ToPhysics();
            value.Y = value.Y.ToPhysics();


            return value;
        }


        public static Vector2 ToPixels(this Vector2 value)
        {
            value.X = ToPixels(value.X);
            value.Y = ToPixels(value.Y);

            return value;
        }


        public static IVector[] ToPixels(this IVector[] value)
        {
            return (from v in value select v.ToPixels()).ToArray();
        }


        public static IVector[] ToPhysics(this IVector[] value)
        {
            return (from v in value select v.ToPhysics()).ToArray();
        }


        public static List<IVector> ToPixels(this List<IVector> value)
        {
            return (from v in value select v.ToPixels()).ToList();
        }


        public static List<IVector> ToPhysics(this List<IVector> value)
        {
            return (from v in value select v.ToPhysics()).ToList();
        }


        public static List<Vector2> ToPixels(this List<Vector2> value)
        {
            return (from v in value select v.ToPixels()).ToList();
        }


        public static List<Vector2> ToPhysics(this List<Vector2> value)
        {
            return (from v in value select v.ToPhysics()).ToList();
        }


        public static string ToString(this IVector value, int round)
        {
            return $"X: {Math.Round(value.X, round).ToString()} - Y: {Math.Round(value.Y, round).ToString()}";
        }


        public static string ToString(this Vector2 value, int round)
        {
            return $"X: {Math.Round(value.X, round).ToString()} - Y: {Math.Round(value.Y, round).ToString()}";
        }


        public static string ToString(this float value, int round)
        {
            return Math.Round(value, round).ToString();
        }


        public static float ToDegrees(this float radians)
        {
            return radians * 180.0f / PI;
        }


        public static float ToRadians(this float degrees)
        {
            return degrees * PI / 180f;
        }


        public static float ToDegrees(this int radians)
        {
            return ToDegrees((float)radians);
        }


        public static float ToRadians(this int degrees)
        {
            return ToRadians((float)degrees);
        }
    }
}

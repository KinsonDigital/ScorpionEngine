using ScorpionEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using VelcroVector = VelcroPhysics.Primitives.Vector2;

namespace VelcroPhysicsDriver
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


        public static int ToPixels(this int value)
        {
            return (int)(value * unitToPixel);
        }


        public static int ToPhysics(this int value)
        {
            return (int)(value * pixelToUnit);
        }


        public static Vector ToMonoVector(this VelcroVector value)
        {
            return new Vector(value.X, value.Y);
        }


        public static Vector ToPhysics(this Vector value)
        {
            value.X = value.X.ToPhysics();
            value.Y = value.Y.ToPhysics();

            return value;
        }


        public static Vector ToPixels(this Vector value)
        {
            value.X = ToPixels(value.X);
            value.Y = ToPixels(value.Y);

            return value;
        }


        public static VelcroVector ToVelcroVector(this Vector value)
        {
            return new VelcroVector(value.X, value.Y);
        }


        public static VelcroVector ToPhysics(this VelcroVector value)
        {
            value.X = value.X.ToPhysics();
            value.Y = value.Y.ToPhysics();

            return value;
        }


        public static List<Vector> ToMonoVectors(this VelcroVector[] value)
        {
            return (from v in value
                    select v.ToMonoVector()).ToList();
        }


        public static List<VelcroVector> ToVelcroVectors(this Vector[] value)
        {
            return (from v in value
                    select v.ToVelcroVector()).ToList();
        }


        public static VelcroVector ToPixels(this VelcroVector value)
        {
            value.X = ToPixels(value.X);
            value.Y = ToPixels(value.Y);

            return value;
        }


        public static Vector[] ToPixels(this Vector[] value)
        {
            return (from v in value select v.ToPixels()).ToArray();
        }


        public static Vector[] ToPhysics(this Vector[] value)
        {
            return (from v in value select v.ToPhysics()).ToArray();
        }


        public static List<Vector> ToPixels(this List<Vector> value)
        {
            return (from v in value select v.ToPixels()).ToList();
        }


        public static List<Vector> ToPhysics(this List<Vector> value)
        {
            return (from v in value select v.ToPhysics()).ToList();
        }


        public static List<VelcroVector> ToPixels(this List<VelcroVector> value)
        {
            return (from v in value select v.ToPixels()).ToList();
        }


        public static List<VelcroVector> ToPhysics(this List<VelcroVector> value)
        {
            return (from v in value select v.ToPhysics()).ToList();
        }


        public static string ToString(this Vector value, int round)
        {
            return $"X: {Math.Round(value.X, round).ToString()} - Y: {Math.Round(value.Y, round).ToString()}";
        }


        public static string ToString(this VelcroVector value, int round)
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

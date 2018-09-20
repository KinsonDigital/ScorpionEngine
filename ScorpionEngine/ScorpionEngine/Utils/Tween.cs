using System;

// ReSharper disable UnusedMember.Global
/**
 * Tweener
 * Animates the value of a double property between two target values using 
 * Robert Penner's easing equations for interpolation over a specified Duration.
 *
 * @author		Darren David darren-code@lookorfeel.com
 * @version		1.0
 *
 * Credit/Thanks:
 * Robert Penner - The easing equations we all know and love 
 *   (http://robertpenner.com/easing/) [See License.txt for license info]
 * 
 * Lee Brimelow - initial port of Penner's equations to WPF 
 *   (http://thewpfblog.com/?p=12)
 * 
 * Zeh Fernando - additional equations (out/in) from 
 *   caurina.transitions.Tweener (http://code.google.com/p/tweener/)
 *   [See License.txt for license info]
 */

namespace ScorpionEngine.Utils
{
    /// <summary>
    ///     Animates the value of a double property between two target values using
    ///     Robert Penner's easing equations for interpolation over a specified Duration.
    /// </summary>
    /// <example>
    ///     <code>
    /// // C#
    /// Tweener anim = new Tweener();
    /// anim.Type = Tweener.EquationType.Linear;
    /// anim.From = 1;
    /// anim.To = 0;
    /// myControl.BeginAnimation( OpacityProperty, anim );
    /// 
    /// // XAML
    /// <Storyboard x:Key="AnimateXamlRect">
    ///             <animation:Tweener
    ///                 Storyboard.TargetName="myControl"
    ///                 Storyboard.TargetProperty="(Canvas.Left)"
    ///                 From="0"
    ///                 To="600"
    ///                 EquationType="BackEaseOut"
    ///                 Duration="00:00:05" />
    ///         </Storyboard>
    /// 
    /// <Control.Triggers>
    ///             <EventTrigger RoutedEvent="FrameworkElement.Loaded">
    ///                 <BeginStoryboard Storyboard="{StaticResource AnimateXamlRect}" />
    ///             </EventTrigger>
    ///         </Control.Triggers>
    /// </code>
    /// </example>
    public class Tweener
    {
        #region Easing Functions
        #region Linear
        /// <summary>
        ///     Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double Linear(double t, double b, double c, double d)
        {
            return c * t / d + b;
        }
        #endregion

        #region Expo
        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ExpoEaseOut(double t, double b, double c, double d)
        {
            return Math.Abs(t - d) < 0.0 ? b + c : c * (- Math.Pow(2, - 10 * t / d) + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ExpoEaseIn(double t, double b, double c, double d)
        {
            return Math.Abs(t) < 0.0 ? b : c * Math.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ExpoEaseInOut(double t, double b, double c, double d)
        {
            if (t == 0)
                return b;

            if (t == d)
                return b + c;

            if ((t /= d / 2) < 1)
                return c / 2 * Math.Pow(2, 10 * (t - 1)) + b;

            return c / 2 * (- Math.Pow(2, - 10 * --t) + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for an exponential (2^t) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ExpoEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return ExpoEaseOut(t * 2, b, c / 2, d);

            return ExpoEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Circular
        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CircEaseOut(double t, double b, double c, double d)
        {
            return c * Math.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CircEaseIn(double t, double b, double c, double d)
        {
            return - c * (Math.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CircEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return - c / 2 * (Math.Sqrt(1 - t * t) - 1) + b;

            return c / 2 * (Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a circular (sqrt(1-t^2)) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CircEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return CircEaseOut(t * 2, b, c / 2, d);

            return CircEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Quad
        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuadEaseOut(double t, double b, double c, double d)
        {
            return - c * (t /= d) * (t - 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuadEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t + b;
        }

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuadEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t + b;

            return - c / 2 * (--t * (t - 2) - 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a quadratic (t^2) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuadEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return QuadEaseOut(t * 2, b, c / 2, d);

            return QuadEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Sine
        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double SineEaseOut(double t, double b, double c, double d)
        {
            return c * Math.Sin(t / d * (Math.PI / 2)) + b;
        }

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double SineEaseIn(double t, double b, double c, double d)
        {
            return - c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double SineEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * Math.Sin(Math.PI * t / 2) + b;

            return - c / 2 * (Math.Cos(Math.PI * --t / 2) - 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a sinusoidal (sin(t)) easing in/out:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double SineEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return SineEaseOut(t * 2, b, c / 2, d);

            return SineEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Cubic
        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CubicEaseOut(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CubicEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t + b;
        }

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CubicEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t + b;

            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a cubic (t^3) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double CubicEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return CubicEaseOut(t * 2, b, c / 2, d);

            return CubicEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Quartic
        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuartEaseOut(double t, double b, double c, double d)
        {
            return - c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuartEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t * t + b;
        }

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuartEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t + b;

            return - c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a quartic (t^4) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuartEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return QuartEaseOut(t * 2, b, c / 2, d);

            return QuartEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Quintic
        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuintEaseOut(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuintEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuintEaseInOut(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a quintic (t^5) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double QuintEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return QuintEaseOut(t * 2, b, c / 2, d);
            return QuintEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Elastic
        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ElasticEaseOut(double t, double b, double c, double d)
        {
            if (Math.Abs((t /= d) - 1) < 0.0)
                return b + c;

            var p = d * .3;
            var s = p / 4;

            return c * Math.Pow(2, - 10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b;
        }

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ElasticEaseIn(double t, double b, double c, double d)
        {
            if (Math.Abs((t /= d) - 1) < 0.0)
                return b + c;

            var p = d * .3;
            var s = p / 4;

            return - (c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
        }

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ElasticEaseInOut(double t, double b, double c, double d)
        {
            if (Math.Abs((t /= d / 2) - 2) < 0.0)
                return b + c;

            var p = d * (.3 * 1.5);
            var s = p / 4;

            if (t < 1)
                return - .5 * (c * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
            return c * Math.Pow(2, - 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
        }

        /// <summary>
        ///     Easing equation function for an elastic (exponentially decaying sine wave) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double ElasticEaseOutIn(double t, double b, double c, double d)
        {
            return t < d / 2 ? ElasticEaseOut(t * 2, b, c / 2, d) : ElasticEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Bounce
        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BounceEaseOut(double t, double b, double c, double d)
        {
            if ((t /= d) < 1 / 2.75)
                return c * (7.5625 * t * t) + b;
            if (t < 2 / 2.75)
                return c * (7.5625 * (t -= 1.5 / 2.75) * t + .75) + b;
            if (t < 2.5 / 2.75)
                return c * (7.5625 * (t -= 2.25 / 2.75) * t + .9375) + b;
            return c * (7.5625 * (t -= 2.625 / 2.75) * t + .984375) + b;
        }

        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BounceEaseIn(double t, double b, double c, double d)
        {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }

        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BounceEaseInOut(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BounceEaseIn(t * 2, 0, c, d) * .5 + b;
            return BounceEaseOut(t * 2 - d, 0, c, d) * .5 + c * .5 + b;
        }

        /// <summary>
        ///     Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BounceEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BounceEaseOut(t * 2, b, c / 2, d);
            return BounceEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion

        #region Back
        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out:
        ///     decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BackEaseOut(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * ((1.70158 + 1) * t + 1.70158) + 1) + b;
        }

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in:
        ///     accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BackEaseIn(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * ((1.70158 + 1) * t - 1.70158) + b;
        }

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out:
        ///     acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BackEaseInOut(double t, double b, double c, double d)
        {
            var s = 1.70158;
            if ((t /= d / 2) < 1)
                return c / 2 * (t * t * (((s *= 1.525) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= 1.525) + 1) * t + s) + 2) + b;
        }

        /// <summary>
        ///     Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in:
        ///     deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public double BackEaseOutIn(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BackEaseOut(t * 2, b, c / 2, d);
            return BackEaseIn(t * 2 - d, b + c / 2, c / 2, d);
        }
        #endregion
        #endregion
    }
}
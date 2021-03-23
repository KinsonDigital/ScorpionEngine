// <copyright file="RenderSection.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Graphics
{
    using System.Drawing;
    using System.Numerics;
    using Raptor.Content;

    /// <summary>
    /// The section of a texture to be rendered to the screen.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     If the <see cref="TypeOfTexture"/> is set to <see cref="TextureType.WholeTexture"/>,
    ///     then the entire texture will be rendered.
    /// </para>
    ///
    /// <para>
    ///     If the <see cref="TypeOfTexture"/> is set to <see cref="TextureType.SubTexture"/>,
    ///     then only a section of the entire texture is rendered.  This could be a single frame
    ///     non-animating entity or a single frame out of multiple frames of an animation for
    ///     and animated entity.
    /// </para>
    /// </remarks>
    public class RenderSection
    {
        /// <summary>
        /// Gets or sets the name of the texture that will be rendered.
        /// </summary>
        public string TextureName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the sub texture in the entire texture that will be rendered.
        /// </summary>
        /// <remarks>
        ///     This is the name of the section in the atlas data.
        /// </remarks>
        public string? SubTextureName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rectangular bounds of the texture to render.
        /// </summary>
        /// <remarks>
        ///     This could be the entire texture of just a section of the texture.
        /// </remarks>
        public Rectangle RenderBounds { get; set; }

        /// <summary>
        /// Gets half of the width of the render section to be rendered.
        /// </summary>
        /// <remarks>
        ///     This is half of the width of the <see cref="Rectangle.Width"/> of the <see cref="RenderBounds"/>.
        /// </remarks>
        public int HalfWidth => RenderBounds.Width / 2;

        /// <summary>
        /// Gets half of the height of the render section to be rendered.
        /// </summary>
        /// <remarks>
        ///     This is half of the height of the <see cref="Rectangle.Height"/> of the <see cref="RenderBounds"/>.
        /// </remarks>
        public int HalfHeight => RenderBounds.Height / 2;

        /// <summary>
        /// Gets or sets the type of texture that is gonna be rendered.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     If set to <see cref="TextureType.WholeTexture"/>,
        ///     then the entire texture will be rendered.
        /// </para>
        ///
        /// <para>
        ///     If set to <see cref="TextureType.SubTexture"/>,
        ///     then only a section of the entire texture is rendered.  This could be a single frame
        ///     non-animated entity or a single frame out of multiple frames of an animation for
        ///     and animated entity.
        /// </para>
        /// </remarks>
        public TextureType TypeOfTexture { get; set; }

        /// <summary>
        /// Gets or sets the animator.
        /// </summary>
        public IAnimator? Animator { get; set; }

        /// <summary>
        /// Creates a non-animating section that renders the entire texture.
        /// </summary>
        /// <param name="wholeTextureName">The name of the entire texture to render.</param>
        /// <returns>
        ///     Non-animating section that results in rendering the entire texture.
        /// </returns>
        public static RenderSection CreateNonAnimatedWholeTexture(string wholeTextureName)
            => new RenderSection()
            {
                TextureName = wholeTextureName,
                SubTextureName = string.Empty,
                TypeOfTexture = TextureType.WholeTexture,
            };

        /// <summary>
        /// Creates a non-animating section of a texture atlas that matches the <paramref name="subTextureName"/>.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas.</param>
        /// <param name="subTextureName">The name of the sub texture inside of the atlas.</param>
        /// <returns>
        ///     Non-animating section that results in rendering a sub texture of an atlas texture.
        /// </returns>
        public static RenderSection CreateNonAnimatedSubTexture(string atlasTextureName, string subTextureName)
            => new RenderSection()
            {
                TextureName = atlasTextureName,
                SubTextureName = subTextureName,
                TypeOfTexture = TextureType.SubTexture,
            };

        /// <summary>
        /// Creates an animating section of a texture atlas that matches the <paramref name="subTextureName"/>.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas.</param>
        /// <param name="subTextureName">The name of the sub texture inside of the atlas.</param>
        /// <returns>
        ///     An animating section that results in rendering multiple frames in an atlas texture.
        /// </returns>
        public static RenderSection CreateAnimatedSubTexture(string atlasTextureName, string subTextureName)
            => new RenderSection()
            {
                TextureName = atlasTextureName,
                SubTextureName = subTextureName,
                Animator = new Animator(),
                TypeOfTexture = TextureType.SubTexture
            };

        /// <summary>
        /// Creates an animating section of a texture atlas that matches the <paramref name="subTextureName"/>.
        /// </summary>
        /// <param name="atlasTextureName">The name of the texture atlas.</param>
        /// <param name="subTextureName">The name of the sub texture inside of the atlas.</param>
        /// <param name="animator">The animator to use in the animation.</param>
        /// <returns>
        ///     An animating section that results in rendering multiple frames in an atlas texture.
        /// </returns>
        public static RenderSection CreateAnimatedSubTexture(string atlasTextureName, string subTextureName, IAnimator animator)
            => new RenderSection()
            {
                TextureName = atlasTextureName,
                SubTextureName = subTextureName,
                Animator = animator,
                TypeOfTexture = TextureType.SubTexture
            };

        /// <summary>
        /// Resets the state of the <see cref="RenderSection"/>.
        /// </summary>
        public void Reset()
        {
            TextureName = string.Empty;
            SubTextureName = string.Empty;
            RenderBounds = Rectangle.Empty;
            TypeOfTexture = TextureType.WholeTexture;
        }

        /// <summary>
        /// Gets the position within the texture based on the type of texture and if it is animated.
        /// </summary>
        /// <returns>The position of the area of the texture to render.</returns>
        public Vector2 GetTexturePosition()
        {
            if (!(Animator is null))
            {
                return new Vector2(RenderBounds.X, RenderBounds.Y);
            }
            else
            {
                switch (TypeOfTexture)
                {
                    case TextureType.WholeTexture:
                        return Vector2.Zero;
                    case TextureType.SubTexture:
                        return new Vector2(RenderBounds.X, RenderBounds.Y);
                    default:
                        return Vector2.Zero;
                }
            }
        }
    }
}

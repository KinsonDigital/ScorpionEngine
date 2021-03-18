// <copyright file="EntityFactory.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KDScorpionEngine.Factories
{
    using KDScorpionEngine.Entities;
    using KDScorpionEngine.Graphics;
    using Raptor.Content;

    /// <summary>
    /// Creates new instances of entities.
    /// </summary>
    public class EntityFactory : IEntityFactory
    {
        /// <inheritdoc/>1
        public IEntity CreateAnimated<TEntity>(string atlasName, string subTextureName)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = atlasName;
            newEntity.SectionToRender.SubTextureName = subTextureName;
            newEntity.SectionToRender.Animator = new Animator();
            newEntity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            return newEntity;
        }

        /// <inheritdoc/>
        public IEntity CreateAnimated<TEntity>(string atlasName, string subTextureName, IAnimator animator)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = atlasName;
            newEntity.SectionToRender.SubTextureName = subTextureName;
            newEntity.SectionToRender.Animator = animator;
            newEntity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            return newEntity;
        }

        /// <inheritdoc/>
        public IEntity CreateNonAnimatedFromTexture<TEntity>(string name)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = name;
            newEntity.SectionToRender.SubTextureName = string.Empty;
            newEntity.SectionToRender.Animator = null;
            newEntity.SectionToRender.TypeOfTexture = TextureType.WholeTexture;

            return newEntity;
        }

        /// <inheritdoc/>
        public IEntity CreateNonAnimatedFromTextureAtlas<TEntity>(string atlasName, string subTextureName)
            where TEntity : IEntity, new()
        {
            var newEntity = new TEntity();

            newEntity.SectionToRender.TextureName = atlasName;
            newEntity.SectionToRender.SubTextureName = subTextureName;
            newEntity.SectionToRender.Animator = null;
            newEntity.SectionToRender.TypeOfTexture = TextureType.SubTexture;

            return newEntity;
        }
    }
}

// <copyright file="Entity.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    using System;

    /// <summary>
    /// Базовый  класс Сущность.
    /// </summary>
    /// <typeparam name="TEntity"> Тип конкретной сущности. </typeparam>
    public abstract class Entity<TEntity> : IEntity<TEntity>, IEquatable<TEntity>
       where TEntity : Entity<TEntity>
    {
        /// <inheritdoc/>
        public Guid Id { get; }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"[{this.Id}]";

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj)
                || (obj is TEntity entity && this.Equals(entity));
        }

        /// <inheritdoc/>
        public virtual bool Equals(TEntity? other)
        {
            return ReferenceEquals(this, other)
                || (other is not null
                && this.GetType() == other.GetType()
                && this.Id != Guid.Empty
                && other.Id != Guid.Empty
                && this.Id == other.Id);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
#pragma warning disable S3249
            return this.Id == Guid.Empty ? base.GetHashCode() : this.Id.GetHashCode();
#pragma warning restore S3249
        }
    }
}

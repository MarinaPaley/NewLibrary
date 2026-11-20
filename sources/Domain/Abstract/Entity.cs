// <copyright file="Entity.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Базовый  класс Сущность.
    /// </summary>
    /// <typeparam name="TEntity"> Тип конкретной сущности. </typeparam>
    public abstract class Entity<TEntity> : IEntity<TEntity>, IEquatable<TEntity>
       where TEntity : Entity<TEntity>
    {
        /// <inheritdoc/>
        public Guid Id { get; protected set; }

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
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is null || this.GetType() != other.GetType())
            {
                return false;
            }

            if (this.Id != Guid.Empty && other.Id != Guid.Empty)
            {
                return this.Id == other.Id;
            }

            return this.EqualsFields(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Id == Guid.Empty
                    ? this.GetHashCodeFields()
                    : this.Id.GetHashCode();
        }

        /// <summary>
        /// Сравнение по бизнес-полям. Вызывается, только если Id совпадают
        /// (в частности — для двух новых сущностей с Id == Guid.Empty).
        /// </summary>
        /// <param name="other"> Другой объект.</param>
        /// <returns> <see langword="true">если остальные поля равны</see>, иначе - <see langword="false"/>.</returns>
        protected virtual bool EqualsFields(TEntity other) => ReferenceEquals(this, other);

        /// <summary> Хэш бизнес-полей. Согласован с <see cref="EqualsFields"/>.</summary>
        /// <returns> Хэш бизнес-полей.</returns>
        protected virtual int GetHashCodeFields() => RuntimeHelpers.GetHashCode(this);
    }
}

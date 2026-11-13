// <copyright file="NamedEntity.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Abstract
{
    using System;
    using Staff;

    /// <summary>
    /// Базовый класс именованной сущности.
    /// </summary>
    /// <typeparam name="TEntity"> Конкретный тип сущности. </typeparam>
    public abstract class NamedEntity<TEntity> : Entity<TEntity>, INamedEntity<TEntity>
        where TEntity : NamedEntity<TEntity>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NamedEntity{TEntity}"/>.
        /// </summary>
        /// <param name="name"> Название. </param>
        protected NamedEntity(string name)
            : base()
        {
            this.Name = name;
        }

        /// <summary>
        /// Значение названия.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// В случае если входное значение <see langword="null"/>.
        /// </exception>
        public string Name
        {
            get;
            set => field = value.TrimOrNull() ?? throw new ArgumentException("Неправильное имя", paramName: value);
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"[{this.Name}]";

        /// <inheritdoc/>
        protected override bool EqualsFields(TEntity other) => string.Equals(this.Name, other.Name, StringComparison.Ordinal);

        /// <inheritdoc/>
        protected override int GetHashCodeFields() => StringComparer.Ordinal.GetHashCode(this.Name);
    }
}

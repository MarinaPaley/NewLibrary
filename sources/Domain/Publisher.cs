// <copyright file="Publisher.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using Domain.Abstract;

    /// <summary>
    /// Издательство.
    /// </summary>
    public sealed class Publisher : Entity, IEquatable<Publisher>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Publisher"/>.
        /// </summary>
        /// <param name="name"> Название. </param>
        /// <exception cref="ArgumentNullException"> В случае <paramref name="name"/> равным <see langword="null"/>. </exception>
        public Publisher(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public override Guid Id { get; set; }

        /// <summary>
        /// Название книги.
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc/>
        public bool Equals(Publisher? other)
        {
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            if (other is null)
            {
                return false;
            }

            return string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Publisher other && this.Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(this.Name);

        /// <inheritdoc/>
        public override string ToString() => $"value is {this.Name}";
    }
}

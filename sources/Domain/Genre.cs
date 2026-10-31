// <copyright file="Genre.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;

    /// <summary>
    /// Жанр.
    /// </summary>
    public sealed class Genre : IEquatable<Genre>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Genre"/>.
        /// </summary>
        /// <param name="name"> Название жанра. </param>
        /// <exception cref="ArgumentNullException"> в случае <paramref name="name"/> равным <see langword="null"/>.</exception>
        public Genre(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Жанр.
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc/>
        public bool Equals(Genre? other)
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
        public override bool Equals(object? obj) => obj is Genre other && this.Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(this.Name);
    }
}

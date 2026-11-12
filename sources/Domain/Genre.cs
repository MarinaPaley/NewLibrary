// <copyright file="Genre.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;

    /// <summary>
    /// Жанр.
    /// </summary>
    public sealed class Genre : NamedEntity<Genre>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Genre"/>.
        /// </summary>
        /// <param name="name"> Название жанра. </param>
        /// <exception cref="ArgumentNullException"> в случае <paramref name="name"/> равным <see langword="null"/>.</exception>
        public Genre(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Книги.
        /// </summary>
        public ISet<Book> Books { get; set; } = new HashSet<Book>();
    }
}

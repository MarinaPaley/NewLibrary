// <copyright file="Book.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using Domain.Abstract;
    using Staff;

    /// <summary>
    /// Книга.
    /// </summary>
    public sealed class Book : NamedEntity<Book>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Book"/>.
        /// </summary>
        /// <param name="name"> Название книги. </param>
        /// <param name="pages"> Количество страниц. </param>
        /// <param name="isbn"> ISBN. </param>
        /// <param name="year"> Год издания.</param>
        /// <param name="publisher"> Издательство. </param>
        /// <param name="authors"> Авторы.</param>
        /// <exception cref="ArgumentNullException"> В случае <paramref name="name"/> равным <see langword="null"/>. </exception>
        public Book(string name, int pages, string? isbn, int year, Publisher publisher, params Author[] authors)
            : base(name)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pages);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(year, DateTime.Now.Year);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(year);
            this.Pages = pages;
            this.ISBN = isbn?.TrimOrNull();
            this.Year = year;
            this.Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            publisher.Books.Add(this);
            this.Authors = new HashSet<Author>(authors);

            foreach (var author in authors)
            {
                author.AddBook(this);
            }
        }

        /// <summary>
        /// Издательство.
        /// </summary>
        public Publisher Publisher { get; private init; }

        /// <summary>
        /// Авторы.
        /// </summary>
        public ISet<Author> Authors { get; private init; }

        /// <summary>
        /// Количество страниц.
        /// </summary>
        public int Pages { get; private init; }

        /// <summary>
        /// ISBN.
        /// </summary>
        public string? ISBN { get; private init; }

        /// <summary>
        /// Год издания.
        /// </summary>
        public int Year { get; private init; }

        /// <summary>
        /// Жанр.
        /// </summary>
        public Genre? Genre { get; set; }

        /// <summary>
        /// Полка.
        /// </summary>
        public Shelf? Shelf { get; set; }

        /// <summary>
        /// Добавляет жанр.
        /// </summary>
        /// <param name="genre"> Жанр. </param>
        /// <returns><see langword="true"/>, если добавили, иначе - <see langword="false"/>.</returns>
        public bool AddGenre(Genre genre)
        {
            if (genre is null)
            {
                return false;
            }

            this.Genre = genre;
            return genre.Books.Add(this);
        }

        /// <summary>
        /// Удаляет жанр.
        /// </summary>
        /// <param name="genre"> Жанр.</param>
        /// <returns> <see langword="true"/>, если удалили, иначе - <see langword="false"/>.</returns>
        public bool RemoveGenre(Genre genre)
        {
            if (genre is null)
            {
                return false;
            }

            this.Genre = null;
            return genre.Books.Remove(this);
        }

        /// <inheritdoc/>
        public override bool Equals(Book? other)
        {
            if (!base.Equals(other))
            {
                return false;
            }

            if (this.ISBN is not null && other.ISBN is not null)
            {
                return string.Equals(this.ISBN, other.ISBN, StringComparison.Ordinal);
            }

            if (this.ISBN is not null || other.ISBN is not null)
            {
                return false;
            }

            return string.Equals(this.Name, other.Name, StringComparison.Ordinal)
                && this.Pages == other.Pages
                && this.Year == other.Year
                && this.Publisher.Equals(other.Publisher)
                && this.Authors.SetEquals(other.Authors);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Book book && this.Equals(book);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (this.Id != Guid.Empty)
            {
                return this.Id.GetHashCode();
            }

            if (this.ISBN is not null)
            {
                return StringComparer.Ordinal.GetHashCode(this.ISBN);
            }

            return HashCode.Combine(this.Name, this.Authors, this.Pages, this.Publisher, this.Year);
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"{base.ToString()}: [{this.Authors.Join()}]";
        }
    }
}

// <copyright file="Book.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            ArgumentNullException.ThrowIfNull(authors);
            if (authors.Length == 0)
            {
                throw new ArgumentException("Коллекция не может быть пустой.", nameof(authors));
            }

            this.Pages = pages;
            this.ISBN = isbn?.TrimOrNull();
            this.Year = year;
            this.Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            publisher.Books.Add(this);

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
        public ISet<Author> Authors { get; private init; } = new HashSet<Author>();

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

            if (ReferenceEquals(this.Genre, genre))
            {
                return false;
            }

            this.Genre?.Books.Remove(this);

            this.Genre = genre;
            genre.Books.Add(this);

            return true;
        }

        /// <summary>
        /// Удаляет жанр у книги.
        /// </summary>
        /// <param name="genre"> Жанр. </param>
        /// <returns> <see langword="true"/>, если удалили, иначе - <see langword="false"/>.</returns>
        public bool RemoveGenre(Genre genre)
        {
            if (genre is null || !ReferenceEquals(this.Genre, genre))
            {
                return false;
            }

            this.Genre = null;
            return genre.Books.Remove(this);
        }

        /// <inheritdoc cref="object.ToString()"/>
        public override string ToString()
        {
            return $"{base.ToString()}: [{this.Authors.Join()}]";
        }

        /// <inheritdoc/>
        protected override bool EqualsFields(Book other)
        {
            if (!base.EqualsFields(other))
            {
                return false;
            }

            if (this.ISBN is not null || other.ISBN is not null)
            {
                return string.Equals(this.ISBN, other.ISBN, StringComparison.Ordinal);
            }

            return this.Pages == other.Pages
                && this.Year == other.Year
                && this.Publisher.Equals(other.Publisher)
                && this.Authors.SetEquals(other.Authors);
        }

        /// <inheritdoc/>
        protected override int GetHashCodeFields()
        {
            return this.ISBN is not null
                ? HashCode.Combine(base.GetHashCodeFields(), this.ISBN)
                : HashCode.Combine(
                    base.GetHashCodeFields(),
                    this.Pages,
                    this.Year,
                    this.Publisher,
                    this.Authors.Count);
        }
    }
}

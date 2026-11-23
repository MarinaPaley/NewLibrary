// <copyright file="BookTests.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public sealed class BookTests
    {
        [Test]
        public void Constructor_Valid_SetsPropertiesAndBackReferences()
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");

            // act
            var book = new Book("Title", 300, "123", 2000, publisher, author);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(book.Name, Is.EqualTo("Title"));
                Assert.That(book.Pages, Is.EqualTo(300));
                Assert.That(book.ISBN, Is.EqualTo("123"));
                Assert.That(book.Year, Is.EqualTo(2000));
                Assert.That(book.Publisher, Is.SameAs(publisher));
                Assert.That(book.Authors, Contains.Item(author));
                Assert.That(publisher.Books, Contains.Item(book));
                Assert.That(author.Books, Contains.Item(book));
            }
        }

        [Test]
        public void Constructor_NullName_Throws()
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");

            // act & assert
            Assert.Throws<ArgumentException>(() => new Book(null!, 300, isbn: null, 2000, publisher, author));
        }

        [TestCaseSource(nameof(InvalidPagesCases))]
        public void Constructor_InvalidPages_Throws(int pages)
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Book("Book", pages, isbn: null, 2000, publisher, author));
        }

        [TestCaseSource(nameof(InvalidYearCases))]
        public void Constructor_InvalidYear_Throws(int year)
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Book("Book", 100, isbn: null, year, publisher, author));
        }

        [Test]
        public void Constructor_FutureYear_Throws()
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Book("Book", 100, isbn: null, DateTime.Now.Year + 1, publisher, author));
        }

        [Test]
        public void Constructor_NullPublisher_Throws()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");

            // act & assert
            Assert.Throws<ArgumentNullException>(
                () => new Book("Book", 100, isbn: null, 2000, publisher: null!, author));
        }

        [Test]
        public void Constructor_NullAuthorsArray_Throws()
        {
            // arrange
            var publisher = new Publisher("Publisher");

            // act & assert
            Assert.Throws<ArgumentNullException>(
                () => new Book("Book", 100, isbn: null, 2000, publisher, (Author[])null!));
        }

        [Test]
        public void Constructor_EmptyAuthors_Throws()
        {
            // arrange
            var publisher = new Publisher("Publisher");

            // act & assert
            Assert.Throws<ArgumentException>(() => new Book("Book", 100, isbn: null, 2000, publisher));
        }

        [Test]
        public void Constructor_NullIsbn_Allowed()
        {
            // arrange & act
            var book = CreateBook();

            // assert
            Assert.That(book.ISBN, Is.Null);
        }

        [TestCaseSource(nameof(WhitespaceIsbnCases))]
        public void Constructor_WhitespaceIsbn_TrimmedToNull(string isbn)
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");

            // act
            var book = new Book("Book", 100, isbn, 2000, publisher, author);

            // assert
            Assert.That(book.ISBN, Is.Null);
        }

        [Test]
        public void AddGenre_Null_ReturnsFalse()
        {
            // arrange
            var book = CreateBook();

            // act
            var added = book.AddGenre(null!);

            // assert
            Assert.That(added, Is.False);
        }

        [Test]
        public void AddGenre_SetsGenreAndBackReference()
        {
            // arrange
            var book = CreateBook();
            var genre = new Genre("Fantasy");

            // act
            var added = book.AddGenre(genre);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(added, Is.True);
                Assert.That(book.Genre, Is.SameAs(genre));
                Assert.That(genre.Books, Contains.Item(book));
            }
        }

        [Test]
        public void AddGenre_SameGenreAgain_ReturnsFalse()
        {
            // arrange
            var book = CreateBook();
            var genre = new Genre("Fantasy");
            book.AddGenre(genre);

            // act
            var added = book.AddGenre(genre);

            // assert
            Assert.That(added, Is.False);
        }

        [Test]
        public void AddGenre_NewGenre_ReplacesOldOne()
        {
            // arrange
            var book = CreateBook();
            var oldGenre = new Genre("Genre1");
            var newGenre = new Genre("Genre2");

            // act
            book.AddGenre(oldGenre);
            book.AddGenre(newGenre);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(book.Genre, Is.SameAs(newGenre));
                Assert.That(oldGenre.Books, Does.Not.Contain(book));
                Assert.That(newGenre.Books, Contains.Item(book));
            }
        }

        [Test]
        public void RemoveGenre_Null_ReturnsFalse()
        {
            // arrange
            var book = CreateBook();

            // act
            var removed = book.RemoveGenre(null!);

            // assert
            Assert.That(removed, Is.False);
        }

        [Test]
        public void RemoveGenre_DifferentGenre_ReturnsFalse()
        {
            // arrange
            var book = CreateBook();
            var currentGenre = new Genre("Genre1");
            var otherGenre = new Genre("Genre2");
            book.AddGenre(currentGenre);

            // act
            var removed = book.RemoveGenre(otherGenre);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(removed, Is.False);
                Assert.That(book.Genre, Is.SameAs(currentGenre));
            }
        }

        [Test]
        public void RemoveGenre_Correct_Removes()
        {
            // arrange
            var book = CreateBook();
            var genre = new Genre("Genre");
            book.AddGenre(genre);

            // act
            var removed = book.RemoveGenre(genre);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(removed, Is.True);
                Assert.That(book.Genre, Is.Null);
                Assert.That(genre.Books, Does.Not.Contain(book));
            }
        }

        [Test]
        public void ToString_ContainsTitleAndAuthors()
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author(familyName: null, "ИмяАвтора");
            var book = new Book("MyBook", 100, isbn: null, 2000, publisher, author);

            // act
            var toString = book.ToString();

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(toString, Does.Contain("MyBook"));
                Assert.That(toString, Does.Contain("ИмяАвтора"));
            }
        }

        [TestCaseSource(nameof(BookEqualityCases))]
        public void Equals_ReturnsExpected(Book book1, Book book2, bool expected)
        {
            // act
            var actual = book1.Equals(book2);

            // assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetHashCode_SameIsbnAndName_Equal()
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");
            var book1 = new Book("Book", 100, "123", 2000, publisher, author);
            var book2 = new Book("Book", 200, "123", 2001, publisher, author);

            // act
            var hash1 = book1.GetHashCode();
            var hash2 = book2.GetHashCode();

            // assert
            Assert.That(hash1, Is.EqualTo(hash2));
        }

        [Test]
        public void GetHashCode_EqualBooks_EqualHash()
        {
            // arrange
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");
            var book1 = new Book("Book", 100, isbn: null, 2000, publisher, author);
            var book2 = new Book("Book", 100, isbn: null, 2000, publisher, author);

            // act & assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(book1, Is.EqualTo(book2));
                Assert.That(book1.GetHashCode(), Is.EqualTo(book2.GetHashCode()));
            }
        }

        private static Book CreateBook()
        {
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");
            return new Book("Book", 100, isbn: null, 2000, publisher, author);
        }

        private static IEnumerable<TestCaseData> InvalidPagesCases()
        {
            yield return new TestCaseData(0).SetArgDisplayNames("Zero");
            yield return new TestCaseData(-1).SetArgDisplayNames("Negative");
            yield return new TestCaseData(int.MinValue).SetArgDisplayNames("MinValue");
        }

        private static IEnumerable<TestCaseData> InvalidYearCases()
        {
            yield return new TestCaseData(0).SetArgDisplayNames("Zero");
            yield return new TestCaseData(-2).SetArgDisplayNames("Negative");
            yield return new TestCaseData(int.MinValue).SetArgDisplayNames("MinValue");
        }

        private static IEnumerable<TestCaseData> WhitespaceIsbnCases()
        {
            yield return new TestCaseData("   ").SetArgDisplayNames("Spaces");
            yield return new TestCaseData("\t\n").SetArgDisplayNames("TabAndNewLine");
        }

        private static IEnumerable<TestCaseData> BookEqualityCases()
        {
            var publisher1 = new Publisher("Publisher1");
            var publisher2 = new Publisher("Publisher2");
            var author1 = new Author("Иванов", "Иван");
            var author2 = new Author("Петров", "Пётр");

            yield return new TestCaseData(
                new Book("Book", 100, isbn: null, 2000, publisher1, author1),
                new Book("Book", 100, isbn: null, 2000, publisher1, author1),
                true)
                .SetArgDisplayNames("book1: SameFields", "book2: SameFields", "true");

            yield return new TestCaseData(
                new Book("Book", 100, "123", 2000, publisher1, author1),
                new Book("Book", 200, "123", 2001, publisher2, author2),
                true)
                .SetArgDisplayNames("book1: SameIsbn", "book2: SameIsbn", "true");

            yield return new TestCaseData(
                new Book("Book", 100, "123", 2000, publisher1, author1),
                new Book("Book", 100, "456", 2000, publisher1, author1),
                false)
                .SetArgDisplayNames("book1: isbn=123", "book2: isbn=456", "false");

            yield return new TestCaseData(
                new Book("Book", 100, "123", 2000, publisher1, author1),
                new Book("Book", 100, isbn: null, 2000, publisher1, author1),
                false)
                .SetArgDisplayNames("book1: isbn=123", "book2: isbn=null", "false");

            yield return new TestCaseData(
                new Book("Book", 100, isbn: null, 2000, publisher1, author1),
                new Book("Book", 200, isbn: null, 2000, publisher1, author1),
                false)
                .SetArgDisplayNames("book1: pages=100", "book2: pages=200", "false");

            yield return new TestCaseData(
                new Book("Book", 100, isbn: null, 2000, publisher1, author1),
                new Book("Book", 100, isbn: null, 2000, publisher2, author1),
                false)
                .SetArgDisplayNames("book1: Publisher1", "book2: Publisher2", "false");

            yield return new TestCaseData(
                new Book("Book", 100, isbn: null, 2000, publisher1, author1),
                new Book("Book", 100, isbn: null, 2000, publisher1, author2),
                false)
                .SetArgDisplayNames("book1: Иванов", "book2: Петров", "false");

            yield return new TestCaseData(
                new Book("Book1", 100, "123", 2000, publisher1, author1),
                new Book("Book2", 100, "123", 2000, publisher1, author1),
                false)
                .SetArgDisplayNames("book1: Book1", "book2: Book2", "false");
        }
    }
}

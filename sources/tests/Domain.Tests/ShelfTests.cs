// <copyright file="ShelfTests.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public sealed class ShelfTests
    {
        [TestCaseSource(nameof(InvalidNameCases))]
        public void Constructor_InvalidName_Throws(string? name)
        {
            // act & assert
            Assert.Throws<ArgumentException>(() => new Shelf(name!));
        }

        [Test]
        public void Constructor_Valid_InitializesFields()
        {
            // arrange & act
            var shelf = new Shelf("Shelf1");

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(shelf.Name, Is.EqualTo("Shelf1"));
                Assert.That(shelf.Books, Is.Not.Null);
                Assert.That(shelf.Books, Is.Empty);
            }
        }

        [Test]
        public void AddBook_Null_ReturnsFalse()
        {
            // arrange
            var shelf = new Shelf("Shelf");

            // act
            var added = shelf.AddBook(null!);

            // assert
            Assert.That(added, Is.False);
        }

        [Test]
        public void AddBook_SetsShelfAndAddsToCollection()
        {
            // arrange
            var shelf = new Shelf("Shelf");
            var book = CreateBook();

            // act
            var added = shelf.AddBook(book);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(added, Is.True);
                Assert.That(shelf.Books, Contains.Item(book));
                Assert.That(book.Shelf, Is.SameAs(shelf));
            }
        }

        [Test]
        public void AddBook_SameShelfTwice_ReturnsFalse()
        {
            // arrange
            var shelf = new Shelf("Shelf");
            var book = CreateBook();
            shelf.AddBook(book);

            // act
            var added = shelf.AddBook(book);

            // assert
            Assert.That(added, Is.False);
        }

        [Test]
        public void AddBook_ToAnotherShelf_MovesBook()
        {
            // arrange
            var shelf1 = new Shelf("Shelf1");
            var shelf2 = new Shelf("Shelf2");
            var book = CreateBook();
            shelf1.AddBook(book);

            // act
            shelf2.AddBook(book);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(shelf1.Books, Does.Not.Contain(book));
                Assert.That(shelf2.Books, Contains.Item(book));
                Assert.That(book.Shelf, Is.SameAs(shelf2));
            }
        }

        [Test]
        public void RemoveBook_Null_ReturnsFalse()
        {
            // arrange
            var shelf = new Shelf("Shelf");

            // act
            var removed = shelf.RemoveBook(null!);

            // assert
            Assert.That(removed, Is.False);
        }

        [Test]
        public void RemoveBook_RemovesAndClearsShelf()
        {
            // arrange
            var shelf = new Shelf("Shelf");
            var book = CreateBook();
            shelf.AddBook(book);

            // act
            var removed = shelf.RemoveBook(book);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(removed, Is.True);
                Assert.That(shelf.Books, Is.Empty);
                Assert.That(book.Shelf, Is.Null);
            }
        }

        [Test]
        public void RemoveBook_NotOnShelf_ReturnsFalse()
        {
            // arrange
            var shelf = new Shelf("Shelf");
            var book = CreateBook();

            // act
            var removed = shelf.RemoveBook(book);

            // assert
            Assert.That(removed, Is.False);
        }

        private static Book CreateBook()
        {
            var publisher = new Publisher("Publisher");
            var author = new Author("Фамилия", "Имя");
            return new Book("Book", 100, isbn: null, 2000, publisher, author);
        }

        private static IEnumerable<TestCaseData> InvalidNameCases()
        {
            yield return new TestCaseData((string?)null).SetArgDisplayNames("Null");
            yield return new TestCaseData(string.Empty).SetArgDisplayNames("Empty");
            yield return new TestCaseData("   ").SetArgDisplayNames("Whitespace");
        }
    }
}

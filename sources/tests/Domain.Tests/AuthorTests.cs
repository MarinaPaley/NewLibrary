// <copyright file="AuthorTests.cs" company="Филипченко Марина Алексеевна">
// Copyright (c) Филипченко Марина Алексеевна 2026. Library.
// </copyright>

namespace Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public sealed class AuthorTests
    {
        [Test]
        public void Constructor_SetsProperties()
        {
            // arrange & act
            var author = new Author("Иванов", "Иван", "Иванович");

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(author.FamilyName, Is.EqualTo("Иванов"));
                Assert.That(author.Name, Is.EqualTo("Иван"));
                Assert.That(author.Patronymic, Is.EqualTo("Иванович"));
                Assert.That(author.Books, Is.Empty);
                Assert.That(author.DateBirth, Is.Null);
                Assert.That(author.DateDeath, Is.Null);
                Assert.That(author.Id, Is.EqualTo(Guid.Empty));
            }
        }

        [Test]
        public void Constructor_NullFamilyAndPatronymic_Allowed()
        {
            // act
            var author = new Author(familyName: null, "Иван");

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(author.FamilyName, Is.Null);
                Assert.That(author.Patronymic, Is.Null);
            }
        }

        [Test]
        public void Constructor_NullName_Throws()
        {
            // act & assert
            Assert.Throws<ArgumentException>(() => new Author("Фамилия", null!, "Отчество"));
        }

        [TestCaseSource(nameof(WhitespaceFamilyNameCases))]
        public void FamilyName_WhitespaceOnly_Throws(string value)
        {
            // arrange
            var author = new Author("Фамилия", "Имя");

            // act & assert
            Assert.Throws<ArgumentException>(() => author.FamilyName = value);
        }

        [Test]
        public void FamilyName_Null_Allowed()
        {
            // arrange & act
            var author = new Author(familyName: null, "Имя");

            // assert
            Assert.That(author.FamilyName, Is.Null);
        }

        [TestCaseSource(nameof(InvalidDateBirthCases))]
        public void DateBirth_GreaterOrEqualDateDeath_Throws(DateOnly death, DateOnly birth)
        {
            // arrange
            var author = new Author("Фамилия", "Имя") { DateDeath = death };

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => author.DateBirth = birth);
        }

        [TestCaseSource(nameof(InvalidDateDeathCases))]
        public void DateDeath_LessOrEqualDateBirth_Throws(DateOnly birth, DateOnly death)
        {
            // arrange
            var author = new Author("Фамилия", "Имя") { DateBirth = birth };

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => author.DateDeath = death);
        }

        [Test]
        public void DateBirth_BeforeDateDeath_Ok()
        {
            // arrange & act
            var author = new Author("Фамилия", "Имя")
            {
                DateDeath = new DateOnly(2000, 1, 1),
                DateBirth = new DateOnly(1900, 1, 1),
            };

            // assert
            Assert.That(author.DateBirth, Is.EqualTo(new DateOnly(1900, 1, 1)));
        }

        [Test]
        public void DateDeath_AfterDateBirth_Ok()
        {
            // arrange & act
            var author = new Author("Фамилия", "Имя")
            {
                DateBirth = new DateOnly(1900, 1, 1),
                DateDeath = new DateOnly(2000, 1, 1),
            };

            // assert
            Assert.That(author.DateDeath, Is.EqualTo(new DateOnly(2000, 1, 1)));
        }

        [Test]
        public void DateBirth_SetNull_Allowed()
        {
            // arrange
            var author = new Author("Фамилия", "Имя") { DateBirth = new DateOnly(2000, 1, 1) };

            // act
            author.DateBirth = null;

            // assert
            Assert.That(author.DateBirth, Is.Null);
        }

        [Test]
        public void DateDeath_SetNull_Allowed()
        {
            // arrange
            var author = new Author("Фамилия", "Имя") { DateDeath = new DateOnly(2000, 1, 1) };

            // act
            author.DateDeath = null;

            // assert
            Assert.That(author.DateDeath, Is.Null);
        }

        [Test]
        public void AddBook_Null_ReturnsFalse()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");

            // act
            var added = author.AddBook(null!);

            // assert
            Assert.That(added, Is.False);
        }

        [Test]
        public void AddBook_ValidData_Success()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");
            var book = CreateBook(author);

            // act & assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(author.Books, Contains.Item(book));
                Assert.That(book.Authors, Contains.Item(author));
            }
        }

        [Test]
        public void AddBook_SecondTime_ReturnsFalse()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");
            var book = CreateBook(author);

            // act
            var added = author.AddBook(book);

            // assert
            Assert.That(added, Is.False);
        }

        [Test]
        public void RemoveBook_Null_ReturnsFalse()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");

            // act
            var removed = author.RemoveBook(null!);

            // assert
            Assert.That(removed, Is.False);
        }

        [Test]
        public void RemoveBook_ValidData_Success()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");
            var book = CreateBook(author);

            // act
            var removed = author.RemoveBook(book);

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(removed, Is.True);
                Assert.That(author.Books, Is.Empty);
                Assert.That(book.Authors, Does.Not.Contain(author));
            }
        }

        [Test]
        public void RemoveBook_NotPresent_ReturnsFalse()
        {
            // arrange
            var author = new Author("Фамилия", "Имя");
            var otherAuthor = new Author("Другой", "Автор");
            var book = CreateBook(otherAuthor);

            // act
            var removed = author.RemoveBook(book);

            // assert
            Assert.That(removed, Is.False);
        }

        [Test]
        public void ToString_OnlyName()
        {
            // arrange & act
            var author = new Author(familyName: null, "Иван");

            // assert
            Assert.That(author.ToString(), Is.EqualTo("Иван"));
        }

        [Test]
        public void ToString_Full_ContainsAllParts()
        {
            // arrange
            var author = new Author("Иванов", "Иван", "Иванович")
            {
                DateBirth = new DateOnly(1900, 5, 15),
                DateDeath = new DateOnly(1980, 3, 20),
            };

            // act
            var result = author.ToString();

            // assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result, Does.Contain("Иван"));
                Assert.That(result, Does.Contain("Иванович"));
                Assert.That(result, Does.Contain("Иванов"));
                Assert.That(result, Does.Contain("15.05.1900"));
                Assert.That(result, Does.Contain("20.03.1980"));
            }
        }

        [Test]
        public void Equals_SameFields_ReturnsTrue()
        {
            // arrange
            var author1 = new Author("Иванов", "Иван", "Иванович") { DateBirth = new DateOnly(1900, 1, 1) };
            var author2 = new Author("Иванов", "Иван", "Иванович") { DateBirth = new DateOnly(1900, 1, 1) };

            // act & assert
            Assert.That(author1, Is.EqualTo(author2));
        }

        [Test]
        public void Equals_DifferentFamilyName_ReturnsFalse()
        {
            // arrange
            var author1 = new Author("Иванов", "Иван", "Иванович");
            var author2 = new Author("Петров", "Иван", "Иванович");

            // act & assert
            Assert.That(author1, Is.Not.EqualTo(author2));
        }

        [Test]
        public void Equals_CaseSensitive()
        {
            // arrange
            var author1 = new Author("Иванов", "Иван");
            var author2 = new Author("ИВАНОВ", "Иван");

            // act & assert
            Assert.That(author1, Is.Not.EqualTo(author2));
        }

        [Test]
        public void Equals_DifferentDateDeath_ReturnsFalse()
        {
            // arrange
            var author1 = new Author("Фамилия", "Имя") { DateDeath = new DateOnly(2000, 1, 1) };
            var author2 = new Author("Фамилия", "Имя") { DateDeath = new DateOnly(2001, 1, 1) };

            // act & assert
            Assert.That(author1, Is.Not.EqualTo(author2));
        }

        [Test]
        public void GetHashCode_SameFields_Equal()
        {
            // arrange
            var author1 = new Author("Иванов", "Иван", "Иванович") { DateBirth = new DateOnly(1900, 1, 1) };
            var author2 = new Author("Иванов", "Иван", "Иванович") { DateBirth = new DateOnly(1900, 1, 1) };

            // act
            var hash1 = author1.GetHashCode();
            var hash2 = author2.GetHashCode();

            // assert
            Assert.That(hash1, Is.EqualTo(hash2));
        }

        private static Book CreateBook(Author author)
        {
            var publisher = new Publisher("Publisher");
            return new Book("Book", 100, isbn: null, 2000, publisher, author);
        }

        private static IEnumerable<TestCaseData> WhitespaceFamilyNameCases()
        {
            yield return new TestCaseData("   ").SetArgDisplayNames("Spaces");
            yield return new TestCaseData("\t").SetArgDisplayNames("Tab");
            yield return new TestCaseData("\n").SetArgDisplayNames("NewLine");
        }

        private static IEnumerable<TestCaseData> InvalidDateBirthCases()
        {
            yield return new TestCaseData(
                new DateOnly(2000, 1, 1),
                new DateOnly(2000, 1, 1))
                .SetArgDisplayNames("Death: 01.01.2000", "Birth: 01.01.2000");
            yield return new TestCaseData(
                new DateOnly(2000, 1, 1),
                new DateOnly(2001, 1, 1))
                .SetArgDisplayNames("Death: 01.01.2000", "Birth: 01.01.2001");
        }

        private static IEnumerable<TestCaseData> InvalidDateDeathCases()
        {
            yield return new TestCaseData(
                new DateOnly(2000, 1, 1),
                new DateOnly(2000, 1, 1))
                .SetArgDisplayNames("Birth: 01.01.2000", "Death: 01.01.2000");
            yield return new TestCaseData(
                new DateOnly(2000, 1, 1),
                new DateOnly(1999, 1, 1))
                .SetArgDisplayNames("Birth: 01.01.2000", "Death: 01.01.1999");
        }
    }
}

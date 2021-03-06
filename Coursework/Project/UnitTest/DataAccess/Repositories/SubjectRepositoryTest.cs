using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DataAccess.Entities;
using DataAccess.Repositories;
using DA = DataAccess.Context;
using System.Data.Entity.Infrastructure;

namespace UnitTest.DataAccess.Repositories
{
    [TestClass]
    public class SubjectRepositoryTest
    {
        // FIELDS
        static DA.AppContext dbContext;
        static Resources.Classes.DbFiller dbFiller;
        // PROPERTIES
        public TestContext TestContext { get; set; }
        // INITIALIZERS
        [ClassInitialize]
        public static void Constructor(TestContext context)
        {
            dbFiller = Resources.Classes.DbFiller.Instance;

            dbContext = Resources.Initializers.DatabaseInitializer.DBContext;
        }
        [TestInitialize]
        public void Filler()
        {
            dbFiller.Fill(dbContext);
        }
        [TestCleanup]
        public void Cleaner()
        {
            dbFiller.Purge(dbContext);
        }

        // TEST
        // COUNT
        #region COUNT
        [TestMethod]
        public void Count()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            int expectedSubjectInDb = dbContext.Subjects.Count(); 

            // Act
            int actualSubjectInDb = subjectRepository.Count();

            // Assert
            Assert.AreEqual(expectedSubjectInDb, actualSubjectInDb);
        }
        [TestMethod]
        public void CountIf4Messages()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            int numberOfMessagesperSubject = 4;
            int expectedSubjectWithMessagesInDb = Resources.Classes.DbFiller.Instance.SubjectMessageAmount
                .Count(s => s.Value == numberOfMessagesperSubject);

            // Act
            int actualSubjectWith4Messages = subjectRepository
                .Count(subject => subject.Messages.Count == numberOfMessagesperSubject);

            // Assert
            Assert.AreEqual(expectedSubjectWithMessagesInDb, actualSubjectWith4Messages);
        }
        #endregion
        // GET
        #region GET
        [TestMethod]
        public void GetAll()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            int expectedSubjectInDb = Resources.Classes.DbFiller.Instance.SubjectAmount;

            // Act
            Subject[] subjectFromDB = subjectRepository.Get().ToArray();
            int actualSubjectInDb = subjectFromDB.Length;

            // Assert
            Assert.AreEqual(expectedSubjectInDb, actualSubjectInDb);
            CollectionAssert.AreEquivalent(dbContext.Subjects.ToArray(), subjectFromDB);
        }
        [TestMethod]
        public void GetFilterByMessageAmount()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            int expectedSubjectInDb = 2;

            // Act
            Subject[] subjectFromDb = subjectRepository.Get(filter: subject => subject.Messages.Count > 2).ToArray();
            int actualSubjectInDb = subjectFromDb.Length;

            // Assert
            Assert.AreEqual(expectedSubjectInDb, actualSubjectInDb);
            CollectionAssert.IsSubsetOf(subjectFromDb, dbContext.Subjects.ToArray());
        }
        [TestMethod]
        public void GetOrder()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            int expectedSubjectInDb = Resources.Classes.DbFiller.Instance.SubjectAmount;

            // Act
            Subject[] subjectFromDb = subjectRepository.Get(orderBy: subject => subject.OrderBy(s => s.Name)).ToArray();
            int actualUserInDb = subjectFromDb.Length;

            // Assert
            Assert.AreEqual(expectedSubjectInDb, actualUserInDb);
            CollectionAssert.AreEqual(dbContext.Subjects.OrderBy(s => s.Name).ToArray(), subjectFromDb);
        }
        [TestMethod]
        public void GetFilterAndOrder()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            int expectedSubjectInDB = Resources.Classes.DbFiller.Instance.SubjectAmount;
            Subject[] valuesInDataBase = dbContext.Subjects.Where(s => s.Messages.Count > 0).OrderByDescending(s => s.Name).ToArray();

            // Act
            Subject[] subjectFromDb = subjectRepository.Get(filter: subject => subject.Messages.Count > 0, orderBy: o => o.OrderByDescending(s => s.Name)).ToArray();
            int actualSubjectFromDb = subjectFromDb.Length;

            // Assert
            Assert.AreEqual(expectedSubjectInDB, actualSubjectFromDb);
            CollectionAssert.AreEqual(valuesInDataBase, subjectFromDb);
        }
        #endregion
        // GET BY ID
        #region GET BY ID
        [TestMethod]
        public void GetById()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Guid idToSearch = dbContext.Subjects.First().Id;
            Subject expectedSubject = dbContext.Subjects.Find(idToSearch);

            // Act
            Subject subjectFromDB = subjectRepository.Get(idToSearch);

            // Assert
            Assert.AreEqual(expectedSubject, subjectFromDB);
        }
        [TestMethod]
        public void GetByWrongId_Null()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Guid wrongId = default(Guid);
            Subject expectedSubjectFromDb = null;

            // Act
            Subject actualSubjectFromDb = subjectRepository.Get(wrongId);

            // Assert
            Assert.AreEqual(expectedSubjectFromDb, actualSubjectFromDb);
        }
        #endregion
        // INSERT
        #region INSERT
        [TestMethod]
        [DataSource(
            providerInvariantName: "Microsoft.VisualStudio.TestTools.DataSource.XML",
            connectionString: @"..\..\Resources\DataAccess\Repositories\RegularSubject.xml",
            tableName: "Subject",
            dataAccessMethod: DataAccessMethod.Random)]
        public void AddRegularSubject()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Subject subject = new Subject { Name = Convert.ToString(TestContext.DataRow["Name"]) };

            // Act
            subjectRepository.Insert(subject);
            dbContext.SaveChanges();

            // Assert
            CollectionAssert.Contains(dbContext.Subjects.ToList(), subject);
        }
        [TestMethod]
        [DataSource(
            providerInvariantName: "Microsoft.VisualStudio.TestTools.DataSource.XML",
            connectionString: @"..\..\Resources\DataAccess\Repositories\WrongSubject.xml",
            tableName: "Subject",
            dataAccessMethod: DataAccessMethod.Random)]
        public void AddSubjectWithWrongNameLength()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Subject subject = new Subject { Name = Convert.ToString(TestContext.DataRow["Name"]) };

            // Act
            subjectRepository.Insert(subject);

            // Assert
            Assert.ThrowsException<System.Data.Entity.Validation.DbEntityValidationException>(() => dbContext.SaveChanges());
            // undo adding
            ((IObjectContextAdapter)dbContext).ObjectContext.Detach(subject);
        }
        #endregion
        // DELETE BY KEY
        #region DELETE BY KEY
        [TestMethod]
        public void DeleteByKey()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Subject expectedDeletedSubject = dbContext.Subjects.First(s => s.Name == "Subject 1");
            Guid idToDelete = expectedDeletedSubject.Id;

            // Act
            // This subject has messages that must have "subject: null" when one will be deleted.
            subjectRepository.Delete(idToDelete);
            dbContext.SaveChanges();

            // Assert
            CollectionAssert.DoesNotContain(dbContext.Subjects.ToArray(), expectedDeletedSubject);
            // Checks if all subject's messages are null.
            Assert.IsTrue(dbContext.Messages.AsEnumerable().Any(m => m.Subject == null || m.Subject.Id != expectedDeletedSubject.Id));
        }
        [TestMethod]
        public void DeleteByWrongKey_Exception()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Guid wrongId = default(Guid);

            // Act
            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => subjectRepository.Delete(wrongId));
        }
        [TestMethod]
        public void DeleteByNullKey_Exception()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            object wrongId = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => subjectRepository.Delete(wrongId));
        }
        #endregion
        // DELETE BY VALUE
        #region DELETE BY VALUE
        [TestMethod]
        public void DeleteByValue()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Subject subjectToDelete = dbContext.Subjects.First(s => s.Name == "Subject 1");

            // Act
            // This subject has messages that must have "subject: null" when one will be deleted.
            subjectRepository.Delete(subjectToDelete);
            dbContext.SaveChanges();

            // Assert
            CollectionAssert.DoesNotContain(dbContext.Subjects.ToArray(), subjectToDelete);
            // Checks if all subject's messages are null.
            Assert.IsTrue(dbContext.Messages.AsEnumerable().Any(m => m.Subject == null || m.Subject.Id != subjectToDelete.Id));
        }
        [TestMethod]
        public void DeleteByNullValue()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => subjectRepository.Delete(entityToDelete: null));
        }
        [TestMethod]
        public void DeleteByChangedValue()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Subject changedSubjectToDelete = dbContext.Subjects.First(s => s.Name == "Subject 1");
            changedSubjectToDelete.Name += "Changed it";

            // Act
            subjectRepository.Delete(entityToDelete: changedSubjectToDelete);
            dbContext.SaveChanges();

            // Assert
            CollectionAssert.DoesNotContain(dbContext.Subjects.ToArray(), changedSubjectToDelete);
            // Checks if all subject's messages are null.
            Assert.IsTrue(dbContext.Messages.AsEnumerable().Any(m => m.Subject == null || m.Subject.Id != changedSubjectToDelete.Id));
        }
        #endregion
        // UPDATE
        #region UPDATE
        [TestMethod]
        public void Update()
        {
            // Arrange
            SubjectRepository subjectRepository = new SubjectRepository(dbContext);
            Subject subjectToUpdate = dbContext.Subjects.First(s => s.Name == "Subject 1");
            string newSubjectName = "New Subject Name";

            // Act
            subjectToUpdate.Name = newSubjectName;
            subjectRepository.Update(subjectToUpdate);
            dbContext.SaveChanges();

            // Assert
            Assert.AreEqual(dbContext.Subjects.Find(subjectToUpdate.Id).Name, newSubjectName);
        }
        #endregion
    }
}

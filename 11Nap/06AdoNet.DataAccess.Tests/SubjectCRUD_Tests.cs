﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _06AdoNet.DataAccess.Tests
{
    [TestClass]
    public class SubjectCRUD_Tests
    {
        private readonly string connectionString = "Server=.\\SQLEXPRESS;Database=CodeFirstDB;Trusted_Connection=True;";

        [TestMethod]
        public void TeacherReadId1_ShouldNotNull()
        {

            //AAA: Act, Arrange, Assert: http://wiki.c2.com/?ArrangeActAssert

            //Act: előkészítés
            var dal = new DataAccessLayer(connectionString);

            //Arrange: feladat elvégzése
            var teacher = dal.TeacherRead(1);

            //Assert: eredmény ellenőrzése
            Assert.IsNotNull(teacher);
            Assert.AreEqual("Matektanár", teacher.TeacherName);
            Assert.AreEqual(1, teacher.Id);
        }

        [TestMethod]
        public void TeacherReadId0_ShouldNull()
        {

            //Act: előkészítés
            var dal = new DataAccessLayer(connectionString);

            //Arrange: feladat elvégzése
            var teacher = dal.TeacherRead(0);

            //Assert: eredmény ellenőrzése
            Assert.IsNull(teacher);
        }

        [TestMethod]
        public void TeacherCreate()
        {
            //Act
            var dal = new DataAccessLayer(connectionString);
            var teacherToCreate = new Teacher() { TeacherName = "Magyar Nyelv és Irodalom" };

            //Arrange
            var id = dal.TeacherCreate(teacherToCreate);

            //Assert
            //mivel az adatbázis identity 1-ről indul, 0-val jelezhetem a hibát a Create függvényből
            //vagyis, ha 0-val jön vissza, akkor nem sikerült a felvitel
            Assert.AreNotEqual(0, id);

            //második körös ellenőrzés, ha nagyon biztosak akarunk lenni
            var createdTeacher = dal.TeacherRead(id);
            Assert.IsNotNull(createdTeacher);
            Assert.AreEqual("Magyar Nyelv és Irodalom", createdTeacher.TeacherName);
            Assert.AreEqual(id, createdTeacher.Id);

            //A teszt ismételhetővé tétele (nagyon fontos!)
            //visszaállítjuk a teszt előtti állapotot
            var affected = dal.TeacherDelete(id);
            Assert.AreEqual(1, affected);

        }

        [TestMethod]
        public void TeacherDelete()
        {
            //Act
            var dal = new DataAccessLayer(connectionString);
            //A teszt ismételhetővé tétele (nagyon fontos!)
            //előre rögzítjük a törlendő rekordot
            var teacherToDelete = new Teacher() { TeacherName = "Magyar Nyelv és Irodalom" };
            var id = dal.TeacherCreate(teacherToDelete);
            Assert.AreNotEqual(0, id);

            //második körös ellenőrzés, ha nagyon biztosak akarunk lenni
            var createdTeacher = dal.TeacherRead(id);
            Assert.IsNotNull(createdTeacher);
            Assert.AreEqual("Magyar Nyelv és Irodalom", createdTeacher.TeacherName);
            Assert.AreEqual(id, createdTeacher.Id);

            //Arrange
            int affectedRows = dal.TeacherDelete(id);

            //Assert
            Assert.AreEqual(1, affectedRows);

            //A teszt ismételhetővé tétele (nagyon fontos!)
            //visszaállítjuk a teszt előtti állapotot

            //ha utólag visszük fel a rekordot, akkor
            //a probléma, amit meg kell oldani, hogy az azonosítót nem tudjuk csak úgy megadni
            //ahhoz ki kell kapcsolni az Identity szabályt az SQL szerveren
            //ezért nem itt oldjuk meg, hanem a teszt előtt

        }

    }
}


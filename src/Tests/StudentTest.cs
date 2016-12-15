using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockProject.Controllers;
using MockProject.Data;
using MockProject.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class StudentTest
    {
        [Fact]
        public async void ShouldCreateStudent()
        {
            var contextMock = new Mock<ApplicationDbContext>();
            var setMock = new Mock<DbSet<Student>>();

            contextMock.Setup(x => x.Student).Returns(setMock.Object);

            var controller = new StudentsController(contextMock.Object);
            var student = new Student() { Id = 1, Name = "Stefan" };
            var result = await controller.Create(student);

            setMock.Verify(x => x.Add(It.IsAny<Student>()), Times.Once());
            contextMock.Verify(x => x.SaveChangesAsync(new System.Threading.CancellationToken()), Times.Once());

            setMock.Verify(m => m.Add(It.Is<Student>(g => g.Name == "Stefan" && g.Id == 1)));


        }

        private Mock<DbSet<Student>> SetupStudentSet(List<Student> dummy)
        {
            var dummyData = dummy.AsQueryable();
            var mockDbSetStudent = new Mock<DbSet<Student>>();

            //alle property van IQueryable correct toekennen
            mockDbSetStudent.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(dummyData.Provider);
            mockDbSetStudent.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(dummyData.Expression);
            mockDbSetStudent.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(dummyData.ElementType);
            mockDbSetStudent.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(dummyData.GetEnumerator());
            
            return mockDbSetStudent;
        }

        [Fact]
        public void IndexAllStudentsInStudentsController()
        {
            var mockDbContext = new Mock<ApplicationDbContext>();            
            var dummyData = new List<Student>() { new Student() { Name = "Jan" } };

            var mockDbSetStudent = SetupStudentSet(dummyData);
            mockDbContext.Setup(x => x.Student).Returns(mockDbSetStudent.Object);

            StudentsController c = new StudentsController(mockDbContext.Object);
            var result = c.IndexAllStudents();

            //result moet een view zijn
            var viewResult = Assert.IsType<ViewResult>(result);

            //check model data
            var model = (IQueryable<Student>)viewResult.Model;
            int aantal = model.Count();

            //checkt het aantal
            Assert.Equal(1, aantal);
            //checkt de naam van het 1e element 
            Assert.Equal("Jan", model.ElementAt(0).Name);
        }

        [Fact]
        public void ShouldReturnShortStudents()
        {
            var dummyData = new List<Student>() { new Student() {
                Name = "Jan", Height = 1.8 },
                new Student() { Name = "Anne", Height = 1.7 } };
            var mockDbContext = new Mock<ApplicationDbContext>();
            
            var mockDbSetStudent = SetupStudentSet(dummyData);
            mockDbContext.Setup(x => x.Student).Returns(mockDbSetStudent.Object);

            StudentsController c = new StudentsController(mockDbContext.Object);
            var result = c.IndexStudentShorterThan(1.75);

            //result moet een view zijn
            var viewResult = Assert.IsType<ViewResult>(result);

            //check model data
            var model = (IList<Student>)viewResult.Model;
            Assert.Equal(1, model.Count());
            Assert.Equal("Anne", model.ElementAt(0).Name);

        }
    }
}

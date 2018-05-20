using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Controllers;
using WebApi.Models;

namespace WebApiTests.ControllersTests
{
	[TestClass]
	public class IncidentsControllerTests
	{
		/// <summary>
		/// Получить тестовые данные.
		/// </summary>
		/// <returns></returns>
		private static List<Incident> GetTestData()
		{
			return new List<Incident>
			{
				new Incident
				{
					Id = 1,
					ClientId = 1,
					Theme = "Жалоба",
					Date = DateTime.Now
				},
				new Incident
				{
					Id = 2,
					ClientId = 2,
					Theme = "Заявление",
					Date = DateTime.Now
				},
				new Incident
				{
					Id = 3,
					ClientId = 3,
					Theme = "Жалоба",
					Date = DateTime.Now
				},
				new Incident
				{
					Id = 4,
					ClientId = 3,
					Theme = "Заявление",
					Date = DateTime.Now
				}
			};
		}

		/// <summary>
		/// Тестируемый метод GetIncidents.
		/// Должен вернуть OkNegotiatedContentResult содержащий инциденты клиента.
		/// </summary>
		[TestMethod]
		public void GetAllIncidents()
		{
			// Arrange.
			var testData = GetTestData();
			var testClientId = 3;

			var mockDbSet = new Mock<DbSet<Incident>>();
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.Provider)
				.Returns(testData.AsQueryable().Provider);
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.Expression)
				.Returns(testData.AsQueryable().Expression);
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.GetEnumerator())
				.Returns(() => testData.AsQueryable().GetEnumerator());

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Incidents)
				.Returns(mockDbSet.Object);

			var incidentsController = new IncidentsController(mockDbContext.Object);

			// Act.
			var actionResult = incidentsController.GetIncidents(testClientId) as OkNegotiatedContentResult<IQueryable<Incident>>;

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsNotNull(actionResult.Content);
			Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<IQueryable<Incident>>));
			Assert.AreEqual(testData.Select(e => e.ClientId = testClientId).Count(), actionResult.Content.ToList().Count);
		}

		/// <summary>
		/// Тестируемый метод GetIncident.
		/// Должен вернуть OkNegotiatedContentResult содержащий инцидент.
		/// </summary>
		[TestMethod]
		public void GetIncidentById()
		{
			// Arrange.
			var testData = GetTestData();
			var testEntity = testData.FirstOrDefault(e => e.Id == 1);

			var mockDbSet = new Mock<DbSet<Incident>>();
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);
			mockDbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(ids => testData.FirstOrDefault(d => d.Id == (int) ids[0]));

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Incidents)
				.Returns(mockDbSet.Object);

			var incidentsController = new IncidentsController(mockDbContext.Object);

			// Act.
			var actionResult = incidentsController.GetIncident(testEntity.Id) as OkNegotiatedContentResult<Incident>;

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsNotNull(actionResult.Content);
			Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Incident>));
			Assert.AreEqual(testEntity, actionResult.Content);
		}

		/// <summary>
		/// Тестируемый метод CreateIncident, добавляет инцидент.
		/// Должен вернуть OkResult.
		/// </summary>
		[TestMethod]
		public void CreateIncident()
		{
			// Arrange.
			var testData = GetTestData();
			var testEntity = new Incident
			{
				Id = 5,
				ClientId = 3,
				Theme = "Заявление",
				Date = DateTime.Now
			};

			var mockDbSet = new Mock<DbSet<Incident>>();
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);
			mockDbSet
				.Setup(m => m.Add(It.IsAny<Incident>()))
				.Callback<Incident>(entity => testData.Add(entity));

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Incidents)
				.Returns(mockDbSet.Object);

			var incidentsController = new IncidentsController(mockDbContext.Object);

			// Act.
			var actionResult = incidentsController.CreateIncident(testEntity) as OkResult;
			var actEntity = testData.Find(e => e.Id == testEntity.Id);

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsInstanceOfType(actionResult, typeof(OkResult));
			Assert.AreEqual(testEntity, actEntity);
		}

		/// <summary>
		/// Тестируемый метод EditIncident, обновляет инцидент.
		/// Должен вернуть OkResult.
		/// </summary>
		[TestMethod]
		public void EditIncident()
		{
			// Arrange.
			var testData = GetTestData();
			var testEntity = new Incident
			{
				Id = 4,
				ClientId = 1,
				Theme = "Жалоба",
				Date = DateTime.Now
			};

			var mockDbSet = new Mock<DbSet<Incident>>();
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator());

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Incidents)
				.Returns(mockDbSet.Object);
			mockDbContext
				.Setup(m => m.Entry(It.IsAny<Incident>()))
				.Callback(() =>
				{
					testData.ForEach(e =>
					{
						if (e.Id == testEntity.Id)
						{
							e.ClientId = testEntity.ClientId;
							e.Date = testEntity.Date;
							e.Text = testEntity.Text;
							e.Theme = testEntity.Theme;
						}
					});
				});

			var incidentsController = new IncidentsController(mockDbContext.Object);

			// Act.
			var actionResult = incidentsController.EditIncident(testEntity.Id, testEntity) as OkResult;
			var actEntity = testData.Find(e => e.Id == testEntity.Id);

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsInstanceOfType(actionResult, typeof(OkResult));
			Assert.AreEqual(testEntity.Id, actEntity.Id);
			Assert.AreEqual(testEntity.ClientId, actEntity.ClientId);
			Assert.AreEqual(testEntity.Date, actEntity.Date);
			Assert.AreEqual(testEntity.Text, actEntity.Text);
			Assert.AreEqual(testEntity.Theme, actEntity.Theme);
		}

		/// <summary>
		/// Тестируемый метод DeleteIncident, удаляет инцидент.
		/// Должен вернуть OkResult.
		/// </summary>
		[TestMethod]
		public void DeleteIncident()
		{
			// Arrange.
			var testData = GetTestData();
			var testId = 2;

			var mockDbSet = new Mock<DbSet<Incident>>();
			mockDbSet
				.As<IQueryable<Incident>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator());
			mockDbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(ids => testData.FirstOrDefault(d => d.Id == (int) ids[0]));
			mockDbSet
				.Setup(m => m.Remove(It.IsAny<Incident>()))
				.Callback<Incident>(entity => testData.Remove(entity));

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Incidents)
				.Returns(mockDbSet.Object);

			var incidentsController = new IncidentsController(mockDbContext.Object);

			// Act.
			var actionResult = incidentsController.DeleteIncident(testId) as OkResult;

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsInstanceOfType(actionResult, typeof(OkResult));
		}
	}
}
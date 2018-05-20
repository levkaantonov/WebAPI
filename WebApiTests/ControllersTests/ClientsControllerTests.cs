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
	public class ClientsControllerTests
	{
		/// <summary>
		/// Получить тестовые данные.
		/// </summary>
		/// <returns></returns>
		private static List<Client> GetTestData()
		{
			return new List<Client>
			{
				new Client
				{
					Id = 1,
					FullName = "Артемьева Марина Георгиевна",
					PassportId = "45№43560683",
					Mail = "er1bf@moldtelecom.md",
					PhoneNumber = "2 341 321352",
					Type = "Заемщик"
				},
				new Client
				{
					Id = 2,
					FullName = "Пономарёв Протасий Германович",
					PassportId = "45119",
					Mail = "wil@blues.minsk.by",
					PhoneNumber = "243252",
					Type = "Заемщик"
				},
				new Client
				{
					Id = 3,
					FullName = "Савельева Марина Протасьевна",
					PassportId = "47387",
					Mail = "ly1bzb@ly1xx.ampr.org",
					PhoneNumber = "42342",
					Type = "Вкладчик"
				}
			};
		}

		/// <summary>
		/// Тестируемый метод GetClients.
		/// Должен вернуть OkNegotiatedContentResult содержащий клиентов.
		/// </summary>
		[TestMethod]
		public void GetAllClients()
		{
			// Arrange.
			var testData = GetTestData();

			var mockDbSet = new Mock<DbSet<Client>>();
			mockDbSet
				.As<IQueryable<Client>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Clients)
				.Returns(mockDbSet.Object);

			var clientsController = new ClientsController(mockDbContext.Object);

			// Act.
			var actionResult = clientsController.GetClients() as OkNegotiatedContentResult<DbSet<Client>>;

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsNotNull(actionResult.Content);
			Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<DbSet<Client>>));
			Assert.AreEqual(testData.Count, actionResult.Content.ToList().Count);
		}

		/// <summary>
		/// Тестируемый метод GetClient.
		/// Должен вернуть OkNegotiatedContentResult содержащий клиента.
		/// </summary>
		[TestMethod]
		public void GetClientById()
		{
			// Arrange.
			var testData = GetTestData();

			var testEntity = testData.FirstOrDefault(e => e.Id == 2);

			var mockDbSet = new Mock<DbSet<Client>>();
			mockDbSet
				.As<IQueryable<Client>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);
			mockDbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(ids => testData.FirstOrDefault(d => d.Id == (int) ids[0]));

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Clients)
				.Returns(mockDbSet.Object);

			var clientsController = new ClientsController(mockDbContext.Object);

			// Act.
			var actionResult = clientsController.GetClient(testEntity.Id) as OkNegotiatedContentResult<Client>;

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsNotNull(actionResult.Content);
			Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Client>));
			Assert.AreEqual(testEntity, actionResult.Content);
		}

		/// <summary>
		/// Тестируемый метод CreateClient, добавляет клиента.
		/// Должен вернуть OkResult.
		/// </summary>
		[TestMethod]
		public void CreateClient()
		{
			// Arrange.
			var testData = GetTestData();

			var testEntity = new Client
			{
				Id = 4,
				FullName = "Герман Абрам Иосифович",
				PassportId = "85468",
				Mail = "lenf@blues.by",
				PhoneNumber = "645397897",
				Type = "Возможный клиент"
			};

			var mockDbSet = new Mock<DbSet<Client>>();
			mockDbSet
				.As<IQueryable<Client>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);
			mockDbSet
				.Setup(m => m.Add(It.IsAny<Client>()))
				.Callback<Client>(entity => testData.Add(entity));

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Clients)
				.Returns(mockDbSet.Object);

			var clientsController = new ClientsController(mockDbContext.Object);

			// Act.
			var actionResult = clientsController.CreateClient(testEntity) as OkResult;
			var actEntity = testData.Find(e => e.Id == testEntity.Id);

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsInstanceOfType(actionResult, typeof(OkResult));
			Assert.AreEqual(testEntity, actEntity);
		}

		/// <summary>
		/// Тестируемый метод EditClient, обновляет клиента.
		/// Должен вернуть OkResult.
		/// </summary>
		[TestMethod]
		public void EditClient()
		{
			// Arrange.
			var testData = GetTestData();
			var testEntity = new Client
			{
				Id = 3,
				FullName = "Герман Абрам Иосифович",
				PassportId = "85468",
				Mail = "lenf@blues.by",
				PhoneNumber = "645397897",
				Type = "Возможный клиент"
			};

			var mockDbSet = new Mock<DbSet<Client>>();
			mockDbSet
				.As<IQueryable<Client>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Clients)
				.Returns(mockDbSet.Object);
			mockDbContext
				.Setup(m => m.Entry(It.IsAny<Client>()))
				.Callback(() =>
				{
					testData.ForEach(e =>
					{
						if (e.Id == testEntity.Id)
						{
							e.FullName = testEntity.FullName;
							e.Mail = testEntity.Mail;
							e.PassportId = testEntity.PassportId;
							e.PhoneNumber = testEntity.PhoneNumber;
							e.Type = testEntity.Type;
						}
					});
				});

			var clientsController = new ClientsController(mockDbContext.Object);

			// Act.
			var actionResult = clientsController.EditClient(testEntity.Id, testEntity) as OkResult;
			var actEntity = testData.Find(e => e.Id == testEntity.Id);

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsInstanceOfType(actionResult, typeof(OkResult));
			Assert.AreEqual(testEntity.Id, actEntity.Id);
			Assert.AreEqual(testEntity.FullName, actEntity.FullName);
			Assert.AreEqual(testEntity.Mail, actEntity.Mail);
			Assert.AreEqual(testEntity.PassportId, actEntity.PassportId);
			Assert.AreEqual(testEntity.PhoneNumber, actEntity.PhoneNumber);
			Assert.AreEqual(testEntity.Type, actEntity.Type);
		}

		/// <summary>
		/// Тестируемый метод DeleteClient, удаляет клиента.
		/// Должен вернуть OkResult.
		/// </summary>
		[TestMethod]
		public void DeleteClient()
		{
			// Arrange.
			var testData = GetTestData();
			var testId = 2;

			var mockDbSet = new Mock<DbSet<Client>>();
			mockDbSet
				.As<IQueryable<Client>>()
				.Setup(m => m.GetEnumerator())
				.Returns(testData.AsQueryable().GetEnumerator);
			mockDbSet
				.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(ids => testData.FirstOrDefault(d => d.Id == (int) ids[0]));
			mockDbSet
				.Setup(m => m.Remove(It.IsAny<Client>()))
				.Callback<Client>(entity => testData.Remove(entity));

			var mockDbContext = new Mock<BankDbContext>();
			mockDbContext
				.Setup(m => m.Clients)
				.Returns(mockDbSet.Object);

			var clientsController = new ClientsController(mockDbContext.Object);

			// Act.
			var actionResult = clientsController.DeleteClient(testId) as OkResult;

			// Assert.
			Assert.IsNotNull(actionResult);
			Assert.IsInstanceOfType(actionResult, typeof(OkResult));
		}
	}
}
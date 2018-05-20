using System;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models;


namespace WebApi.Controllers
{
	/// <summary>
	/// Контроллер клиентов.
	/// </summary>
	[EnableCors("*", "*", "*")]
	public class ClientsController : ApiController
	{
		/// <summary>
		/// Контекст БД.
		/// </summary>
		private readonly BankDbContext _db;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public ClientsController() : this(new BankDbContext())
		{
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		public ClientsController(BankDbContext db)
		{
			if (db == null)
			{
				throw new ArgumentNullException(nameof(db));
			}

			_db = db;
		}

		/// <summary>
		/// Получить всех клиентов.
		/// GET: api/clients
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult GetClients()
		{
			return Ok(_db.Clients);
		}

		/// <summary>
		/// Получить клиента.
		/// GET: api/clients/3
		/// </summary>
		/// <param name="id">Id клиента.</param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult GetClient(int id)
		{
			var client = _db.Clients.Find(id);
			if (client == null)
			{
				return NotFound();
			}
			return Ok(client);
		}

		/// <summary>
		/// Создать клиента.
		/// POST: api/clients
		/// </summary>
		/// <param name="client">Клиент.</param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult CreateClient([FromBody] Client client)
		{
			if (client == null)
			{
				return BadRequest();
			}

			_db.Clients.Add(client);
			_db.SaveChanges();
			return Ok();
		}

		/// <summary>
		/// Обновить клиента.
		/// PUT: api/clients/3
		/// </summary>
		/// <param name="id">Id клиента.</param>
		/// <param name="client">Клиент.</param>
		/// <returns></returns>
		[HttpPut]
		public IHttpActionResult EditClient(int id, [FromBody] Client client)
		{
			if (client == null)
			{
				return BadRequest();
			}

			if (id != client.Id)
			{
				return BadRequest();
			}

			_db.Entry(client);
			_db.SaveChanges();
			return Ok();
		}

		/// <summary>
		/// Удалить клиента.
		/// DELETE: api/clients/3
		/// </summary>
		/// <param name="id">Id клиента.</param>
		/// <returns></returns>
		[HttpDelete]
		public IHttpActionResult DeleteClient(int id)
		{
			var client = _db.Clients.Find(id);
			if (client == null)
			{
				return BadRequest();
			}
			_db.Clients.Remove(client);
			_db.SaveChanges();
			return Ok();
		}

		protected override void Dispose(bool disposing)
		{
			_db.Dispose();
			base.Dispose(disposing);
		}
	}
}
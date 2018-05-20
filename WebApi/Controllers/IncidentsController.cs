using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models;

namespace WebApi.Controllers
{
	/// <summary>
	/// Контроллер инцидентов.
	/// </summary>
	[EnableCors("*", "*", "*")]
	public class IncidentsController : ApiController
	{
		/// <summary>
		/// Контекст БД.
		/// </summary>
		private readonly BankDbContext _db;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public IncidentsController() : this(new BankDbContext())
		{
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		public IncidentsController(BankDbContext db)
		{
			if (db == null)
			{
				throw new ArgumentNullException(nameof(db));
			}

			_db = db;
		}

		/// <summary>
		/// Получить все инциденты клиента.
		/// GET: api/incidents/3/all
		/// </summary>
		/// <param name="clientId">Id клиента.</param>
		/// <returns></returns>
		[Route("api/incidents/{clientId:int}/all")]
		[HttpGet]
		public IHttpActionResult GetIncidents(int clientId)
		{
			return Ok(_db.Incidents.Where(incident => incident.ClientId == clientId));
		}

		/// <summary>
		/// Получить инцидент.
		/// GET: api/incidents/3
		/// </summary>
		/// <param name="id">Id инцидента.</param>
		/// <returns></returns>
		[HttpGet]
		public IHttpActionResult GetIncident(int id)
		{
			var incident = _db.Incidents.Find(id);
			if (incident == null)
			{
				return NotFound();
			}

			return Ok(incident);
		}

		/// <summary>
		/// Создать инцидент.
		/// POST: api/incidents
		/// </summary>
		/// <param name="incident"></param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult CreateIncident([FromBody] Incident incident)
		{
			if (incident == null)
			{
				return BadRequest();
			}

			_db.Incidents.Add(incident);
			_db.SaveChanges();

			return Ok();
		}

		/// <summary>
		/// Обновить инцидент.
		/// PUT: api/incidents/3
		/// </summary>
		/// <param name="id">Id инцидента.</param>
		/// <param name="incident">Инцидент.</param>
		/// <returns></returns>
		[HttpPut]
		public IHttpActionResult EditIncident(int id, [FromBody] Incident incident)
		{
			if (incident == null)
			{
				return BadRequest();
			}

			if (id != incident.Id)
			{
				return BadRequest();
			}

			_db.Entry(incident);
			_db.SaveChanges();

			return Ok();
		}

		/// <summary>
		/// Удалить инцидент.
		/// DELETE: api/incidents/3
		/// </summary>
		/// <param name="id">Id инцидента.</param>
		/// <returns></returns>
		[HttpDelete]
		public IHttpActionResult DeleteIncident(int id)
		{
			var incident = _db.Incidents.Find(id);
			if (incident == null)
			{
				return BadRequest();
			}

			_db.Incidents.Remove(incident);
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
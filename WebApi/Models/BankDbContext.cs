using System.Data.Entity;

namespace WebApi.Models
{
	/// <summary>
	/// Контекст данных.
	/// </summary>
	public class BankDbContext : DbContext
	{
		/// <summary>
		/// Конструктор.
		/// </summary>
		public BankDbContext() : base("name=BankDBConnection")
		{
		}

		/// <summary>
		/// Клиенты.
		/// </summary>
		public virtual DbSet<Client> Clients { get; set; }

		/// <summary>
		/// Инциденты.
		/// </summary>
		public virtual DbSet<Incident> Incidents { get; set; }

		/// <summary>
		/// Прикрепляет данные к контексту и поечает как модифицированные.
		/// Нужен для тестов.
		/// </summary>
		/// <param name="entity"></param>
		public new virtual void Entry(object entity)
		{
			base.Entry(entity).State = EntityState.Modified;
		}
	}
}
using System;
using System.Data.Entity;

namespace WebApi.Models
{
	/// <summary>
	/// Инициализатор БД.
	/// </summary>
	public class BankDbContextInitializer : DropCreateDatabaseAlways<BankDbContext>
	{

		/// <summary>
		/// Инициализация БД.
		/// </summary>
		/// <param name="db">Контекст данных.</param>
		protected override void Seed(BankDbContext db)
		{
			// Клиенты.
			db.Clients.Add(new Client
			{
				Id = 1,
				FullName = "Артемьева Марина Георгиевна",
				PassportId = "45№43560683",
				Mail = "er1bf@moldtelecom.md",
				PhoneNumber = "2 341 321352",
				Type = "Заемщик"
			});
			db.Clients.Add(new Client
			{
				Id = 2,
				FullName = "Пономарёв Протасий Германович",
				PassportId = "45119",
				Mail = "wil@blues.minsk.by",
				PhoneNumber = "243252",
				Type = "Заемщик"
			});
			db.Clients.Add(new Client
			{
				Id = 3,
				FullName = "Савельева Марина Протасьевна",
				PassportId = "47387",
				Mail = "ly1bzb@ly1xx.ampr.org",
				PhoneNumber = "42342",
				Type = "Вкладчик"
			});
			db.Clients.Add(new Client
			{
				Id = 4,
				FullName = "Григорьева Вера Макаровна",
				PassportId = "46817",
				Mail = "ly1bzb@uy0ll.ampr.org",
				PhoneNumber = "234324234",
				Type = "Вкладчик"
			});
			db.Clients.Add(new Client
			{
				Id = 5,
				FullName = "Муравьёва Оксана Руслановна",
				PassportId = "45061",
				Mail = "ly1bzb@ly3od.ampr.org",
				PhoneNumber = "4234234",
				Type = "Заемщик"
			});
			db.Clients.Add(new Client
			{
				Id = 6,
				FullName = "Гурьев Станислав Ильяович",
				PassportId = "45161",
				Mail = "ly1bzb@pub.osf.lt",
				PhoneNumber = "32423423",
				Type = "Заемщик"
			});
			db.Clients.Add(new Client
			{
				Id = 7,
				FullName = "Муравьёв Василий Семёнович",
				PassportId = "44909",
				Mail = "ly1ff@kb0tdf.ampr.org",
				PhoneNumber = "234234",
				Type = "Вкладчик"
			});

			// Инциденты.
			db.Incidents.Add(new Incident
			{
				Id = 1,
				ClientId = 1,
				Theme = "Жалоба",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 2,
				ClientId = 2,
				Theme = "Заявление",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 3,
				ClientId = 3,
				Theme = "Жалоба",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 4,
				ClientId = 4,
				Theme = "Заявление",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 5,
				ClientId = 5,
				Theme = "Жалоба",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 6,
				ClientId = 6,
				Theme = "Заявление",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 7,
				ClientId = 7,
				Theme = "Жалоба",
				Date = DateTime.Now
			});
			db.Incidents.Add(new Incident
			{
				Id = 8,
				ClientId = 7,
				Theme = "Заявление",
				Date = DateTime.Now
			});

			base.Seed(db);
		}
	}
}
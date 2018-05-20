using System;
using System.Runtime.Serialization;

namespace WebApi.Models
{
	/// <summary>
	/// Класс инцидента.
	/// </summary>
	[KnownType(typeof(Incident))]
	[DataContract]
	public class Incident
	{
		/// <summary>
		/// Id инцидента.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Ид клиента.
		/// </summary>
		[DataMember]
		public int ClientId { get; set; }

		/// <summary>
		/// Тема инцидента.
		/// </summary>
		[DataMember]
		public string Theme { get; set; }

		/// <summary>
		/// Содержание.
		/// </summary>
		[DataMember]
		public string Text { get; set; }

		/// <summary>
		/// Дата регистрации.
		/// </summary>
		[DataMember]
		public DateTime Date { get; set; }
	}
}
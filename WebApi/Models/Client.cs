using System.Runtime.Serialization;

namespace WebApi.Models
{
	/// <summary>
	/// Класс клиента.
	/// </summary>
	[KnownType(typeof(Client))]
	[DataContract]
	public class Client
	{
		/// <summary>
		/// Id клиента.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// ФИО.
		/// </summary>
		[DataMember]
		public string FullName { get; set; }

		/// <summary>
		/// Номер паспорта.
		/// </summary>
		[DataMember]
		public string PassportId { get; set; }

		/// <summary>
		/// Почта.
		/// </summary>
		[DataMember]
		public string Mail { get; set; }

		/// <summary>
		/// Номер телефона.
		/// </summary>
		[DataMember]
		public string PhoneNumber { get; set; }

		/// <summary>
		/// Тип клиента.
		/// </summary>
		[DataMember]
		public string Type { get; set; }
	}
}
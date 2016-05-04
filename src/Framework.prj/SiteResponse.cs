using System;
using System.Net;

namespace RujanService.Framework
{
	/// <summary>Предоставляет ответ сайта.</summary>
	public class SiteResponse
	{
		#region Properties
		
		/// <summary>Возвращает содержимое страницы сайта.</summary>
		public string ResponseContent { get; }

		/// <summary>Возвращает код состояния ответа.</summary>
		public int ResponseStatus { get; }

		#endregion

		#region .ctor

		/// <summary>Создаёт и инициализирует ответ сайта.</summary>
		/// <param name="pageContent">Содержимое страницы.</param>
		/// <param name="statusCode">Код состояния ответа.</param>
		public SiteResponse(string pageContent, HttpStatusCode statusCode)
		{
			ResponseContent = pageContent;
			ResponseStatus = (int)statusCode;
		}

		/// <summary>Создаёт и инициализирует ответ сайта.</summary>
		/// <param name="statusCode">Код состояния ответа.</param>
		public SiteResponse(HttpStatusCode statusCode)
		{
			ResponseStatus = (int)statusCode;
		}

		#endregion
	}
}

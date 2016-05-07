using System;
using System.Linq;
using System.Net;

namespace RujanService.Framework
{
	/// <summary>Предоставляет ответ сайта.</summary>
	public class SiteResponse
	{
		#region Properties

		/// <summary>Возвращает содержимое страницы сайта.</summary>		
		/// <value>Содержимое страницы сайта.</value>
		public string ResponseContent { get; }

		/// <summary>Возвращает код состояния ответа.</summary>
		/// <value>Код состояния ответа.</value>
		public int ResponseStatus { get; }

		/// <summary>Возвращает mime-тип ответа сайта.</summary>
		/// <value>Mime-тип ответа сайта.</value>
		public string MimeType { get; }

		#endregion

		#region .ctor

		/// <summary>Создаёт и инициализирует ответ сайта.</summary>
		/// <param name="pageContent">Содержимое страницы.</param>
		/// <param name="statusCode">Код состояния ответа.</param>
		/// <param name="mimeType">Mime-тип ответа сайта.</param>
		public SiteResponse(string pageContent, HttpStatusCode statusCode, string mimeType)
		{
			ResponseContent = pageContent;
			MimeType = mimeType;
			ResponseStatus = (int)statusCode;
		}

		/// <summary>Создаёт и инициализирует ответ сайта.</summary>
		/// <param name="pageContent">Содержимое страницы.</param>
		/// <param name="statusCode">Код состояния ответа.</param>
		public SiteResponse(string pageContent, HttpStatusCode statusCode)
		{
			ResponseContent = pageContent;
			MimeType = Utilities.MimeTypes.Where(x => x.Key == ".html").FirstOrDefault().Value;
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

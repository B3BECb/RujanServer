using System;
using System.Threading;

namespace RujanService.Framework
{
	/// <summary>Описывает сайт.</summary>
	public interface ISite : IDisposable
	{
		/// <summary>Возвращает наименование сайта.</summary>
		string Name { get; }

		/// <summary>Возвращает описание сайта.</summary>
		string Description { get; }

		/// <summary>Возвращает адрес сайта на сервере.</summary>
		string Address { get; }

		/// <summary>Получает ответ сайта на запрос <see cref="HttpListenerResponse"/>.</summary>
		/// <param name="token">Токен отмены.</param>
		/// <param name="request">Запрос.</param>
		/// <param name="query">Параметры запроса.</param>
		/// <returns>Код состояния ответа.<para><seealso cref="HttpStatusCode"/></para>.</returns>
		SiteResponse GetResponse(CancellationToken token, string request, string query);
	}
}

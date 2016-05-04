using System.Net;
using System.Threading;

using RujanService.Framework;

namespace Localhost
{
	/// <summary>Предоставляет сайт с текстом.</summary>
	public class Main : ISite
	{
		#region Implenented ISite properties 

		public string Address => "/localhost/";

		public string Description => "Localhost site";

		public string Name => "Localhost";

		#endregion

		#region .ctor

		/// <summary>Инициализирует и создаёт <see cref="Main"/>.</summary>
		public Main()
		{

		}

		#endregion

		#region Implemented ISite public methods

		public SiteResponse GetResponse(CancellationToken token, string request, string query)
		{
			var page = Properties.Resources.index;
			return new SiteResponse(page, HttpStatusCode.OK);
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{

		}

		#endregion
	}
}

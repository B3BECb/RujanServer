using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

using RujanService.Framework;

namespace Text
{
	/// <summary>Предоставляет сайт с текстом.</summary>
	public class Main : ISite
	{
		#region Implenented ISite properties 

		public string Address => "/text/";

		public string Description => "Wall of text";

		public string Name => "Text";

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
			return new SiteResponse(HttpStatusCode.BadRequest);
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{

		}

		#endregion
	}
}

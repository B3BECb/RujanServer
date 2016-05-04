using System.Net;
using System.Threading;

using RujanService.Framework;

namespace Images
{
	/// <summary>Предоставляет сайт с изображениями.</summary>
	public class Main : ISite
	{
		#region Implenented ISite properties

		public string Address => "/Images/";

		public string Description => "Images site";

		public string Name => "Images";

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

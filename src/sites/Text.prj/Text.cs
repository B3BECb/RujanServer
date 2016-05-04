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
			var response = new StringBuilder();
			var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			response.Append("<html><head><meta charset=\"utf-8\"></head><body>");
			response.Append("<pre style=\"word - wrap: break-word; white - space: pre - wrap; \">");
			response.Append(File.ReadAllText(path + "sites/text/text/textFile.txt", Encoding.UTF8));
			response.Append("</pre></body></html>");
			return new SiteResponse(response.ToString(), HttpStatusCode.OK);
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{

		}

		#endregion
	}
}

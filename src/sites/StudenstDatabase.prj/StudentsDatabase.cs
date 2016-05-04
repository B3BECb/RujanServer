using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using RujanService.Framework;

namespace StudentsDatabase
{
	/// <summary>Предоставляет сайт с текстом.</summary>
	public class Main : ISite
	{
		#region Implenented ISite properties 

		public string Address => "/StudentsDatabase/";

		public string Description => "Students database site";

		public string Name => "StudentsDatabase";

		private List<string> _students;

		#endregion

		#region .ctor

		/// <summary>Инициализирует и создаёт <see cref="Main"/>.</summary>
		public Main()
		{
			_students = new List<string>()
			{
				"Николаев Иван Григорьевич",
				"Петров Алексей Владимирович",
				"Новиков Дмитрий Дмитриевич"
			};
		}

		#endregion

		#region Implemented ISite public methods

		public SiteResponse GetResponse(CancellationToken token, string request, string query)
		{
			IReadOnlyList<string> selectedStudents;
			string students = "";

			if(query.Length > 0)
			{
				if(query.Split('=').LastOrDefault() == "all")
					selectedStudents = GetStudents(true);
				else
					selectedStudents = GetStudents(false);

				foreach(var student in selectedStudents)
				{
					students += $"<p>{student}</p>";
				}

				return new SiteResponse(students, HttpStatusCode.OK);
			}

			var page = Properties.Resources.index;
			return new SiteResponse(page, HttpStatusCode.OK);
		}

		#endregion

		#region Private methods

		private IReadOnlyList<string> GetStudents(bool getAll, string fio = "")
		{
			if(getAll) return _students;

			return _students.Where(stud => stud == fio).ToList();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{

		}

		#endregion
	}
}

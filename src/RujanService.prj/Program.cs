using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RujanService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			if(args.Contains("--c"))
			{
				Console.WriteLine("press 'q' to exit");
				Console.WriteLine("press 's' to setup");

				try
				{
					var settings = new ServerSettings();
					settings.Load();

					using(var server = new RujanHttpServer(settings))
					{
						server.StartAsync();

						bool isRunning = true;

						while(isRunning)
						{
							switch(Console.ReadKey().KeyChar)
							{
								case 'q':
									server.Stop();
									isRunning = false;
									break;

								case 's':
									Console.WriteLine();

									server.Stop();

									ServerSettings.Configure(settings);

									settings.Save();

									server.StartAsync();
									break;
							}
						}
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine("Fatal!");
					Console.WriteLine(ex);
					Console.ReadKey();
				}
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new ServiceRunner()
				};
				ServiceBase.Run(ServicesToRun);
			}
		}
	}
}

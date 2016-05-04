using System;
using System.IO;
using System.Xml.Serialization;

using RujanService.Framework;

namespace RujanService
{
	/// <summary>Предоставляет настройки сервера <see cref="RujanHttpServer"/>.</summary>
	[Serializable]
	public sealed class ServerSettings
	{
		const string SETTINGS_PATH = @"C:\ProgramData\RujanServer\";

		/// <summary>Возвращает и задаёт адрес <see cref="RujanHttpServer"/>.</summary>
		/// <value>Адрес сервера.</value>
		public string Address { get; set; }

		/// <summary>Возвращает и задаёт порт <see cref="RujanHttpServer"/>.</summary>
		/// <value>Порт сервера.</value>
		public int Port { get; set; }

		/// <summary>Инициализирует <see cref="ServerSettings"/> сервера <see cref="RujanHttpServer"/> с настройками поумолчанию.</summary>
		public ServerSettings()
		{
			Address = "http://localhost";
			Port = 4242;
		}
		/// <summary>Инициализирует <see cref="ServerSettings"/> сервера <see cref="RujanHttpServer"/> с заданными настройками.</summary>
		/// <param name="address">Адрес сервера.</param>
		/// <param name="port">Порт сервера.</param>
		public ServerSettings(string address, int port)
		{
			Address = address;
			Port = port;
		}

		/// <summary>Загружает настройки <see cref="RujanHttpServer"/>.</summary>
		public void Load()
		{
			var xml = new XmlSerializer(typeof(ServerSettings));

			Utilities.CreateDirectory(SETTINGS_PATH);

			using(Stream fStream = new FileStream(SETTINGS_PATH + "settings.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
			{
				try
				{
					var loadedSettings = xml.Deserialize(fStream) as ServerSettings;
					
					this.Address = loadedSettings.Address;
					this.Port = loadedSettings.Port;
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		/// <summary>Сохраняет настройки <see cref="RujanHttpServer"/>.</summary>
		public void Save()
		{
			var xml = new XmlSerializer(typeof(ServerSettings));

			Utilities.CreateDirectory(SETTINGS_PATH);

			using(Stream fStream = new FileStream(SETTINGS_PATH + "settings.xml", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				xml.Serialize(fStream, this);
			}
		}

		/// <summary>Выполняет конфигурацию настроек сервера.</summary>
		/// <param name="settings">Настройки сервера.</param>
		public static void Configure(ServerSettings settings)
		{
			Console.WriteLine($"Current address: {settings.Address}");
			Console.WriteLine($"Current port: {settings.Port}");

			Console.WriteLine("Press enter if you don't want change specified parametr.");

			Console.Write("Address: ");
			var parametr = Console.ReadLine();
			if(parametr != "") settings.Address = parametr;

			Console.Write("Port: ");
			parametr = Console.ReadLine();
			int port;
			if(parametr != "") settings.Port = int.TryParse(parametr, out port) ? port : settings.Port;
		}
	}
}
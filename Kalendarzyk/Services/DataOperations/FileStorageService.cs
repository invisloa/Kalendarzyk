using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Services.DataOperations
{

	public class FileStorageService : IFileStorageService
	{
		public async Task<string> ReadFileAsync(string filePath)
		{
			try
			{
				if (!FileExists(filePath))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(filePath));
				}

				using var streamReader = new StreamReader(filePath, Encoding.UTF8);
				return await streamReader.ReadToEndAsync();
			}
			catch
			(Exception ex)	// no file exists probably first run
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task WriteFileAsync(string filePath, string content)
		{
			try
			{
			var directory = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

			using var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8);
			await streamWriter.WriteAsync(content);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public bool FileExists(string filePath)
		{
			return File.Exists(filePath);
		}

	}

}
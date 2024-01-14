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
			if (!FileExists(filePath))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			}

			using var streamReader = new StreamReader(filePath, Encoding.UTF8);
			return await streamReader.ReadToEndAsync();
		}

		public async Task WriteFileAsync(string filePath, string content)
		{
			var directory = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			using var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8);
			await streamWriter.WriteAsync(content);
		}

		public bool FileExists(string filePath)
		{
			return File.Exists(filePath);
		}

	}

}
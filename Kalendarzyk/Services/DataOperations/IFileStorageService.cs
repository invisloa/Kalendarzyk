
namespace Kalendarzyk.Services.DataOperations
{
	public interface IFileStorageService
	{
		Task<string> ReadFileAsync(string filePath);
		Task WriteFileAsync(string filePath, string content);
	}
}

namespace Kalendarzyk.Services.DataOperations
{
	public interface IFileSelectorService
	{
		Task<string> AsyncSelectFile();
	}
}
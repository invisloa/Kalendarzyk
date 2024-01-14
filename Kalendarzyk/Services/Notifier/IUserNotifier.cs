
namespace Kalendarzyk.Services.Notifier
{
	public interface IUserNotifier
	{
		Task<string> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons);
		Task ShowMessageAsync(string message, CancellationToken cancellationToken);
	}
}
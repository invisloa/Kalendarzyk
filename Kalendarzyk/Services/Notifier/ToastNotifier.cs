using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Services.Notifier
{
	public class ToastNotifier : IUserNotifier
	{
		public async Task ShowMessageAsync(string message, CancellationToken cancellationToken)
		{
			await Toast.Make(message).Show(cancellationToken);
		}

		public async Task<string> ShowActionSheetAsync(string title, string cancel, string destruction, params string[] buttons)
		{
			return await App.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
		}

	}
}

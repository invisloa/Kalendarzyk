using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Services.DataOperations
{
	public class FileSelectorService : IFileSelectorService
	{
		public async Task<string> AsyncSelectFile()
		{
			var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
			{
				{ DevicePlatform.WinUI, new[] { ".json" } },
				{ DevicePlatform.Android, new[] { "application/json" } },
				{ DevicePlatform.iOS, new[] { ".json" } }
			});
			var options = new PickOptions
			{
				FileTypes = customFileType,
				PickerTitle = "Select file to open"
			};

			var result = await FilePicker.PickAsync(options);
			if (result != null)
			{
				return result.FullPath;
			}
			else
			{
				return null;
			}
		}
	}
}

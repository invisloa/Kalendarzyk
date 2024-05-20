using Kalendarzyk.ViewModels;

namespace Kalendarzyk.Models.EventModels
{
	public class MicroTask
	{
		public string Title { get; set; }
		public bool IsCompleted { get; set; }

		public MicroTask(string title, bool isCompleted = false)
		{
			Title = title;
			IsCompleted = isCompleted;
		}
	}
}

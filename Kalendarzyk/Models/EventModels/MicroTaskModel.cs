using Kalendarzyk.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Models.EventModels
{
	public class MicroTaskModel : BaseViewModel
	{
		private string _title;
		private bool _isCompleted;

		public string MicroTaskTitle
		{
			get => _title;
			set
			{
				if (_title == value)
				{
					return;
				}
				_title = value;
				OnPropertyChanged();
			}
		}
		public bool IsMicroTaskCompleted
		{
			get => _isCompleted;
			set
			{
				if (_isCompleted == value)
				{
					return;
				}
				_isCompleted = value;
				OnPropertyChanged();
			}
		}
		public MicroTaskModel(string title, bool isCompleted = false)
		{
			MicroTaskTitle = title;
			IsMicroTaskCompleted = isCompleted;
		}

	}
}

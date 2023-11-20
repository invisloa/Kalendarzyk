using Kalendarzyk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kalendarzyk.Models.EventTypesModels
{
	public class MainEventType : IMainEventType
	{
		public string Title { get; set; }
		public IMainTypeVisualModel SelectedVisualElement { get; set; }  // new property for the icon
		public override string ToString()
		{
			return Title;
		}
		public MainEventType(string title, IMainTypeVisualModel icon)
		{
			Title = title;
			SelectedVisualElement = icon;

		}
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			MainEventType other = (MainEventType)obj;
			return Title == other.Title && SelectedVisualElement.Equals(other.SelectedVisualElement);
		}

		public override int GetHashCode()
		{
			return (Title, SelectedVisualElement).GetHashCode();
		}
	}
}

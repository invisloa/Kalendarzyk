namespace Kalendarzyk.Views.CustomControls.CCViewModels
{
	using global::Kalendarzyk.Views.CustomControls.CCInterfaces;
	using System;
	using System.Collections.ObjectModel;

	namespace Kalendarzyk.Views.CustomControls.CCHelperClass
	{
		public class FilterDatesCCViewModel : IFilterDatesCC, IFilterDatesCCHelperClass
		{
			// there is no SEARCHBOX here because it is not worth to add all logic for it
			private DateTime _filterDateFrom;
			private DateTime _filterDateTo;

			// Event for FilterDateFrom property changed
			public event Action FilterDateFromChanged;

			// Event for FilterDateTo property changed
			public event Action FilterDateToChanged;

			public string TextFilterDateFrom { get; set; } = "FILTER FROM:";

			public string TextFilterDateTo { get; set; } = "FILTER UP TO:";

			public DateTime FilterDateFrom
			{
				get => _filterDateFrom;
				set
				{
					if (_filterDateFrom != value)
					{
						_filterDateFrom = value;
						// Fire the event after value is set
						FilterDateFromChanged?.Invoke();
					}
				}
			}

			public DateTime FilterDateTo
			{
				get => _filterDateTo;
				set
				{
					if (_filterDateTo != value)
					{
						_filterDateTo = value;
						// Fire the event after value is set
						FilterDateToChanged?.Invoke();
					}
				}
			}

			public ObservableCollection<Models.EventModels.IGeneralEventModel> AllEventsListOC { get; set; }
		}
	}

}

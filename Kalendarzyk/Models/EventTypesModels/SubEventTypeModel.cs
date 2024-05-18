using Kalendarzyk.Models.EventModels;
using Kalendarzyk.ViewModels;
using System.Runtime.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Kalendarzyk.Models.EventTypesModels
{
	public class SubEventTypeModel : BaseViewModel, ISubEventTypeModel
	{
		public IMainEventType MainEventType { get; set; }
		public string EventTypeName { get; set; }
		private Color _eventTypeColor;
		private TimeSpan _defaultEventTime;
		private bool _isSelectedToFilter;

		private bool _isValueType;
		private bool _isMicroTaskType;
		private IEnumerable<MicroTaskModel> _microTasksList;
		private QuantityModel _quantityAmount;
		public IEnumerable<MicroTaskModel> MicroTasksList
		{
			get => _microTasksList;
			set
			{
				if (_microTasksList != value)
				{
					_microTasksList = value;
					OnPropertyChanged();
				}
			}
		}
		// Store color as string due to serialization issues
		public string EventTypeColorString
		{
			get
			{
				return _eventTypeColor.ToArgbHex();
			}
			set
			{
				_eventTypeColor = Color.FromArgb(value);
			}
		}
		public TimeSpan DefaultEventTimeSpan
		{
			get
			{
				return _defaultEventTime;
			}
			set
			{
				if (_defaultEventTime != value)
				{
					_defaultEventTime = value;
				}
			}
		}

		[JsonIgnore]
		public Color EventTypeColor
		{
			get
			{
				return _eventTypeColor;
			}
			set
			{
				if (_eventTypeColor != value)
				{
					_eventTypeColor = value;
					OnPropertyChanged();
				}
			}
		}

		private Color _backgroundColor;

		// BackgroundColor is added to the model to store the color that is currently shown (isCompleted color adjustment) - consider changing it to a converter (low priority)
		[JsonIgnore]
		public Color BackgroundColor
		{
			get => _backgroundColor;
			set
			{
				if (_backgroundColor != value)
				{
					_backgroundColor = value;
					OnPropertyChanged();
				}
			}
		}
		public bool IsValueType
		{
			get => _isValueType;
			set
			{
				if (_isValueType != value)
				{
					_isValueType = value;
					OnPropertyChanged();
				}
			}
		}
		public bool IsMicroTaskType
		{
			get => _isMicroTaskType;
			set
			{
				if (_isMicroTaskType != value)
				{
					_isMicroTaskType = value;
					OnPropertyChanged();
				}
			}
		}
		public bool IsSelectedToFilter
		{
			get => _isSelectedToFilter;
			set
			{
				if (_isSelectedToFilter != value)
				{
					_isSelectedToFilter = value;
					OnPropertyChanged();
				}
			}
		}
		public QuantityModel QuantityAmount
		{
			get => _quantityAmount;
			set
			{
				if (_quantityAmount != value)
				{
					_quantityAmount = value;
					OnPropertyChanged();
				}
			}
		}

		public SubEventTypeModel(IMainEventType mainEventType, string eventTypeName, Color eventTypeColor, TimeSpan defaultEventTime, QuantityModel quantity = null, IEnumerable<MicroTaskModel> microTasksList = null, bool isSelectedToFilter = true)
		{
			MainEventType = mainEventType;
			IsSelectedToFilter = isSelectedToFilter;
			DefaultEventTimeSpan = defaultEventTime;
			EventTypeName = eventTypeName;
			EventTypeColor = eventTypeColor;
			BackgroundColor = eventTypeColor; // Initialize BackgroundColor as EventTypeColor upon object creation
			if (quantity != null)
			{
				QuantityAmount = quantity;
				IsValueType = true;
			}
			if (microTasksList != null)
			{
				MicroTasksList = microTasksList;
				IsMicroTaskType = true;
			}
		}
		public bool Equals(ISubEventTypeModel obj)
		{
			// Check if the passed object is null
			if (obj == null)
			{
				return false;
			}

			// Attempt to cast the passed object to ISubEventTypeModel
			if (obj is not ISubEventTypeModel other)
			{
				return false;
			}

			// Compare MainEventType
			if (!MainEventType.Equals(other.MainEventType))
			{
				return false;
			}

			// Compare EventTypeColorString
			if (EventTypeColorString != other.EventTypeColorString)
			{
				return false;
			}

			// Compare EventTypeName
			if (EventTypeName != other.EventTypeName)
			{
				return false;
			}

			// If all comparisons passed, the objects are equal
			return true;
		}


		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + MainEventType.GetHashCode();
				hash = hash * 23 + (EventTypeName?.GetHashCode() ?? 0);
				hash = hash * 23 + (EventTypeColorString?.GetHashCode() ?? 0);
				return hash;
			}
		}

		public override string ToString()
		{
			return EventTypeName;
		}
		//This method will be called after deserialization and will ensure that BackgroundColor is initialized to the value of EventTypeColor
		[OnDeserialized]
		private void OnDeserializedMethod(StreamingContext context)
		{
			BackgroundColor = EventTypeColor;
		}

	}
}

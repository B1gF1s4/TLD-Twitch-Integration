using Il2Cpp;
using UnityEngine;

namespace TLD_Twitch_Integration.Gui
{
	public class DisplayValue
	{
		public string Name { get; set; }

		public string Title { get; set; }

		public string ValueID { get; set; }

		public bool IsSeparator { get; set; }

		private const int LABEL_WIDTH = 60;

		private readonly UILabel? _titleLabel;
		private readonly UILabel? _valueLabel;

		public DisplayValue(string name)
		{
			Name = name;
			Title = name;
			ValueID = name;

			IsSeparator = true;
		}

		public DisplayValue(Panel_HUD hud, string name, string title, string valueID)
		{
			Name = name;
			Title = title;
			ValueID = valueID;

			_titleLabel = CreateTitleLabel(hud, Name);
			_valueLabel = CreateValueLabel(hud, Name);

			Disable();

			IsSeparator = false;
		}

		public void UpdatePosition(Panel_HUD hud, int index)
		{
			var transform = hud.m_VistaNotification.transform;

			if (_titleLabel != null)
				_titleLabel.transform.localPosition = new Vector3(
					transform.localPosition.x,
					transform.localPosition.y - index * 36 + 8 + Display.CurrentDisplayValuesOffset,
					transform.localPosition.z);

			if (_valueLabel != null)
				_valueLabel.transform.localPosition = new Vector3(
					transform.localPosition.x,
					transform.localPosition.y - index * 36 + Display.CurrentDisplayValuesOffset,
					transform.localPosition.z);
		}

		public void Update(Panel_HUD hud)
		{
			if (hud.m_VistaNotification == null)
				return;

			var parent = hud.m_VistaNotification;

			if (_titleLabel != null)
			{
				_titleLabel.enabled = parent.isVisible;
				_titleLabel.text = Title;
			}

			if (_valueLabel != null)
			{
				_valueLabel.enabled = parent.isVisible;
				_valueLabel.text = Display.GetCurrentValue(ValueID);
			}
		}

		public void Disable()
		{
			if (_titleLabel != null)
				_titleLabel.enabled = false;

			if (_valueLabel != null)
				_valueLabel.enabled = false;
		}

		public float GetYLocation()
		{
			if (_titleLabel != null)
				return _titleLabel.transform.position.y;

			return 0;
		}

		private static UILabel CreateTitleLabel(Panel_HUD hud, string name)
		{
			var parentTransform = hud.m_VistaNotification.transform;
			var label = UnityEngine.Object.Instantiate(
				hud.m_VistaNotificationTitle);

			label.transform.SetParent(parentTransform.parent, false);
			label.transform.localScale = new Vector3(0.8f, 0.8f, 1);
			label.name = "Title_" + name;
			label.width = LABEL_WIDTH;
			label.alignment = NGUIText.Alignment.Center;

			return label;
		}

		private static UILabel CreateValueLabel(Panel_HUD hud, string name)
		{
			var parentTransform = hud.m_VistaNotification.transform;
			var label = UnityEngine.Object.Instantiate(
				hud.m_VistaNotificationDescription);

			label.transform.SetParent(parentTransform.parent, false);
			label.transform.localScale = new Vector3(0.8f, 0.8f, 1);
			label.name = "Value_" + name;
			label.width = LABEL_WIDTH;
			label.alignment = NGUIText.Alignment.Center;

			return label;
		}
	}
}

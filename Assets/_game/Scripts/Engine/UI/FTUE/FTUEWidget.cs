using UnityEngine;
using Sirenix.OdinInspector;

namespace RomenoCompany
{
	public class FTUEWidget : Widget
	{
		[SerializeField, FoldoutGroup("References")]
		private FTUEHighlight _ftueHighlight = null;

		[SerializeField, FoldoutGroup("References")]
		private FTUEHint _ftueHint = null;

		[SerializeField, FoldoutGroup("References")]
		private FTUETooltip _ftueTooltip = null;


		public override void InitializeWidget()
		{
			widgetType = WidgetType.FTUE;

			base.InitializeWidget();

			_ftueHint.Init();
			_ftueHighlight.Init();
			_ftueTooltip.Init();
		}

		public void PresentFTUE(GameObject highlightedGo, FTUEType ftueType)
		{
			var ftueTemplate = DB.Instance.ftueSettings.GetFTUE(ftueType);
			if (ftueTemplate == null)
			{
				Debug.LogError($"FTUEWidget: ftueType {ftueType} not found");
				return;
			}
			
			if (ftueTemplate.highlightObject)
				_ftueHighlight.BeginHighlight(highlightedGo, ftueTemplate.highlightSettings);

			if (ftueTemplate.showHint) _ftueHint.ShowHint(highlightedGo.transform as RectTransform, ftueTemplate.hintSettings);
			else _ftueHint.HideHint();

			if (ftueTemplate.showTooltip) _ftueTooltip.ShowToolTip(highlightedGo.transform as RectTransform, ftueTemplate.tooltipSettings);
			else _ftueTooltip.HideTooltip();
		}

		public void WithdrawFTUE()
		{
			_ftueHighlight.EndHighlight();
			_ftueHint.HideHint();
			_ftueTooltip.HideTooltip();
		}
	}
}

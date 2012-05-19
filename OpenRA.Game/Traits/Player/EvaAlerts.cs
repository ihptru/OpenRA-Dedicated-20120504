#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

namespace OpenRA.Traits
{
	public class EvaAlertsInfo : TraitInfo<EvaAlerts>
	{
		// Sound effects
		public readonly string RadarUp = "radaron2.aud";
		public readonly string RadarDown = "radardn1.aud";

		public readonly string CashTickUp = "cashup1.aud";
		public readonly string CashTickDown = "cashdn1.aud";

		// Build Palette
		public readonly string BuildingCannotPlaceAudio = "nodeply1.aud";
		public readonly string NewOptions = "newopt1.aud";

		// For manual powerup/down in ra-ng
		public readonly string DisablePower = "bleep11.aud";
		public readonly string EnablePower = "bleep12.aud";

		// Eva speech
		public readonly string Repairing = "repair1.aud";
		public readonly string LowPower = "lopower1.aud";
		public readonly string SilosNeeded = "silond1.aud";
		public readonly string PrimaryBuildingSelected = "pribldg1.aud";

		// Special powers
		public readonly string AbilityInsufficientPower = "nopowr1.aud";

		public readonly string LevelUp = "hydrod1.aud";
	}

	public class EvaAlerts {}
}

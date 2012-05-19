#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System.Collections.Generic;
using System.Drawing;
using OpenRA.Mods.RA.Activities;
using OpenRA.Mods.RA.Buildings;
using OpenRA.Mods.RA.Move;
using OpenRA.Mods.RA.Orders;
using OpenRA.Traits;

namespace OpenRA.Mods.RA
{
	class C4DemolitionInfo : ITraitInfo
	{
		public readonly int C4Delay = 45; // 1.8 seconds
		public object Create(ActorInitializer init) { return new C4Demolition(this); }
	}

	class C4Demolition : IIssueOrder, IResolveOrder, IOrderVoice
	{
		readonly C4DemolitionInfo Info;

		public C4Demolition(C4DemolitionInfo info)
		{
			Info = info;
		}

		public IEnumerable<IOrderTargeter> Orders
		{
			get { yield return new UnitTraitOrderTargeter<Building>( "C4", 6, "c4", true, false ); }
		}

		public Order IssueOrder( Actor self, IOrderTargeter order, Target target, bool queued )
		{
			if( order.OrderID == "C4" )
				return new Order("C4", self, queued) { TargetActor = target.Actor };

			return null;
		}

		public void ResolveOrder(Actor self, Order order)
		{
			if (order.OrderString == "C4")
			{
				self.SetTargetLine(Target.FromOrder(order), Color.Red);

				var mobile = self.Trait<Mobile>();
				self.CancelActivity();
				self.QueueActivity(new Enter(order.TargetActor));
				self.QueueActivity(new Demolish(order.TargetActor, Info.C4Delay));
				self.QueueActivity(mobile.MoveTo(self.Location, 0));
			}
		}

		public string VoicePhraseForOrder(Actor self, Order order)
		{
			return (order.OrderString == "C4") ? "Attack" : null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR
{
    public class CompMeltable : ThingComp
	{
		protected float meltRate = 0;

        public CompProperties_Meltable PropsMelt => (CompProperties_Meltable)props;

		public MeltStage Stage
		{
			get
			{
				return parent.AmbientTemperature <= 0 ? MeltStage.Solid: MeltStage.Melting;
			}
		}

		public override void CompTick()
		{
			Tick(1);
		}

		public override void CompTickRare()
		{
			Tick(250);
		}

		private void Tick(int interval)
		{
			if (Stage == MeltStage.Melting)
			{
				meltRate = GenTemperature.RotRateAtTemperature(parent.AmbientTemperature); //Update melt rate.
				var meltRateOverInterval = meltRate * interval;
				var damage = Mathf.RoundToInt(meltRateOverInterval); //Damage must be int.
				Log.Message("Melting Item Releasing " + -meltRateOverInterval + " heat at ambient temperature " + parent.AmbientTemperature + " for an interval of " + interval + ", taking " + damage + " damage.");
				GenTemperature.PushHeat(parent.PositionHeld, parent.MapHeld, -meltRateOverInterval); //Cool area by damage amount.
				if (parent.stackCount > 1 && parent.HitPoints <= damage) //If it's a stack and we're about to run out of HP..
				{
					parent.stackCount--; //Decrement stack.
					damage -= parent.HitPoints; //Reduce damage by hitpoints to not be kind, we want to reserve it and apply it still.
					parent.HitPoints = parent.MaxHitPoints; //Reset HP instead.
				}
				else //Not about to run out of HP..
					parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, damage)); //Do damage.
			}
		}

		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(parent.stackCount + count);
			parent.HitPoints = GenMath.RoundRandom(Mathf.Lerp((float)parent.HitPoints, (float)parent.MaxHitPoints, t));
		}

		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (Stage)
			{
				case MeltStage.Solid:
					stringBuilder.Append("MeltStateSolid".Translate() + ".");
					break;
				case MeltStage.Melting:
					stringBuilder.Append("MeltStateMelting".Translate() + ".\n" + (meltRate > 0 ? ("Melt rate: " + Math.Round(meltRate, 2) + " / tick") : ""));
					break;
			}
			return stringBuilder.ToString();
		}
	}
}
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
		private float MeltProgressInt;

        public CompProperties_Meltable PropsMelt => (CompProperties_Meltable)props;

		public float MeltProgressPct => MeltProgress / (float)PropsMelt.TicksToMeltStart;

		public float MeltProgress
		{
			get
			{
				return MeltProgressInt;
			}
			set
			{
				MeltProgressInt = value;
			}
		}

		public MeltStage Stage
		{
			get
			{
				if (PropsMelt.MeltingDestroys)
				{
					return parent.AmbientTemperature <= 0 ? MeltStage.Solid: MeltStage.Melting;
				}
				else
				{
					return MeltProgress < PropsMelt.TicksToMeltStart ? MeltStage.Solid : MeltStage.Melting;
				}
			}
		}

		public int TicksUntilMeltAtCurrentTemp
		{
			get
			{
				float ambientTemperature = parent.AmbientTemperature;
				ambientTemperature = Mathf.RoundToInt(ambientTemperature);
				return TicksUntilMeltAtTemp(ambientTemperature);
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref MeltProgressInt, "MeltProg", 0f);
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
			float meltProgress = MeltProgress;
			float num = GenTemperature.RotRateAtTemperature(parent.AmbientTemperature);
			MeltProgress += num * (float)interval;
			if (Stage == MeltStage.Melting)
			{
				if (PropsMelt.MeltDamagePerHour > 0f)
				{
					if (Mathf.FloorToInt(meltProgress / 2500f) != Mathf.FloorToInt(MeltProgress / 2500f) && ShouldTakeMeltDamage())
					{
						if (/*parent.HitPoints <= PropsMelt.MeltDamagePerHour && */PropsMelt.MeltingDestroys)
                        {
							if (parent.IsInAnyStorage() && parent.SpawnedOrAnyParentSpawned)
							{
								Messages.Message("MessageMeltedAway".Translate(parent.def.label, parent).CapitalizeFirst(), new TargetInfo(parent.PositionHeld, parent.MapHeld), MessageTypeDefOf.NegativeEvent);
							}
							parent.Destroy();
						}
						else
							parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, GenMath.RoundRandom(PropsMelt.MeltDamagePerHour)));	
					}
				}
			}
		}

		private bool ShouldTakeMeltDamage()
		{
			if (parent.ParentHolder is Thing thing && thing.def.category == ThingCategory.Building && thing.def.building.preventDeteriorationInside)
			{
				return false;
			}
			return true;
		}

		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(parent.stackCount + count);
			float MeltProgress = ((ThingWithComps)otherStack).GetComp<CompMeltable>().MeltProgress;
			MeltProgress = Mathf.Lerp(MeltProgress, MeltProgress, t);
		}

		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompMeltable>().MeltProgress = MeltProgress;
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
					stringBuilder.Append("MeltStateMelting".Translate() + ".");
					break;
			}
			if ((float)PropsMelt.TicksToMeltStart - MeltProgress > 0f)
			{
				float num = GenTemperature.RotRateAtTemperature(Mathf.RoundToInt(parent.AmbientTemperature));
				int ticksUntilMeltAtCurrentTemp = TicksUntilMeltAtCurrentTemp;
				stringBuilder.AppendLine();
				if (num < 0.001f)
				{
					stringBuilder.Append("MeltableCurrentlyFrozen".Translate() + ".");
				}
				else if (num < 0.999f)
				{
					stringBuilder.Append("MeltableCurrentlyRefrigerated".Translate(ticksUntilMeltAtCurrentTemp.ToStringTicksToPeriod()) + ".");
				}
				else
				{
					stringBuilder.Append("MeltableNotRefrigerated".Translate(ticksUntilMeltAtCurrentTemp.ToStringTicksToPeriod()) + ".");
				}
			}
			return stringBuilder.ToString();
		}

		public int ApproxTicksUntilMeltWhenAtTempOfTile(int tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return TicksUntilMeltAtTemp(temperatureFromSeasonAtTile);
		}

		public int TicksUntilMeltAtTemp(float temp)
		{
			float num = GenTemperature.RotRateAtTemperature(temp);
			if (num <= 0f)
			{
				return 72000000;
			}
			float num2 = (float)PropsMelt.TicksToMeltStart - MeltProgress;
			if (num2 <= 0f)
			{
				return 0;
			}
			return Mathf.RoundToInt(num2 / num);
		}

        public void MeltImmediately()
		{
			if (MeltProgress < (float)PropsMelt.TicksToMeltStart)
			{
				MeltProgress = PropsMelt.TicksToMeltStart;
			}
		}
	}
}

﻿using System;
using EngineCore.Simulater;
using GameLogic.Game.Elements;

namespace GameLogic.Game.Controllors
{
	public class MagicReleaserControllor : GControllor
	{
		public MagicReleaserControllor(GPerception per) : base(per)
		{

		}

		public override GAction GetAction(GTime time, GObject current)
		{
			var releaser = current as MagicReleaser;
            releaser.TickTimeLines(time);

			switch (releaser.State)
			{
				case ReleaserStates.NOStart:
					{
						releaser.OnEvent(Layout.EventType.EVENT_START);
						releaser.SetState(ReleaserStates.Releasing);
                        //tick
                        if (releaser.Magic.triggerDurationTime > 0)
                        {
                            releaser.LastTickTime = time.Time;
                            releaser.tickStartTime = time.Time;
                            releaser.OnEvent(Layout.EventType.EVENT_TRIGGER);
                        }
					}
					break;
				case ReleaserStates.Releasing:
					{
                        if (releaser.Magic.triggerTicksTime > 0)
                        {
                            if (releaser.tickStartTime + releaser.Magic.triggerDurationTime > time.Time)
                            {
                                if (releaser.LastTickTime + releaser.Magic.triggerTicksTime < time.Time)
                                {
                                    releaser.LastTickTime = time.Time;
                                    releaser.OnEvent(Layout.EventType.EVENT_TRIGGER);
                                }
                                break;
                            }
                        }

						if (releaser.IsCompleted)
						{
							releaser.SetState(ReleaserStates.ToComplete);
						}
					}
					break;
                case ReleaserStates.ToComplete:
                    { 
                        releaser.OnEvent(Layout.EventType.EVENT_END);
                        releaser.SetState(ReleaserStates.Completing);
                    }
                    break;
				case ReleaserStates.Completing:
					{
						if (releaser.IsCompleted)
						{
							releaser.SetState(ReleaserStates.Ended);
						}
					}
					break;
				case ReleaserStates.Ended:
					{
						if (releaser.IsCompleted)
						{
							GObject.Destory(releaser);
						}
					}
					break;
			}
			return GAction.Empty;
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WurmAssistant.Granger
{
    class GrangerAI
    {
        //
        enum ViabilityFlag { Included, Excluded, Inbreed, Ignored }

        class WorkItem
        {
            public Horse horse;
            public ViabilityFlag flag = ViabilityFlag.Included;
            public Color color = new Color();
            public float Value = 0F;
        }

        List<WorkItem> HorsesWorker = new List<WorkItem>();

        private Horse curHorse;
        private Herd herd;

        public float DuplicateGoodFactor = 1.0F;
        public float DuplicateBadFactor = 1.0F;

        public GrangerAI(Horse currentlySelectedHorse, Herd herd)
        {
            this.curHorse = currentlySelectedHorse;
            this.herd = herd;

            foreach (Horse horse in herd.Horses.Values)
            {
                if (horse != curHorse)
                {
                    WorkItem item = new WorkItem();
                    item.horse = horse;
                    if (horse.IsMale == curHorse.IsMale) item.flag = ViabilityFlag.Ignored;
                    HorsesWorker.Add(item);
                }
            }
        }

        public void ExcludeNegatives()
        {
            foreach (WorkItem item in HorsesWorker)
            {
                if (TraitValues.HasAnyNegatives(item.horse)) item.flag = ViabilityFlag.Excluded;
            }
        }

        public void ExcludeInbreed()
        {
            foreach (WorkItem item in HorsesWorker)
            {
                if (item.horse.IsInbreed(curHorse) && item.horse.IsMale != curHorse.IsMale) 
                    item.flag = ViabilityFlag.Excluded;
            }
        }

        public void IgnoreUnavailable()
        {
            foreach (WorkItem item in HorsesWorker)
            {
                if (item.flag != ViabilityFlag.Excluded 
                    && (item.horse.IsNotInMood 
                    || item.horse.IsPregnant
                    || item.horse.maybeDied))
                {
                    item.flag = ViabilityFlag.Ignored;
                }
            }
        }

        float maximumBreedingPositiveValue = 0F;

        public void PerformCalculations(bool preferMissingGoodTraits, bool includePotentialValue)
        {
            maximumBreedingPositiveValue = TraitValues.GetHighestCombinedPositiveTraitValue(DuplicateGoodFactor);
            foreach (WorkItem item in HorsesWorker)
            {
                if (item.flag != ViabilityFlag.Excluded)
                {
                    if (item.horse.IsInbreed(curHorse) && curHorse.IsMale != item.horse.IsMale) 
                        item.flag = ViabilityFlag.Inbreed;

                    if (preferMissingGoodTraits) item.Value = item.horse.GetBreedingValueForUniqueGoodTraits(curHorse);
                    else item.Value = item.horse.GetBreedingValueForAllGoodTraits(curHorse, DuplicateGoodFactor);

                    item.Value += item.horse.GetBreedingValueForAllBadTraits(curHorse, DuplicateBadFactor);

                    if (includePotentialValue) item.Value += item.horse.GetPotentialBreedingValue(curHorse, DuplicateGoodFactor, DuplicateBadFactor);
                }
            }
        }

        public Dictionary<Horse, Color> GetColoredDict()
        {
            WorkItem bestCandidate = null;

            foreach (WorkItem item in HorsesWorker)
            {
                CustomColors.HSLColor color = new CustomColors.HSLColor();
                color.Luminosity = 210D;
                color.Saturation = 240D;
                color.Hue = 0D;

                if (bestCandidate == null || item.Value > bestCandidate.Value)
                {
                    if (item.flag != ViabilityFlag.Ignored && item.flag != ViabilityFlag.Excluded) bestCandidate = item;
                }

                if (item.flag == ViabilityFlag.Excluded)
                {
                    color.Hue = 0D; //red
                }
                else if (item.flag == ViabilityFlag.Inbreed)
                {
                    color.Hue = 20D; //yellow-orange
                }
                else if (item.flag == ViabilityFlag.Included)
                {
                    color.Hue = 35D; //yellow - default for zero traits
                }

                if (item.flag != ViabilityFlag.Excluded)
                {
                    color.Hue += (item.Value / maximumBreedingPositiveValue) * 45F;
                }

                if (item.flag == ViabilityFlag.Ignored)
                {
                    color.Hue = 0D;
                    color.Saturation = 0D;
                    color.Luminosity = 210D;
                }

                item.color = color;
            }

            if (bestCandidate != null) bestCandidate.color = new CustomColors.HSLColor(120D, 240D, 180D);

            Dictionary<Horse, Color> returnDict = new Dictionary<Horse, Color>();

            foreach (WorkItem item in HorsesWorker)
            {
                returnDict.Add(item.horse, item.color);
            }

            return returnDict;
        }
    }
}

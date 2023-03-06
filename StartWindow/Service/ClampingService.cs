using StartWindow.Enums;
using System.Collections.Generic;

namespace StartWindow.Service
{
    public static class ClampingService
    {
        public static List<string> GetAll()
        {
            List<string> Clampings = new List<string>();
            Clampings.Clear();
            Clampings.Add(EnumClamping.GripZabierak.ToString());
            Clampings.Add(EnumClamping.GripGrip.ToString());
            Clampings.Add(EnumClamping.GripTang.ToString());
            Clampings.Add(EnumClamping.GripPinWelding.ToString());
            return Clampings;
        }
    }
}

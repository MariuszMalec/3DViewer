using StartWindow.Enums;
using System.Collections.Generic;

namespace StartWindow.Service
{
    public static class MachineService
    {
        public static List<string> GetAll()
        {
            List<string> machines = new List<string>();
            machines.Clear();
            machines.Add(EnumMachine.HM_HSTM_300_SIM840D.ToString());
            machines.Add(EnumMachine.HM_HSTM_300HD_SIM840D.ToString());
            machines.Add(EnumMachine.SH_HX151_24_SIM840D.ToString());
            machines.Add(EnumMachine.HM_HSTM_500_SIM840D.ToString());
            machines.Add(EnumMachine.HM_HSTM_500M_SIM840D.ToString());
            machines.Add(EnumMachine.HURON_EX20_SIM840D.ToString());
            machines.Add(EnumMachine.HM_HSTM_1000_SIM840D.ToString());
            return machines;
        }
    }
}

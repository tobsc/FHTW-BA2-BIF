using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HwInf.Common.BL
{
    public class BL
    {
        private HwInfContext _dal;

        public BL() {
            _dal = new HwInfContext();
        }
        public BL(HwInfContext dal)
        {
            _dal = dal;
        }

        public IQueryable<Device> GetDevices(bool onlyActive = true)
        {

            return _dal.Devices.Include(x => x.Type)
                .Where(i => i.DeviceId > 0) // TODO: where is active
                .Take(10000);
        }
    }
}

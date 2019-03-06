using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wiki.Models;

namespace Wiki.Classes
{
    public interface IRKDWorkFactory
    {
        RKDWork makeRKDWork(RKD rkd);
    }
}

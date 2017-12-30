using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballPicker.Criteria
{
    public abstract class Criteria
    {
        abstract public double GetNormalized();
    }
}

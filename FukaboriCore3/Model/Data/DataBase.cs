using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FukaboriCore.Model.Data
{
    public　class DataBase
    {
        Dictionary<Column,double[]> doubleRows { get; set; }
        Dictionary<Column,int[]> intRows { get; set; }
        Dictionary<Column,string[]> stringRows { get; set; }

        
    }

    public class Column
    {

    }
}

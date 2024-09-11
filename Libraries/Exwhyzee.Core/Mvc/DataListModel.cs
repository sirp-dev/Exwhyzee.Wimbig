using System.Collections.Generic;

namespace Exwhyzee.Mvc
{
    public class DataListModel<T>
    {
        public DataListModel(IEnumerable<T> data, int recordsFiltered, int recordsTotal)
        {
            Data = data;
            RecordsFiltered = recordsFiltered;
            RecordsTotal = recordsTotal;
        }

        public IEnumerable<T> Data { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}

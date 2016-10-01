using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feeder.Common.ViewModel
{
    public interface IViewModel
    {
        void Initialize(object parameter);

        Task<bool> OnLeaving(object parameter);
    }
}

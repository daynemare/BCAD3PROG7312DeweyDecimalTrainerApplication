using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ModelRegisterLogin
    {
       public int USER_LIBRARY_CARD_ID { get; set; }
        public string USER_PASSWORD { get; set; }
        public string USER_FNAME { get; set; }
        public string USER_LNAME { get; set; }

    }
}

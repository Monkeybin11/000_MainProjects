using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public class StringLogger : ILogger<string>
    {
        private static StringLogger instance;

        public static StringLogger GetInstance()
        {
            if ( instance == null ) instance = new StringLogger();
            return instance;
        }

        public List<T> WriterLog( List<T> logs , T log)
        {
            logs.Add(     )

        }


        public List<string> LogList
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    namespace MyNamespace
    {
        public class Car
        {

        }
    }
    public class Car
    {
        public int No { get; set; } 

        //public void Start()
        //{
            
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic HotStart()
        {
            return 5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic CoolStart()
        {
            return -1;
        }
        public void Start(string option)
        {
            if (option=="Auto")
            {

            }
            if (option == "Manual")
            {

            }
        }
    }
}

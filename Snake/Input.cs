using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Input
    {
        //List of availible kayboard buttons
        private static Hashtable keyTable = new Hashtable(); 

        // check to see what button is pressed
        public static bool KeyPresed(Keys key)
        {
            if (keyTable[key] == null) return false;
            return (bool)keyTable[key];
        }
        //Detect if a keyboard button is pressed
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}

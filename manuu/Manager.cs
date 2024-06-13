using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace manuu
{
    internal class Manager
    {
        public static Frame MainFrame { get; set; }
        public static int user_now_admin = 0;
        public static TextBlock page_now_text { get; set; }

        public static void Add_test()
        {
            var need = new Bludo
            {
                name_bludo = "Test",
            };

            for_restoranEntities.GetContext().Bludo.Add(need);
            for_restoranEntities.GetContext().SaveChanges();


        }
    }
}

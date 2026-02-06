using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktasıdır.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Uygulamayı 'AnaSayfaForm' ile başlat
            Application.Run(new AnaSayfaForm());
        }
    }
}
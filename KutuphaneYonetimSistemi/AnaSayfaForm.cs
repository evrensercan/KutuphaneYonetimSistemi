using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KutuphaneYonetimSistemi
{
    public partial class AnaSayfaForm : Form
    {
        #region Veritabanı Bağlantısı ve Tanımlamalar

        // SQL Bağlantı dizesi (Connection String)
        SqlConnection baglanti = new SqlConnection("Server=LAPTOP-K7ECI3M9\\SQLEXPRESS;Database=KutuphaneYonetimSistemi_DB;Trusted_Connection=True;TrustServerCertificate=True;");

        #endregion

        public AnaSayfaForm()
        {
            InitializeComponent();
        }

        private void AnaSayfaForm_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde güncel istatistikleri panele yansıt
            IstatistikleriGetir();
        }

        private void AnaSayfaForm_Activated(object sender, EventArgs e)
        {
            // Ana sayfaya her dönüldüğünde sayıları güncelle (Örn: Kitap eklendiyse sayı artsın)
            IstatistikleriGetir();
        }

        #region Menü Navigasyon İşlemleri

        /// <summary>
        /// Üye Yönetim Paneline geçişi sağlar.
        /// </summary>
        private void btnUye_Click(object sender, EventArgs e)
        {
            UyeYonetimForm uyeSyfası = new UyeYonetimForm();
            this.Hide(); // Ana formu gizle
            uyeSyfası.ShowDialog(); // Yeni formu aç
            this.Show(); // İşlem bitince ana formu tekrar göster
        }

        /// <summary>
        /// Kitap Stok ve Yönetim Paneline geçişi sağlar.
        /// </summary>
        private void btnKitap_Click(object sender, EventArgs e)
        {
            KitapYonetimForm kitap = new KitapYonetimForm();
            this.Hide();
            kitap.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Kitap Ödünç Verme (Emanet) Paneline geçişi sağlar.
        /// </summary>
        private void btnHareket_Click(object sender, EventArgs e)
        {
            OduncIslemForm odunc = new OduncIslemForm();
            this.Hide();
            odunc.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// İade İşlemleri Paneline geçişi sağlar.
        /// </summary>
        private void btnIade_Click(object sender, EventArgs e)
        {
            IadeIslemForm iade = new IadeIslemForm();
            this.Hide();
            iade.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Tüm geçmiş hareketlerin listelendiği panele geçişi sağlar.
        /// </summary>
        private void btnGecmis_Click(object sender, EventArgs e)
        {
            IslemGecmisiForm gecmis = new IslemGecmisiForm();
            this.Hide();
            gecmis.ShowDialog();
            this.Show();
        }

        #endregion

        #region Veritabanı Sorgu İşlemleri

        /// <summary>
        /// Veritabanından toplam üye, kitap ve emanet sayılarını çeker ve etiketlere yazar.
        /// </summary>
        void IstatistikleriGetir()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            // 1. Toplam Üye Sayısı
            SqlCommand uyeSayisi = new SqlCommand("SELECT COUNT(*) FROM TBL_UYE", baglanti);
            lblUyeSayisi.Text = uyeSayisi.ExecuteScalar().ToString();

            // 2. Toplam Kitap Sayısı
            SqlCommand kitapSayisi = new SqlCommand("SELECT COUNT(*) FROM TBL_KİTAP", baglanti);
            lblKitapSayisi.Text = kitapSayisi.ExecuteScalar().ToString();

            // 3. Emanetteki (Dışarıdaki) Kitap Sayısı
            SqlCommand emanetSayisi = new SqlCommand("SELECT COUNT(*) FROM TBL_KİTAP WHERE Durum = 0", baglanti);
            lblEmanetSayisi.Text = emanetSayisi.ExecuteScalar().ToString();

            // 4. Raftaki (Müsait) Kitap Sayısı
            SqlCommand rafSayisi = new SqlCommand("SELECT COUNT(*) FROM TBL_KİTAP WHERE Durum = 1", baglanti);
            lblRafSayisi.Text = rafSayisi.ExecuteScalar().ToString();

            baglanti.Close();
        }

        #endregion

        #region UI Olayları (Boş)
        // Tasarım tarafında yanlışlıkla oluşturulan olaylar
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void lblUyeSayisi_Click(object sender, EventArgs e) { }
        private void lblRafSayisi_Click(object sender, EventArgs e) { }
        #endregion
    }
}
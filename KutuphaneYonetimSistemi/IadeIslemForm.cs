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
    public partial class IadeIslemForm : Form
    {
        #region Değişkenler ve Bağlantı

        SqlConnection baglanti = new SqlConnection("Server=LAPTOP-K7ECI3M9\\SQLEXPRESS;Database=KutuphaneYonetimSistemi_DB;Trusted_Connection=True;TrustServerCertificate=True;");

        // İşlem yapılacak hareketin ID'sini tutan değişken (Varsayılan: 0)
        string secilenHareketID = "0";

        #endregion

        public IadeIslemForm()
        {
            InitializeComponent();
        }

        private void IadeIslemForm_Load(object sender, EventArgs e)
        {
            // Sayfa yüklendiğinde emanetteki kitapları listele
            EmanetListesi();
        }

        #region Listeleme ve Seçim İşlemleri

        /// <summary>
        /// Henüz iade edilmemiş (IadeTarihi NULL olan) kitapları listeler.
        /// </summary>
        void EmanetListesi()
        {
            baglanti.Open();

            // İlişkili tablolardan (JOIN) Üye ve Kitap isimlerini getirir
            string sorgu = "SELECT HareketID, UyeAd + ' ' + UyeSoyad AS 'Üye', KitapAdi AS 'Kitap', AlisTarihi " +
                           "FROM TBL_HAREKET " +
                           "INNER JOIN TBL_UYE ON TBL_HAREKET.UyeID = TBL_UYE.UyeID " +
                           "INNER JOIN TBL_KİTAP ON TBL_HAREKET.KitapID = TBL_KİTAP.KitapID " +
                           "WHERE IadeTarihi IS NULL";

            SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            baglanti.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kullanıcının tablodan seçtiği satırın verilerini al
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            // Hareket ID'sini global değişkene ata (İade işlemi için kullanılacak)
            secilenHareketID = dataGridView1.Rows[secilen].Cells[0].Value.ToString();

            // Bilgi etiketine seçilen kişinin ve kitabın adını yazdır
            string uyeAd = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            string kitapAd = dataGridView1.Rows[secilen].Cells[2].Value.ToString();

            lblBilgi.Text = uyeAd + " - " + kitapAd;
        }

        #endregion

        #region İade ve Ceza İşlemleri

        /// <summary>
        /// Seçilen kitabın iade işlemini gerçekleştirir ve gecikme varsa ceza hesaplar.
        /// </summary>
        private void btnIadeAl_Click(object sender, EventArgs e)
        {
            if (secilenHareketID == "0")
            {
                MessageBox.Show("Lütfen listeden iade edilecek işlemi seçin!");
                return;
            }

            // --- Ceza Hesaplama Algoritması ---
            DateTime alisTarihi = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["AlisTarihi"].Value);
            DateTime bugun = DateTime.Now;

            // Tarihler arası farkı gün bazında bul
            TimeSpan fark = bugun - alisTarihi;
            int gunSayisi = fark.Days;

            decimal cezaTutari = 0;

            // Eğer 15 günden fazla kaldıysa ceza uygula
            if (gunSayisi > 15)
            {
                int gecikenGun = gunSayisi - 15;
                cezaTutari = gecikenGun * 5.0m; // Günlük 5 TL Ceza

                DialogResult cezaOnay = MessageBox.Show("Kitap " + gecikenGun + " gün gecikmiş!\n\n"
                    + "Tahsil Edilecek Ceza: " + cezaTutari.ToString("C2")
                    + "\n\nCezayı tahsil ettiniz mi? İşleme devam edilsin mi?",
                    "Gecikme Cezası", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (cezaOnay == DialogResult.No)
                {
                    return; // Kullanıcı cezayı onaylamazsa işlemi durdur
                }
            }
            // -------------------------------------------

            baglanti.Open();

            // 1. ADIM: HAREKET TABLOSUNU GÜNCELLE (İade Tarihini Gir)
            SqlCommand komutHareket = new SqlCommand("UPDATE TBL_HAREKET SET IadeTarihi = @p1 WHERE HareketID = @p2", baglanti);
            komutHareket.Parameters.AddWithValue("@p1", DateTime.Now); // Şu anki zaman
            komutHareket.Parameters.AddWithValue("@p2", secilenHareketID);
            komutHareket.ExecuteNonQuery();

            // 2. ADIM: KİTAP DURUMUNU GÜNCELLE (Durumu '1' yani 'Rafta' yap)
            SqlCommand komutKitap = new SqlCommand("UPDATE TBL_KİTAP SET Durum = 1 WHERE KitapID = (SELECT KitapID FROM TBL_HAREKET WHERE HareketID = @p3)", baglanti);
            komutKitap.Parameters.AddWithValue("@p3", secilenHareketID);
            komutKitap.ExecuteNonQuery();

            baglanti.Close();

            MessageBox.Show("Kitap iade alındı ve rafa kaldırıldı!");

            // Sayfayı temizle ve listeyi yenile
            EmanetListesi();
            lblBilgi.Text = "...";
            secilenHareketID = "0";
        }

        #endregion

        // Kullanılmayan olaylar
        private void lblBilgi_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
    }
}
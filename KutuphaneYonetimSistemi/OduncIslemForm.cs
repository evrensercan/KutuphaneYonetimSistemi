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
    public partial class OduncIslemForm : Form
    {
        #region Değişkenler

        SqlConnection baglanti = new SqlConnection("Server=LAPTOP-K7ECI3M9\\SQLEXPRESS;Database=KutuphaneYonetimSistemi_DB;Trusted_Connection=True;TrustServerCertificate=True;");

        // İşlem için seçilen Üye ve Kitap ID'leri
        string secilenUyeID = "0";
        string secilenKitapID = "0";

        #endregion

        public OduncIslemForm()
        {
            InitializeComponent();
        }

        private void OduncIslemForm_Load(object sender, EventArgs e)
        {
            // Sayfa açıldığında tabloları doldur
            UyeListele();
            KitapListele();
        }

        #region Listeleme ve Arama Metotları

        /// <summary>
        /// Tüm üyeleri veritabanından çeker.
        /// </summary>
        void UyeListele()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT UyeID, UyeAd, UyeSoyad, UyeTelNo FROM TBL_UYE", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        /// <summary>
        /// Sadece 'Rafta' (Durum=1) olan, yani müsait kitapları çeker.
        /// </summary>
        void KitapListele()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT KitapID, KitapAdi, KitapYazari, KitapTürü, KitapSayfaSayisi FROM TBL_KİTAP WHERE Durum = 1", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        // Üye Arama Kutusu (Her harf yazıldığında çalışır)
        private void txtUyeAra_TextChanged(object sender, EventArgs e)
        {
            string sorgu = "SELECT UyeID, UyeAd, UyeSoyad, UyeTelNo FROM TBL_UYE WHERE UyeAd LIKE '%" + txtUyeAra.Text + "%' OR UyeSoyad LIKE '%" + txtUyeAra.Text + "%'";
            SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        // Kitap Arama Kutusu
        private void txtKitapAra_TextChanged(object sender, EventArgs e)
        {
            string sorgu = "SELECT KitapID, KitapAdi, KitapYazari, KitapTürü, KitapSayfaSayisi FROM TBL_KİTAP WHERE Durum = 1 AND KitapAdi LIKE '%" + txtKitapAra.Text + "%'";
            SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        #endregion

        #region Seçim İşlemleri

        // Üye Tablosundan seçim yapıldığında
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            // 1. ID'yi hafızaya al (Veritabanı kaydı için)
            secilenUyeID = dataGridView1.Rows[secilen].Cells[0].Value.ToString();

            // 2. İsmi ekrana yaz (Kullanıcı bilgilendirmesi için)
            string ad = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            string soyad = dataGridView1.Rows[secilen].Cells[2].Value.ToString();

            lblSecilenUye.Text = ad + " " + soyad;
        }

        // Kitap Tablosundan seçim yapıldığında
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;

            secilenKitapID = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
            lblSecilenKitap.Text = dataGridView2.Rows[secilen].Cells[1].Value.ToString();
        }

        #endregion

        #region Ödünç Verme İşlemi

        /// <summary>
        /// Seçilen kitabı seçilen üyeye ödünç verir. 
        /// Hem Hareket tablosuna kayıt atar hem de Kitap tablosunda durumu günceller.
        /// </summary>
        private void btnOdunc_Click(object sender, EventArgs e)
        {
            // GÜVENLİK KONTROLÜ
            if (secilenUyeID == "0" || secilenKitapID == "0")
            {
                MessageBox.Show("Lütfen hem üyeyi hem de kitabı seçiniz!");
                return;
            }

            baglanti.Open();

            // 1. ADIM: HAREKET KAYDI OLUŞTUR
            SqlCommand komutHareket = new SqlCommand("INSERT INTO TBL_HAREKET (KitapID, UyeID, AlisTarihi) VALUES (@k1, @u1, @t1)", baglanti);
            komutHareket.Parameters.AddWithValue("@k1", secilenKitapID);
            komutHareket.Parameters.AddWithValue("@u1", secilenUyeID);
            komutHareket.Parameters.AddWithValue("@t1", dtpTarih.Value); // Seçilen tarih
            komutHareket.ExecuteNonQuery();

            // 2. ADIM: KİTAP DURUMUNU GÜNCELLE (Raftan İndir -> Durum=0)
            SqlCommand komutKitap = new SqlCommand("UPDATE TBL_KİTAP SET Durum = 0 WHERE KitapID = @k2", baglanti);
            komutKitap.Parameters.AddWithValue("@k2", secilenKitapID);
            komutKitap.ExecuteNonQuery();

            baglanti.Close();

            MessageBox.Show("Kitap başarıyla ödünç verildi!");

            // TEMİZLİK VE YENİLEME
            lblSecilenUye.Text = "...";
            lblSecilenKitap.Text = "...";
            secilenKitapID = "0";
            secilenUyeID = "0";

            // Kitap listesini yenile (Verilen kitap listeden düşmeli)
            KitapListele();
        }

        #endregion

        // Boş Olaylar
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void lblSecilenUye_Click(object sender, EventArgs e) { }
        private void lblSecilenKitap_Click(object sender, EventArgs e) { }
    }
}
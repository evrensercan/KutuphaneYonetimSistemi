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
    public partial class IslemGecmisiForm : Form
    {
        SqlConnection baglanti = new SqlConnection("Server=LAPTOP-K7ECI3M9\\SQLEXPRESS;Database=KutuphaneYonetimSistemi_DB;Trusted_Connection=True;TrustServerCertificate=True;");

        public IslemGecmisiForm()
        {
            InitializeComponent();
        }

        private void IslemGecmisiForm_Load(object sender, EventArgs e)
        {
            // Filtreleme seçeneklerini ComboBox'a ekle
            cmbFiltre.Items.Clear();
            cmbFiltre.Items.Add("Tüm İşlemler");
            cmbFiltre.Items.Add("İade Edilenler (Yeşil)");
            cmbFiltre.Items.Add("Teslim Bekleyenler (Kırmızı)");

            cmbFiltre.SelectedIndex = 0; // Varsayılan olarak tümünü seç

            FiltreliListele();
        }

        #region Dinamik Filtreleme ve Listeleme

        /// <summary>
        /// Kullanıcının arama metnine ve filtre seçimine göre verileri dinamik olarak getirir.
        /// </summary>
        void FiltreliListele()
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                // 1. Temel Sorgu İskeleti
                // "WHERE 1=1" tekniği, dinamik olarak AND eklemeyi kolaylaştırmak için kullanılmıştır.
                string sorgu = "SELECT HareketID, UyeAd + ' ' + UyeSoyad AS 'Üye', KitapAdi AS 'Kitap', AlisTarihi, IadeTarihi " +
                               "FROM TBL_HAREKET " +
                               "INNER JOIN TBL_UYE ON TBL_HAREKET.UyeID = TBL_UYE.UyeID " +
                               "INNER JOIN TBL_KİTAP ON TBL_HAREKET.KitapID = TBL_KİTAP.KitapID " +
                               "WHERE 1=1 ";

                // 2. Arama Kutusu Filtresi (Üye veya Kitap Adına Göre)
                if (!string.IsNullOrEmpty(txtIslemAra.Text))
                {
                    sorgu += " AND (UyeAd LIKE '%" + txtIslemAra.Text + "%' OR KitapAdi LIKE '%" + txtIslemAra.Text + "%')";
                }

                // 3. Durum Filtresi (ComboBox Seçimi)
                if (cmbFiltre.SelectedIndex == 1) // "İade Edilenler"
                {
                    sorgu += " AND IadeTarihi IS NOT NULL";
                }
                else if (cmbFiltre.SelectedIndex == 2) // "Teslim Bekleyenler"
                {
                    sorgu += " AND IadeTarihi IS NULL";
                }

                // 4. Sıralama (En son işlem en üstte)
                sorgu += " ORDER BY HareketID DESC";

                // 5. Verileri Çek
                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void txtIslemAra_TextChanged(object sender, EventArgs e)
        {
            // Arama kutusuna her harf girildiğinde listeyi güncelle
            FiltreliListele();
        }

        private void cmbFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Filtre kategorisi değiştiğinde listeyi güncelle
            FiltreliListele();
        }

        #endregion

        #region Görsel Düzenlemeler (Renklendirme)

        /// <summary>
        /// Tablo verilerle dolduktan sonra satırları duruma göre renklendirir.
        /// </summary>
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow satir in dataGridView1.Rows)
            {
                // Eğer İade Tarihi doluysa (Kitap gelmişse) -> YEŞİL
                if (satir.Cells["IadeTarihi"].Value != DBNull.Value)
                {
                    satir.DefaultCellStyle.BackColor = Color.LightGreen;
                    satir.DefaultCellStyle.SelectionBackColor = Color.Green;
                }
                // Eğer İade Tarihi boşsa (Kitap henüz gelmemişse) -> KIRMIZI
                else
                {
                    satir.DefaultCellStyle.BackColor = Color.Salmon;
                    satir.DefaultCellStyle.SelectionBackColor = Color.DarkRed;
                }
            }
        }

        #endregion
    }
}
# 📚 Kütüphane Yönetim Sistemi

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Status](https://img.shields.io/badge/Status-Completed-success?style=for-the-badge)

<img width="1225" height="963" alt="Ekran görüntüsü 2026-02-06 175253" src="https://github.com/user-attachments/assets/e5cdfdce-5195-43d1-ad48-ca6922e81c4b" />

C# Windows Forms ve SQL Server mimarisi üzerine inşa edilmiş, kapsamlı bir kütüphane otomasyon projesidir. Bu proje; üye takibi, kitap envanter yönetimi, ödünç/iade süreçleri ve ceza algoritmaları gibi gerçek hayat senaryolarını dijitalleştirmek amacıyla geliştirilmiştir.

## 🚀 Öne Çıkan Özellikler

Proje, katmanlı mimariye uygun olarak **CRUD (Create, Read, Update, Delete)** işlemlerinin tamamını kapsar.

* **👥 Üye Yönetimi:**
    * Yeni üye kaydı, bilgi güncelleme.
    * **Güvenli Silme (Soft Delete):** İşlem geçmişi olan üyelerin veritabanından tamamen silinmesini engelleyen ilişkisel bütünlük kontrolü.
* **📖 Kitap & Stok Takibi:**
    * Kitap ekleme, düzenleme ve kategorilendirme.
    * Anlık stok durumu görüntüleme.
* **🔄 Ödünç & İade Sistemi:**
    * Kitapların hangi üyede olduğunu ve iade tarihlerini takip etme.
    * Akıllı filtreleme ile **Teslim Edilenler (Yeşil)** ve **Gecikenler (Kırmızı)** şeklinde renkli listeleme.
* **⚠️ Otomatik Ceza Hesaplama:**
    * İade tarihi geçen kitaplar için gün bazlı otomatik ceza tutarı hesaplama algoritması.
* **📊 Anlık İstatistikler:**
    * Dashboard ekranında toplam üye, emanetteki kitap ve raftaki kitap sayılarının canlı takibi.

## 🛠️ Teknolojiler

* **Dil:** C#
* **Platform:** Windows Forms (.NET Framework)
* **Veritabanı:** Microsoft SQL Server (ADO.NET)
* **IDE:** Visual Studio 2022

## 💻 Kurulum Adımları

1.  Projeyi bilgisayarınıza klonlayın:
    ```bash
    git clone https://github.com/evrensercan/KutuphaneYonetimSistemi.git
    ```
2.  SQL Server'da `KutuphaneYonetimSistemi_DB` adında bir veritabanı oluşturun.
3.  `Script.sql` dosyasını çalıştırarak tabloları oluşturun.
4.  Form dosyalarındaki (`*Form.cs`) bağlantı cümlesinde yer alan sunucu adını (`Server=...`) kendi SQL Server adınızla değiştirin.
5.  Projeyi çalıştırın.

---
👨‍💻 **Geliştirici:** Evren SERCAN
🔗 **LinkedIn:** [linkedin.com/in/evren-sercan](https://www.linkedin.com/in/evren-sercan/)

---
## 📷 Diğer Ekran Görüntüleri

<details>
<summary>🖼️ Görselleri Görüntülemek İçin Tıklayın</summary>
<br>
<img width="1023" height="715" alt="Ekran görüntüsü 2026-02-06 175309" src="https://github.com/user-attachments/assets/34e86385-2ddd-4131-a5d1-d792e355c55a" />
<img width="1051" height="602" alt="Ekran görüntüsü 2026-02-06 175518" src="https://github.com/user-attachments/assets/58e7d80d-5855-4253-9ce4-f5bc091680f6" />
<img width="1368" height="989" alt="Ekran görüntüsü 2026-02-06 175724" src="https://github.com/user-attachments/assets/d31211d7-babe-4732-9cf2-999891cb4ba1" />
<img width="889" height="615" alt="Ekran görüntüsü 2026-02-06 175755" src="https://github.com/user-attachments/assets/762b5f72-a687-4faf-abb6-ad45305c780a" />
<img width="746" height="533" alt="Ekran görüntüsü 2026-02-06 175827" src="https://github.com/user-attachments/assets/7ce11e18-575e-4429-b936-77779ef36483" />
</details>

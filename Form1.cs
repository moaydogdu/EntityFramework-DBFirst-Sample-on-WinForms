using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EntityOrnek
{
    public partial class Form1 : Form
    {
        DbSinavOgrenciEntities1 db = new DbSinavOgrenciEntities1();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RRN9BL1;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * From tbldersler", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {

            dataGridView1.DataSource = db.TBLOGRENCİ.ToList();

            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            //Entity Framework Linq Sorgusu
            var query = from item in db.TBLNOTLAR
                        select new
                        {
                            item.NOTID,
                            item.TBLOGRENCİ.AD,
                            item.TBLOGRENCİ.SOYAD,
                            item.TBLDERSLER.DERSAD,
                            item.SINAV1,
                            item.SINAV2,
                            item.SINAV3,
                            item.ORTALAMA,
                            item.DURUM
                        };
            dataGridView1.DataSource = query.ToList();

            //dataGridView1.DataSource = db.TBLNOTLAR.ToList();

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            TBLOGRENCİ ogrenci = new TBLOGRENCİ();
            ogrenci.AD = TxtAd.Text;
            ogrenci.SOYAD = TxtSoyad.Text;

            db.TBLOGRENCİ.Add(ogrenci);
            db.SaveChanges();//Değişiklikleri kaydet, database'e yansıt.
            MessageBox.Show("Öğrenci Eklenmiştir.");

        }

        private void BtnDersEkle_Click(object sender, EventArgs e)
        {
            TBLDERSLER ders = new TBLDERSLER();
            ders.DERSAD = TxtDersAd.Text;

            db.TBLDERSLER.Add(ders);
            db.SaveChanges();
            MessageBox.Show("Ders Eklenmiştir.");
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCİ.Find(ID);
            db.TBLOGRENCİ.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Sistemden Silindi");
            //İlişkili bir değer silinirken hata alıyoruz.
            //İlişkili değerlerin silinmemesi gerekiyormuş ???
            //Onun yerine active ve passive yapısı kullanılıyor.
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCİ.Find(id);
            //Güncelleme işleminde bir atama söz konusu. Kendine ait metodu yok.
            x.AD = TxtAd.Text;
            x.SOYAD = TxtSoyad.Text;
            x.FOTOĞRAF = TxtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci başarıyla güncellendi.");
        }

        private void BtnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLOGRENCİ.Where
                (x => x.AD == TxtAd.Text && x.SOYAD == TxtSoyad.Text).ToList();
        }

        private void TxtAd_TextChanged(object sender, EventArgs e)
        {
            //LINQ Sorgusu ve Contains metodu ile yapıyorum.
            string aranan = TxtAd.Text;
            var degerler = from item in db.TBLOGRENCİ
                           where item.AD.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();
        }

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            List<TBLOGRENCİ> liste;
            if (radioButton1.Checked == true)
            {
                //ASC - Ascending
                liste = db.TBLOGRENCİ.OrderBy
                    (p => p.AD).ToList();
                dataGridView1.DataSource = liste;
            }
            else if (radioButton2.Checked == true)
            {
                liste = db.TBLOGRENCİ.OrderByDescending
                    (p => p.AD).ToList();
                dataGridView1.DataSource = liste;
            }
            else if (radioButton3.Checked == true)
            {
                //
                liste = db.TBLOGRENCİ.OrderBy
                    (p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = liste;
            }
            else if (radioButton4.Checked == true)
            {
                liste = db.TBLOGRENCİ.Where
                    (p => p.ID == 4).ToList();
                dataGridView1.DataSource = liste;
            }
            else if (radioButton5.Checked == true)
            {
                liste = db.TBLOGRENCİ.Where
                    (p => p.AD.StartsWith("A")).ToList();
                dataGridView1.DataSource = liste;

            }
            else if (radioButton6.Checked == true)
            {
                liste = db.TBLOGRENCİ.Where(p => p.AD.EndsWith("A")).ToList();
                dataGridView1.DataSource = liste;
            }
            else if (radioButton7.Checked == true)
            {
                bool deger = db.TBLOGRENCİ.Any();
                //Yukarıdaki any fonksiyonu hiç kayıt var mı diye bakar.
                MessageBox.Show(deger.ToString());
            }
            else if (radioButton8.Checked == true)
            {
                int toplam = db.TBLOGRENCİ.Count();
                MessageBox.Show($"Toplam kayıt sayısı:{toplam}");
            }
            else if (radioButton9.Checked == true)
            {
                //SUM
                var toplam = db.TBLNOTLAR.Sum(p => p.SINAV1);
                MessageBox.Show($"Toplam Sınav1 Puanı: {toplam}");
            }
            else if (radioButton10.Checked == true)
            {
                //AVG
                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);
                MessageBox.Show($"1.Sınavın Ortalaması: {ortalama}");
            }
            else if (radioButton11.Checked == true)
            {
                //MAX
                var maks = db.TBLNOTLAR.Max(p => p.SINAV1);
                MessageBox.Show($"1.Sınavın En Yükseği: {maks}");
            }
            else if (radioButton12.Checked == true)
            {
                var min = db.TBLNOTLAR.Min(p => p.SINAV1);
                MessageBox.Show($"1.Sınavın En Düşüğü: {min}");
            }
            //Ödev
            //Ortalamanın üzerinde olan notları yazdırmak.
            else if (radioButton13.Checked == true)
            {
                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);
                List<TBLNOTLAR> list = db.TBLNOTLAR.Where
                    (p => p.SINAV1 > ortalama).ToList();
                dataGridView1.DataSource = list;
            }


        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            //Linq sorgusunda join kullanma
            var sorgu = from item in db.TBLNOTLAR   
                        join item2 in db.TBLOGRENCİ
                        on item.OGR equals item2.ID
                        join item3 in db.TBLDERSLER
                        on item.DERS equals item3.DERSID
                        
                        select new
                        {
                            ogrenci = item2.AD + " "+item2.SOYAD,
                            ders = item3.DERSAD,
                            sinav1 = item.SINAV1,
                            sinav2 = item.SINAV2,
                            sinav3 = item.SINAV3,
                            ortalama = item.ORTALAMA
                        };
            dataGridView1.DataSource = sorgu.ToList();
        }
    }
}

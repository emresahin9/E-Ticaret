# E-Ticaret
 MVC deseni kullanılarak oluşturulmuş basit bir E-Ticaret sitesi örneği. Yönetici ürün ve kategori ekleyebilir, düzenleyebilir. Yorumları görebilir ve silebilir. Kullanıcı ürün satın alabilir ve yorum yapabilir.
 
 ## KURULUM
  SQL Management Studio'yu açıyoruz ve aşağıdaki gibi Attach'e tıklayıp dosyalarda database-files içindeki .mdf uzantılı dosyayı seçiyoruz.
  
  ![db0](https://user-images.githubusercontent.com/41463201/81695187-f240d200-946a-11ea-92dc-f492a5e8eb35.png)
  
  Projemizi Visual Studio'da açıyoruz ve yukardaki (LocalDB) ile başlayan server adını aşağıdaki Web.config içinde belirtilen alana yazıyoruz.
  
  ![db4](https://user-images.githubusercontent.com/41463201/81695189-f371ff00-946a-11ea-84bd-9546bcbbad56.png)
   Ardından projeyi çalıştırıyoruz.
   
   ---
   ## Giriş Sayfası
  Tüm tasarım responsive olarak tasarlanmıştır.
  
  ![1](https://user-images.githubusercontent.com/41463201/81695141-e5bc7980-946a-11ea-8e51-9bea70fde2a5.png)
  
  ---
  ## Ürünlerin Listelendiği Anasayfa
  
  ![2](https://user-images.githubusercontent.com/41463201/81695143-e6eda680-946a-11ea-9587-165f955736fa.png)
  
  ---
  ## Ürün Sayfası
  Kullanıcı ürün bilgilerine ulaşabilir ve ürünü sepete ekleyebilir.
  
  ![3](https://user-images.githubusercontent.com/41463201/81695148-e81ed380-946a-11ea-80e1-5aa7ba1b22f5.png)
  
  ---
  Aynı sayfada ürün yorumları gösterilir ve ürüne yorum yapılabilir.
  
  ![4](https://user-images.githubusercontent.com/41463201/81695153-e9500080-946a-11ea-82ab-84bbf1619cdc.png)
  
  ---
  ## Alışveriş Sepeti
  Sepete eklenen ürünler görülebilir, sepetten çıkarılabilir, satın alım yapılabilir.
  
  ![5](https://user-images.githubusercontent.com/41463201/81695155-ea812d80-946a-11ea-9506-2c1f5698ce8a.png)
  
  ---
  ## Profil Düzenleme
  Kullanıcı bilgilerini ve şifresini değiştirebilir.
  
  ![6](https://user-images.githubusercontent.com/41463201/81695164-ec4af100-946a-11ea-8a49-40c886bd7cef.png)
  
  ---
  ## Yönetici Girişi
  Url bölmesine aşağıdaki gibi YonetimGiris yazılarak yönetici giriş ekranına ulaşılabilir.
  
  ![7](https://user-images.githubusercontent.com/41463201/81695165-ece38780-946a-11ea-95e6-5106f9bc9948.png)
  
  ---
  Kayıtlı yönetici, şifresini unutursa yeni şifre oluşturulur ve mail olarak iletilir. 
  
  ![8](https://user-images.githubusercontent.com/41463201/81695169-ece38780-946a-11ea-8286-d3cf7cb59113.png)
  
  ---
  Siteye kayıtlı kullanıcılar görülebilir.
  
  ![9](https://user-images.githubusercontent.com/41463201/81695171-ed7c1e00-946a-11ea-9f44-af65e5f7a43c.png)
  
  ---
  ## Ürün İşlemleri
  Ürün ekleme, çıkarma, düzenleme yapılabilir.
  
  ![10](https://user-images.githubusercontent.com/41463201/81695175-eead4b00-946a-11ea-9996-f82f326eeb24.png)
  
  ![11](https://user-images.githubusercontent.com/41463201/81695178-efde7800-946a-11ea-8301-21e8bd510f94.png)
  
  ---
  ## Site İletişim Bilgileri Düzenleme
  
  ![12](https://user-images.githubusercontent.com/41463201/81695181-f0770e80-946a-11ea-8161-2de695372a69.png)
  
  ---
  Ürünlere yapılan yorumlar görülebilir ve silinebilir.
  
  ![13](https://user-images.githubusercontent.com/41463201/81695184-f10fa500-946a-11ea-8cee-0ac106dac19d.png)
  
  ---
  ### Veritabanı Diyagramı
  Code-First-Database olarak oluşturulmuştur. Ürün-Sepet arasında çoka çok; Kategori-Ürün, Ürün-Yorum, Kullanıcı-Yorum arasında bire çok; Kullanıcı-Sepet tabloları arasında da bire bir ilişki vardır. İletişim ve Yönetici tablolarında herhangi bir ilişki yoktur.
  
  ![eticaret-database](https://user-images.githubusercontent.com/41463201/81695193-f40a9580-946a-11ea-89ff-222cc8ff1d3e.png)

readMebyKata

Selam, kod içindeki commentlerim //** ile başlıyor. bu readme'yi Azure konfigürasyonuyla alakalı yazıyorum.

Microsoft Authentication işlemi için Azure'de bir Active Directory'ye ihtiyacımız olacak. Adım adım:

0- Gerekiyorsa önce kiracı (Tenant) yaratmalısınız. 1. adımı uygulamaya geçin isterse önce tenant yaratın

1- Azure'de "Azure Active Directory B2C" Yaratın

2- Azure AD B2C içerisinde Manage- App Registrations- New Registration ile bir kayıt yaratın.
	2.1- Yaratırken desteklenen hesap tipleri olarak "Accounts in any identity provider or organizational directory 
	(for authenticating users with user flows)" seçmelisiniz.

3- Kaydın içindeki Application (clientID) appsetting.json dosyasında kullanacağımız id olacak.

4- Authentication'a girin. Platform ekleyip "Web" seçin. Buraya Atlasın redirect adresini gireceğiz.
Configuration'da Callback-Path benim kodumda "/signin-microsoft" şeklinde ayarlı, bu da site adresimiz eğer
"exampleatlas.com.tr" ise redirectleneceğimiz adresin "exampleatlas.com.tr/signin-microsoft" olacağı anlamına geliyor.
Gereken şekilde düzenleyip redirect adresini kaydedin.
	4.1- Publishlediğim App için redirect adresi "https://basicapp20240710093731.azurewebsites.net/signin-microsoft".

5- Yine Authentication kısmında "Implicit grant and hybrid flows" kısmında 2 seçenek var. "Access tokens" ve "ID tokens".
Bu seçeneklerden ID Tokens'in seçili olduğuna ve Access Tokens'in seçili OLMADIĞINA emin olun. Aksi erişim hatası sebebi.


6- Yine Authentication'da en altta Advanced settings'te Allow public client flows seçeneği "No" olacak.

7- Manage kısmında Certificates and secret'a girin. Burada yeni bir secret oluşturun ve Value kısmını kopyalayın.
Kopyaladığınız secret'i appsettings.json'da ClientSecret'a yapıştırın.

Daha detaylı dokümantasyon için : 
https://learn.microsoft.com/tr-tr/azure/active-directory-b2c/tutorial-create-tenant#create-an-azure-ad-b2c-tenant

Umarım eksik yazdığım bir yer yoktur. Gerekli olursa yardımcı olmaya hazırım.
kata
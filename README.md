![Logo](https://www.adayazilim.com/public/dist/img/ada-yazilim-logo.png)

# Ada Yazılım - 2025 Staj Programı

**Ada Yazılım** olarak her yıl düzenli olarak gerçekleştirdiğimiz staj programımızın bu yıla ait planlaması aşağıda belirtilen esaslar doğrultusunda yürütülecektir:

Staj süresi **bir ay** olarak belirlenmiş olup, bu süre zarfında tüm stajyerlerin **sigortacılık alanına yönelik** belirli bir yazılım projesi üzerinde çalışmaları beklenmektedir. Geliştirilecek projenin konusu ve gereksinimleri bu dokümanın devamında ayrıntılı olarak belirtilmiştir. Bununla birlikte, **sigortacılık alanıyla ilişkili** farklı bir proje fikri bulunan stajyerler, bu fikirlerini proje sorumlusu ekip liderine sunmaları ve **onay almaları halinde** kendi projelerini geliştirme yönünde ilerleyebileceklerdir.

Proje geliştirme sürecinde kullanılacak teknoloji ve platform seçiminde stajyerlere **serbestlik tanınmaktadır**. Web, mobil veya masaüstü tabanlı uygulamalar geliştirilebilmektedir. Şirket bünyesinde yaygın olarak .NET ve React teknolojileri kullanılmakta olduğundan, bu teknolojilerin tercih edilmesi önerilmektedir. **Ancak stajyerin kendini en yetkin, üretken ve rahat hissettiği teknolojiyi kullanması desteklenmektedir.**

**Staj programının temel amacı teknoloji bilgisini ölçmek değil; algoritma kurma yeteneği, problem çözme becerisi ve proje geliştirme yaklaşımını değerlendirmektir.**

Proje çalışmaları, **Ada Yazılım’a ait GitHub organizasyonu altında her stajyer için ayrılacak bireysel alanlar üzerinden yürütülecektir.** Tüm kodlama ve dokümantasyon süreci bu platform üzerinden yönetilecek; **haftalık ilerlemeler düzenli olarak GitHub’a aktarılacak ve ekip toplantılarında paylaşılacaktır.** Staj dönemi, her stajyerin geliştirme sürecine katkı sağladığı projenin **bireysel sunumu ile tamamlanacaktır.**


## Proje Konusu ve Amacı

2025 yılı Ada Yazılım staj programı kapsamında geliştirilecek proje, **Sigorta Yönetim Platformu**dur. Bu uygulamanın temel amacı, sigortacılık sektöründe yaygın olarak karşılaşılan süreçleri yazılım ortamına taşımak ve bu süreçlerin yönetimini kolaylaştıran bir sistem geliştirmektir.

Proje kapsamında aşağıdaki temel modüllerin yer alması beklenmektedir:

### Kullanıcı Yönetimi ve Yetkilendirme Modülü
Uygulamaya erişen kullanıcıların kimlik doğrulama süreçlerini ve yetki seviyelerinin yönetilmesini sağlar. Kullanıcıların kayıt olma, giriş yapma, şifre sıfırlama gibi temel işlemleri destekler. Farklı kullanıcı rollerine göre erişim izinleri tanımlanır.

### Üretim Modülü
Kullanıcının sistem üzerinden sigorta teklifi alabilmesi, teklifi onaylaması ve ardından online ödeme adımıyla poliçeleştirme işlemini gerçekleştirebilmesi.

### Müşteri Modülü
Bireysel ve kurumsal müşterilerin sisteme kaydedilmesi, bilgilerin güncellenmesi ve müşteri bazlı poliçe geçmişinin izlenmesi.

### Hasar Modülü
Poliçe sahibi kullanıcıların hasar bildirimi yapabilmesi, hasar dosyalarının oluşturulması ve sürecin sistem üzerinden takip edilebilmesi.

### Ödeme Modülü
Sigorta poliçelerinin satın alınması ve yenilenmesi süreçlerindeki ödeme işlemleri bu modülde yönetilir. Kredi kartı vb. ödeme yöntemleri taklit edilerek dummy ödeme süreçleri gerçekleştirilir. Gerçek finansal işlem veya ödeme altyapısı entegre edilmeyecek, sistem işleyişi örnek senaryolar üzerinden simüle edilecektir.

### Raporlama Modülü
Bu modül, sigorta süreçleri, müşteri işlemleri ve hasar durumlarına ilişkin verilerin analitik olarak sunulmasını sağlar. Grafik ve tablolarla raporlama yapılır.

### Döküman Modülü
Bu modül, poliçe ve müşteri ile ilgili belgelerin yüklenmesi, saklanması ve erişilmesini sağlar. Belgeler sistemde tutulur ancak gerçek dosya yönetimi operasyonları yerine basit örnek/dummy dosyalar ve işlemler kullanılacaktır. Amaç, doküman yönetimi sürecinin temel mantığını göstermek ve pratiğe dökmektir.

### EK NOT

Unit test veya End-to-End test gibi testler opsiyonel olup, uygulama içerisinde SMS, E-posta, Yapay Zeka (Chatbot/LLM) gibi entegrasyonlar da opsiyonel olarak değerlendirilecektir. Bu tür ek özelliklerin projeye dahil edilmesi **zorunlu olmamakla birlikte**, stajyerlerin teknik yetkinliklerini ve inisiyatiflerini göstermeleri açısından **artı puan** sağlayacaktır.

**Staj projesi olarak geliştirilen bu uygulama, gerçek bir üretim (production) ortamında kullanılmak üzere tasarlanmamış olup, sektörel süreçlerin temel prensiplerini anlamak ve uygulamak amacıyla hazırlanmıştır. Dolayısıyla, uygulamanın işleyişi ve kapsamı prototip seviyesinde olup, gerçek dünya koşullarını tamamen yansıtmayabilir.**

Bu proje ile stajyerlerin, gerçek hayatta karşılığı olan bir sektörel ihtiyacı analiz etme, yazılım mimarisi kurma, modüler bir yapı geliştirme ve iş süreçlerini yazılıma aktarma becerilerini geliştirmeleri hedeflenmektedir. Ayrıca, bu süreçte yazılım geliştirme yaşam döngüsünün temel adımları olan analiz, geliştirme, test ve sürümleme aşamalarını deneyimlemeleri amaçlanmaktadır. Staj programı boyunca kazanılacak bu deneyimlerin, katılımcıların kariyer yolculuğunda sağlam bir temel oluşturacağına inanıyor ve tüm stajyerlerimize projelerini büyük bir heyecan ve sorumlulukla geliştirmelerini diliyoruz.


### © 2025 Ada Yazılım. Tüm hakları saklıdır.

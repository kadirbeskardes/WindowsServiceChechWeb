# Windows Servis

Bu proje, bir Windows hizmeti olarak çalışan bir uygulamadır. Temel olarak internet bağlantısı ve belirli bir web sitesine bağlantı durumunu periyodik olarak kontrol eder. Eğer internet bağlantısı yoksa veya belirtilen web sitesine erişim sağlanamıyorsa, ilgili durumları log dosyalarına kaydeder. Ayrıca, çalışan bir işlem olan "ConsoleApp1" adlı uygulamayı kontrol eder ve gerektiğinde sonlandırır.
## Özellikler

- İnternet bağlantısının olup olmadığını kontrol etme.
- Bir Web sitesinin çalışır durumda olup olmadığını kontrol etme.
- Log tutma.
- Cihazda çalışan bir Process aynı anda birden fazla kere çalıştırılmışsa ilk açılan Process hariç hepsini durdurma.

## Kurulum

Servisi kurmak ve çalıştırmak için aşağıdaki adımları izleyin:

1. Bu depoyu klonlayın:
    ```bash
    git clone https://github.com/kadirbeskardes/WindowsServiceChechWeb.git
    ```
2. Projeyi Visual Studio'da açın.
3. Gerekli bağımlılıkları yükleyin.
4. Eğerki internet  bağlantısını değil de bir Web sayfasının ayakta olup olmadığını kontrol etmek istiyorsanız gstatic URL'si yerine kontrol etmek istediğiniz Web sayfasının URL adresini giriniz.
5. Projeyi derleyin ve çalıştırın.
6. Çalıştır'ı (Win+R) açın ve 'services.msc' yazın.
7. 'Service1' isimli servisi başlatın.

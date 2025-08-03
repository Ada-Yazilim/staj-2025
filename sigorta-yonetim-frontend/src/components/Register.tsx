import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import './Login.css';

interface RegisterProps {
  onSwitchToLogin: () => void;
}

const Register: React.FC<RegisterProps> = ({ onSwitchToLogin }) => {
  const [formData, setFormData] = useState({
    ad: '',
    soyad: '',
    email: '',
    password: '',
    confirmPassword: '',
    telefon: '',
    // Müşteri bilgileri - artık tüm müşteriler için ortak
    tcKimlikNo: '',
    dogumTarihi: '',
    cinsiyet: 1, // Varsayılan: Erkek
    medeniDurum: 1, // Varsayılan: Bekar
    meslek: '',
    egitimDurumu: 1, // Varsayılan: İlkokul
    aylikGelir: '',
    adresIl: '',
    adresIlce: '',
    adresMahalle: '',
    adresDetay: '',
    postaKodu: ''
  });
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const { register } = useAuth();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    let value = e.target.value;
    
    // Ad ve Soyad validasyonu - sadece harf, boşluk ve Türkçe karakterler
    if (e.target.name === 'ad' || e.target.name === 'soyad') {
      // Sadece harf, boşluk ve Türkçe karakterlere izin ver
      value = value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
      // Birden fazla boşluğu tek boşluğa çevir
      value = value.replace(/\s+/g, ' ');
      // Başındaki ve sonundaki boşlukları kaldır
      value = value.trim();
    }
    
    // Telefon numarası - sadece rakam
    if (e.target.name === 'telefon') {
      value = value.replace(/\D/g, '');
      if (value.length > 10) {
        value = value.substring(0, 10);
      }
    }

    // TC Kimlik No - sadece rakam
    if (e.target.name === 'tcKimlikNo') {
      value = value.replace(/\D/g, '');
      if (value.length > 11) {
        value = value.substring(0, 11);
      }
    }

    // Meslek validasyonu - sadece harf ve boşluk
    if (e.target.name === 'meslek') {
      value = value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
      value = value.replace(/\s+/g, ' ');
      value = value.trim();
    }

    // Adres alanları validasyonu
    if (e.target.name === 'adresIl' || e.target.name === 'adresIlce') {
      value = value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ\s]/g, '');
      value = value.replace(/\s+/g, ' ');
      value = value.trim();
    }

    if (e.target.name === 'adresMahalle') {
      value = value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s]/g, '');
      value = value.replace(/\s+/g, ' ');
      value = value.trim();
    }

    if (e.target.name === 'adresDetay') {
      value = value.replace(/[^a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s.,/-]/g, '');
      value = value.replace(/\s+/g, ' ');
      value = value.trim();
    }

    // Posta kodu - sadece rakam
    if (e.target.name === 'postaKodu') {
      value = value.replace(/\D/g, '');
    }

    setFormData(prev => ({
      ...prev,
      [e.target.name]: value
    }));
  };

  const validateForm = () => {
    // Zorunlu alanlar kontrolü
    if (!formData.ad.trim()) {
      setMessage('Ad alanı zorunludur');
      return false;
    }

    if (!formData.soyad.trim()) {
      setMessage('Soyad alanı zorunludur');
      return false;
    }

    if (!formData.email.trim()) {
      setMessage('E-posta alanı zorunludur');
      return false;
    }

    if (!formData.telefon.trim()) {
      setMessage('Telefon alanı zorunludur');
      return false;
    }

    if (!formData.adresIl.trim()) {
      setMessage('İl alanı zorunludur');
      return false;
    }

    if (!formData.adresIlce.trim()) {
      setMessage('İlçe alanı zorunludur');
      return false;
    }

    if (!formData.adresMahalle.trim()) {
      setMessage('Mahalle alanı zorunludur');
      return false;
    }

    if (!formData.adresDetay.trim()) {
      setMessage('Adres detayı zorunludur');
      return false;
    }

    // Ad ve Soyad validasyonu
    if (formData.ad.length < 2) {
      setMessage('Ad en az 2 karakter olmalıdır');
      return false;
    }

    if (formData.soyad.length < 2) {
      setMessage('Soyad en az 2 karakter olmalıdır');
      return false;
    }

    // Ad ve Soyad sadece harf kontrolü
    const nameRegex = /^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$/;
    if (!nameRegex.test(formData.ad)) {
      setMessage('Ad sadece harf içerebilir');
      return false;
    }

    if (!nameRegex.test(formData.soyad)) {
      setMessage('Soyad sadece harf içerebilir');
      return false;
    }

    // E-posta validasyonu
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      setMessage('Geçerli bir e-posta adresi giriniz');
      return false;
    }

    // Şifre validasyonu
    if (formData.password !== formData.confirmPassword) {
      setMessage('Şifreler eşleşmiyor');
      return false;
    }

    if (formData.password.length < 6) {
      setMessage('Şifre en az 6 karakter olmalıdır');
      return false;
    }

    // Telefon validasyonu
    const digitsOnly = formData.telefon.replace(/\D/g, '');
    let phoneDigits = digitsOnly;
    if (phoneDigits.startsWith('90')) {
      phoneDigits = phoneDigits.substring(2);
    }
    if (phoneDigits.startsWith('0')) {
      phoneDigits = phoneDigits.substring(1);
    }
    
    if (phoneDigits.length !== 10 || (!phoneDigits.startsWith('5') && !phoneDigits.startsWith('2'))) {
      setMessage('Geçerli bir Türkiye telefon numarası giriniz (Örn: 0532 123 45 67)');
      return false;
    }

    // TC Kimlik No validasyonu
    if (formData.tcKimlikNo) {
      const tcDigits = formData.tcKimlikNo.replace(/\D/g, '');
      if (tcDigits.length !== 11) {
        setMessage('TC Kimlik No 11 haneli olmalıdır');
        return false;
      }
      if (tcDigits[0] === '0') {
        setMessage('TC Kimlik No 0 ile başlayamaz');
        return false;
      }
      if (tcDigits[0] === '1' && tcDigits[1] === '1' && tcDigits[2] === '1' && tcDigits[3] === '1' && tcDigits[4] === '1' && tcDigits[5] === '1' && tcDigits[6] === '1' && tcDigits[7] === '1' && tcDigits[8] === '1' && tcDigits[9] === '1' && tcDigits[10] === '1') {
        setMessage('Geçersiz TC Kimlik No');
        return false;
      }
    }

    // Meslek validasyonu
    if (formData.meslek) {
      const meslekRegex = /^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$/;
      if (!meslekRegex.test(formData.meslek)) {
        setMessage('Meslek sadece harf ve boşluk içerebilir');
        return false;
      }
    }

    // Adres alanları validasyonu
    if (formData.adresIl) {
      const adresIlRegex = /^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$/;
      if (!adresIlRegex.test(formData.adresIl)) {
        setMessage('İl adı sadece harf ve boşluk içerebilir');
        return false;
      }
    }

    if (formData.adresIlce) {
      const adresIlceRegex = /^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$/;
      if (!adresIlceRegex.test(formData.adresIlce)) {
        setMessage('İlçe adı sadece harf ve boşluk içerebilir');
        return false;
      }
    }

    if (formData.adresMahalle) {
      const adresMahalleRegex = /^[a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s]+$/;
      if (!adresMahalleRegex.test(formData.adresMahalle)) {
        setMessage('Mahalle adı sadece harf ve rakam içerebilir');
        return false;
      }
    }

    if (formData.adresDetay) {
      const adresDetayRegex = /^[a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s.,/-]+$/;
      if (!adresDetayRegex.test(formData.adresDetay)) {
        setMessage('Açık adres sadece harf, rakam, boşluk, nokta, virgül, tire ve / içerebilir');
        return false;
      }
    }

    if (formData.postaKodu) {
      const postaKoduRegex = /^\d+$/;
      if (!postaKoduRegex.test(formData.postaKodu)) {
        setMessage('Posta kodu sadece rakam içerebilir');
        return false;
      }
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage('');

    console.log('Form validasyonu başlıyor...');
    if (!validateForm()) {
      console.log('Form validasyonu başarısız');
      setLoading(false);
      return;
    }

    console.log('Form validasyonu başarılı, register data hazırlanıyor...');
    // Register data'yı hazırla - boş string'leri undefined yap
    const registerData = {
      ad: formData.ad,
      soyad: formData.soyad,
      email: formData.email,
      password: formData.password,
      confirmPassword: formData.confirmPassword,
      telefon: formData.telefon,
      // Müşteri bilgileri - artık tüm müşteriler için ortak
      tcKimlikNo: formData.tcKimlikNo || undefined,
      dogumTarihi: formData.dogumTarihi || undefined,
      cinsiyet: formData.cinsiyet,
      medeniDurum: formData.medeniDurum,
      meslek: formData.meslek || undefined,
      egitimDurumu: formData.egitimDurumu,
      aylikGelir: formData.aylikGelir ? parseFloat(formData.aylikGelir) : undefined,
      adresIl: formData.adresIl,
      adresIlce: formData.adresIlce,
      adresMahalle: formData.adresMahalle,
      adresDetay: formData.adresDetay,
      postaKodu: formData.postaKodu || undefined
    };

    console.log('Register data:', registerData);

    try {
      console.log('Register API çağrısı yapılıyor...');
      const result = await register(registerData);
      console.log('Register API yanıtı:', result);

      if (result.success) {
        console.log('Kayıt başarılı!');
        setMessage('Kayıt başarılı! Giriş yapabilirsiniz.');
        // Başarılı kayıt sonrası login sayfasına yönlendir
        setTimeout(() => {
          onSwitchToLogin();
        }, 2000);
      } else {
        console.log('Kayıt başarısız:', result.message);
        setMessage(`Kayıt başarısız: ${result.message}`);
      }
    } catch (error) {
      console.error('Register API hatası:', error);
      setMessage('Sunucu hatası oluştu. Lütfen tekrar deneyin.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card register-card">
        <h2>Sigorta Yönetim Platformu</h2>
        <h3>Kayıt Ol</h3>
        
        <form onSubmit={handleSubmit}>
          {/* Temel Bilgiler */}
          <div className="form-section">
            <h4>👤 Temel Bilgiler</h4>
            <div className="form-row">
              <div className="form-group">
                <label htmlFor="ad">Ad:</label>
                <input
                  type="text"
                  id="ad"
                  name="ad"
                  value={formData.ad}
                  onChange={handleChange}
                  required
                  disabled={loading}
                  placeholder="Adınızı giriniz"
                />
              </div>

              <div className="form-group">
                <label htmlFor="soyad">Soyad:</label>
                <input
                  type="text"
                  id="soyad"
                  name="soyad"
                  value={formData.soyad}
                  onChange={handleChange}
                  required
                  disabled={loading}
                  placeholder="Soyadınızı giriniz"
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="email">E-posta:</label>
                <input
                  type="email"
                  id="email"
                  name="email"
                  value={formData.email}
                  onChange={handleChange}
                  required
                  disabled={loading}
                  placeholder="E-posta adresinizi giriniz"
                />
              </div>

              <div className="form-group">
                <label htmlFor="telefon">Telefon:</label>
                <input
                  type="tel"
                  id="telefon"
                  name="telefon"
                  value={formData.telefon}
                  onChange={handleChange}
                  disabled={loading}
                  placeholder="5XX XXX XX XX (10 haneli)"
                  maxLength={10}
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="password">Şifre:</label>
                <input
                  type="password"
                  id="password"
                  name="password"
                  value={formData.password}
                  onChange={handleChange}
                  required
                  disabled={loading}
                  placeholder="Şifrenizi giriniz (en az 6 karakter)"
                />
              </div>

              <div className="form-group">
                <label htmlFor="confirmPassword">Şifre Tekrarı:</label>
                <input
                  type="password"
                  id="confirmPassword"
                  name="confirmPassword"
                  value={formData.confirmPassword}
                  onChange={handleChange}
                  required
                  disabled={loading}
                  placeholder="Şifrenizi tekrar giriniz"
                />
              </div>
            </div>
          </div>

          {/* Kimlik Bilgileri */}
          <div className="form-section">
            <h4>🆔 Kimlik Bilgileri</h4>
            <div className="form-row">
                <div className="form-group">
                  <label htmlFor="tcKimlikNo">TC Kimlik No:</label>
                  <input
                    type="text"
                    id="tcKimlikNo"
                    name="tcKimlikNo"
                    value={formData.tcKimlikNo}
                    onChange={handleChange}
                    disabled={loading}
                    placeholder="11 haneli TC Kimlik No"
                    maxLength={11}
                  />
                </div>
            </div>
          </div>

          {/* Kişisel Bilgiler */}
            <div className="form-section">
              <h4>👤 Kişisel Bilgiler</h4>
              <div className="form-row">
                <div className="form-group">
                  <label htmlFor="dogumTarihi">Doğum Tarihi:</label>
                  <input
                    type="date"
                    id="dogumTarihi"
                    name="dogumTarihi"
                    value={formData.dogumTarihi}
                    onChange={handleChange}
                    disabled={loading}
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="cinsiyet">Cinsiyet:</label>
                  <select
                    id="cinsiyet"
                    name="cinsiyet"
                    value={formData.cinsiyet}
                    onChange={handleChange}
                    disabled={loading}
                  >
                    <option value={1}>Erkek</option>
                    <option value={2}>Kadın</option>
                  </select>
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label htmlFor="medeniDurum">Medeni Durum:</label>
                  <select
                    id="medeniDurum"
                    name="medeniDurum"
                    value={formData.medeniDurum}
                    onChange={handleChange}
                    disabled={loading}
                  >
                    <option value={1}>Bekar</option>
                    <option value={2}>Evli</option>
                    <option value={3}>Boşanmış</option>
                  </select>
                </div>

                <div className="form-group">
                  <label htmlFor="egitimDurumu">Eğitim Durumu:</label>
                  <select
                    id="egitimDurumu"
                    name="egitimDurumu"
                    value={formData.egitimDurumu}
                    onChange={handleChange}
                    disabled={loading}
                  >
                    <option value={1}>İlkokul</option>
                    <option value={2}>Ortaokul</option>
                    <option value={3}>Lise</option>
                    <option value={4}>Üniversite</option>
                    <option value={5}>Yüksek Lisans</option>
                    <option value={6}>Doktora</option>
                  </select>
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label htmlFor="meslek">Meslek:</label>
                  <input
                    type="text"
                    id="meslek"
                    name="meslek"
                    value={formData.meslek}
                    onChange={handleChange}
                    disabled={loading}
                    placeholder="Mesleğinizi giriniz"
                  />
                </div>

                <div className="form-group">
                  <label htmlFor="aylikGelir">Aylık Gelir (₺):</label>
                  <input
                    type="number"
                    id="aylikGelir"
                    name="aylikGelir"
                    value={formData.aylikGelir}
                    onChange={handleChange}
                    disabled={loading}
                    placeholder="Aylık gelirinizi giriniz"
                    min="0"
                  />
                </div>
              </div>
            </div>

          {/* Adres Bilgileri */}
          <div className="form-section">
            <h4>📍 Adres Bilgileri</h4>
            <div className="form-row">
              <div className="form-group">
                <label htmlFor="adresIl">İl:</label>
                <input
                  type="text"
                  id="adresIl"
                  name="adresIl"
                  value={formData.adresIl}
                  onChange={handleChange}
                  disabled={loading}
                  placeholder="İl adını giriniz"
                />
              </div>

              <div className="form-group">
                <label htmlFor="adresIlce">İlçe:</label>
                <input
                  type="text"
                  id="adresIlce"
                  name="adresIlce"
                  value={formData.adresIlce}
                  onChange={handleChange}
                  disabled={loading}
                  placeholder="İlçe adını giriniz"
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="adresMahalle">Mahalle:</label>
                <input
                  type="text"
                  id="adresMahalle"
                  name="adresMahalle"
                  value={formData.adresMahalle}
                  onChange={handleChange}
                  disabled={loading}
                  placeholder="Mahalle adını giriniz"
                />
              </div>

              <div className="form-group">
                <label htmlFor="postaKodu">Posta Kodu:</label>
                <input
                  type="text"
                  id="postaKodu"
                  name="postaKodu"
                  value={formData.postaKodu}
                  onChange={handleChange}
                  disabled={loading}
                  placeholder="Posta kodunu giriniz"
                />
              </div>
            </div>

            <div className="form-group">
              <label htmlFor="adresDetay">Açık Adres:</label>
              <textarea
                id="adresDetay"
                name="adresDetay"
                value={formData.adresDetay}
                onChange={handleChange}
                disabled={loading}
                placeholder="Sokak, cadde, bina no, daire no vb."
                rows={3}
              />
            </div>
          </div>

          {message && (
            <div className={message.toLowerCase().includes('başarısız') || message.toLowerCase().includes('hata') ? 'error-message' : 'success-message'}>
              {message}
            </div>
          )}

          <button 
            type="submit" 
            disabled={loading}
            className="login-button"
          >
            {loading ? 'Kayıt yapılıyor...' : 'Kayıt Ol'}
          </button>
        </form>

        <div className="auth-switch">
          <p>Zaten hesabınız var mı?</p>
          <button 
            type="button" 
            onClick={onSwitchToLogin}
            className="switch-button"
          >
            Giriş Yap
          </button>
        </div>
      </div>
    </div>
  );
};

export default Register; 